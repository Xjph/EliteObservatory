using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;

namespace EDDisco
{
    class LogMonitor
    {
        private readonly FileSystemWatcher logWatcher;
        public string CurrentSystem { get; private set; }
        public string CurrentLogPath { get; private set; }
        public string CurrentLogLine { get; private set; }
        private string PreviousScan;
        private string LogDirectory;
        private string JsonException;
        public bool LastScanValid { get; private set; }
        public bool ReadAllInProgress { get; private set; }
        public bool ReadAllComplete { get; private set; }
        public ScanEvent LastScan { get; private set; }
        public Dictionary<(string, long), ScanEvent> SystemBody { get; private set; }
        private JournalPoker Poker;
        
        public LogMonitor(string logPath)
        {
            
            LogDirectory = logPath;
            LogDirectory = CheckLogPath();
            logWatcher = new FileSystemWatcher(LogDirectory, "Journal.????????????.??.log")
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName
            };
            logWatcher.Changed += LogChanged;
            logWatcher.Created += LogChanged;
            SystemBody = new Dictionary<(string, long), ScanEvent>();
            ReadAllInProgress = false;
            ReadAllComplete = false;
        }

        public void MonitorStart()
        {
            logWatcher.EnableRaisingEvents = true;
            Poker = new JournalPoker(LogDirectory);
            Poker.Start();
        }

        public void MonitorStop()
        {
            logWatcher.EnableRaisingEvents = false;
            Poker.Stop();
            Poker = null;
        }

        public void ReadAll(ProgressBar progressBar)
        {
            ReadAllInProgress = true;
            progressBar.Visible = true;
            DirectoryInfo logDir = new DirectoryInfo(CheckLogPath());
            FileInfo[] allJournals = logDir.GetFiles("Journal.????????????.??.log");
            int progress = 0;
            foreach (var journalFile in allJournals)
            {
                using (StreamReader currentLog = new StreamReader(File.Open(journalFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                {
                    while (!currentLog.EndOfStream)
                    {
                        CurrentLogLine = currentLog.ReadLine();

                        ProcessLine();
                    }
                }
                progressBar.Value = (progress++ * 100) / allJournals.Count();
                progressBar.Refresh();
            }
            progressBar.Visible = false;
            ReadAllInProgress = false;
            ReadAllComplete = true;
        }

        public bool IsMonitoring()
        {
            return logWatcher.EnableRaisingEvents;
        }

        private string CheckLogPath()
        {
            LogDirectory = string.IsNullOrEmpty(LogDirectory) ? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Saved Games\\Frontier Developments\\Elite Dangerous" : LogDirectory;
            if (!Directory.Exists(LogDirectory) || new DirectoryInfo(LogDirectory).GetFiles("Journal.????????????.??.log").Count() == 0)
            {
                System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog
                {
                    RootFolder = Environment.SpecialFolder.MyComputer,
                    ShowNewFolderButton = false,
                    Description = "Select Elite Dangerous Journal Folder"
                };
                         
                System.Windows.Forms.DialogResult result = folderBrowserDialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    LogDirectory = folderBrowserDialog.SelectedPath;
                }
            }
            return LogDirectory;
        }

        private void LogChanged(object source, FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    CurrentLogPath = e.FullPath;
                    CurrentLogPath = string.Empty;
                    break;

                case WatcherChangeTypes.Changed:
                    CurrentLogPath = e.FullPath;
                    bool updateScan = false;
                    using (StreamReader currentLog = new StreamReader(File.Open(CurrentLogPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                    {
                        while (!currentLog.EndOfStream)
                        {
                            string checkLine = currentLog.ReadLine();
                            if (checkLine.Contains("\"event\":\"Scan\"") && checkLine != PreviousScan)
                            {
                                CurrentLogLine = checkLine;
                                updateScan = true;
                            }
                            else if (checkLine.Contains("\"event\":\"Location\"") || checkLine.Contains("\"event\":\"FSDJump\""))
                            {
                                CurrentLogLine = checkLine;
                            }
                        }
                    }
                    if (updateScan)
                    {
                        PreviousScan = CurrentLogLine;
                    }
                    ProcessLine();

                    break;

                default:
                    break;
            }
        }

        private void ProcessLine()
        {
            try
            {
                if (CurrentLogLine != null)
                {
                    JObject lastEvent = JObject.Parse(CurrentLogLine);
                    LastScanValid = false;
                    //Journals prior to Elite Dangerous 2.3 "The Commanders" had differently formatted scan events which I can't be bothered to support.
                    if (DateTime.Parse(lastEvent["timestamp"].ToString(), null, System.Globalization.DateTimeStyles.RoundtripKind) > new DateTime(2017, 04, 12))
                    {
                        switch (lastEvent["event"].ToString())
                        {
                            case "Scan":
                                LastScan = lastEvent.ToObject<ScanEvent>();
                                SystemBody[(CurrentSystem, LastScan.BodyId)] = LastScan;
                                LastScanValid = true;
                                break;
                            case "FSDJump":
                            case "Location":
                                CurrentSystem = lastEvent["StarSystem"].ToString();
                                break;
                            case "FSSDiscoveryScan":
                            case "FSSAllBodiesFound":
                                if (lastEvent["SystemName"] != null)
                                {
                                    CurrentSystem = lastEvent["SystemName"].ToString();
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            //Garbage data from unexpected games crashes during journal writes can happen, ignore it.
            catch (Newtonsoft.Json.JsonReaderException ex)
            {
                JsonException = ex.Message;
                return;
            }

            EventHandler entry = LogEntry;
            entry?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler LogEntry;
    }
}
