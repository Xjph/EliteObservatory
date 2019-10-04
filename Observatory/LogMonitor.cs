using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Observatory
{
    class LogMonitor
    {
        private readonly FileSystemWatcher logWatcher;
        public string CurrentSystem { get; private set; }
        public string CurrentLogPath { get; private set; }
        public string CurrentLogLine { get; private set; }
        private List<string> LinesToProcess;
        private string LogDirectory;
        public bool LastScanValid { get; private set; }
        public bool ReadAllInProgress { get; private set; }
        public bool ReadAllComplete { get; private set; }
        public ScanEvent LastScan { get; private set; }
        public Dictionary<(string, long), ScanEvent> SystemBody { get; private set; }
        public UserInterest UserInterest { get; private set; }
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
            CurrentSystem = string.Empty;
        }

        public void MonitorStart()
        {
            UserInterest = new UserInterest();
            logWatcher.EnableRaisingEvents = true;
            Poker = new JournalPoker(LogDirectory);
            Poker.Start();
        }

        public void MonitorStop()
        {
            UserInterest = null;
            logWatcher.EnableRaisingEvents = false;
            Poker.Stop();
            Poker = null;
        }

        public void ReadAll(ProgressBar progressBar)
        {
            UserInterest = new UserInterest();
            ReadAllInProgress = true;
            progressBar.Visible = true;
            DirectoryInfo logDir = new DirectoryInfo(CheckLogPath());
            FileInfo[] allJournals = logDir.GetFiles("Journal.????????????.??.log");
            int progress = 0;
            try
            {
                foreach (var journalFile in allJournals)
                {
                    using (StreamReader currentLog = new StreamReader(File.Open(journalFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                    {
                        while (!currentLog.EndOfStream)
                        {
                            CurrentLogLine = currentLog.ReadLine();
                            if (CurrentLogLine.Trim().StartsWith("{") && CurrentLogLine.Trim().EndsWith("}") && CurrentLogLine.Contains("\"event\":\"Scan\"") || CurrentLogLine.Contains("\"event\":\"Location\"") || CurrentLogLine.Contains("\"event\":\"FSDJump\""))
                            {
                                ProcessLine(CurrentLogLine);
                            }
                        }
                    }
                    progressBar.Value = (progress++ * 100) / allJournals.Count();
                    progressBar.Refresh();
                }
            }
            catch (Exception ex)
            {
                DialogResult response = MessageBox.Show("An error has occured while reading the log files. Would you like to see additional error detail?", "Error Reading Logs", MessageBoxButtons.YesNo);
                if (response == DialogResult.Yes)
                {
                    MessageBox.Show($"Journal Line: {CurrentLogLine}\r\nException message: {ex.Message}\r\nStack trace: {ex.StackTrace}", "Error Detail", MessageBoxButtons.OK);
                }
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
                   
                    using (StreamReader currentLog = new StreamReader(File.Open(CurrentLogPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                    {
                        LinesToProcess = new List<string>();
                        while (!currentLog.EndOfStream)
                        {
                            string checkLine = currentLog.ReadLine();
                            if (checkLine.Contains("\"ScanType\":\"AutoScan\"") && checkLine.Contains(CurrentSystem) || CurrentSystem == string.Empty)
                            {
                                LinesToProcess.Add(checkLine);
                            }
                            else if (checkLine.Contains("\"event\":\"Scan\""))
                            {
                                CurrentLogLine = checkLine;
                            }
                            else if (checkLine.Contains("\"event\":\"Location\"") || checkLine.Contains("\"event\":\"FSDJump\""))
                            {
                                CurrentLogLine = checkLine;
                            }
                        }

                        LinesToProcess.Add(CurrentLogLine);
                    }

                    foreach (string line in LinesToProcess)
                    {
                        ProcessLine(line);
                    }

                    break;

                default:
                    break;
            }
        }

        private void ProcessLine(string logLine)
        {
   
            if (logLine != null)
            {
                JObject lastEvent = (JObject)JsonConvert.DeserializeObject(logLine, new JsonSerializerSettings() { DateParseHandling = DateParseHandling.None });
                LastScanValid = false;
                //Journals prior to Elite Dangerous 2.3 "The Commanders" had differently formatted scan events which I can't be bothered to support.
                if (DateTime.TryParseExact(lastEvent["timestamp"].ToString(), "yyyy-MM-ddTHH:mm:ssZ", null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime eventTime))
                {
                    if (eventTime > new DateTime(2017, 04, 12))
                    {
                        switch (lastEvent["event"].ToString())
                        {
                            case "Scan":
                                if (!lastEvent["BodyName"].ToString().Contains("Belt Cluster"))
                                {
                                    LastScan = lastEvent.ToObject<ScanEvent>();
                                    if (!SystemBody.ContainsKey((CurrentSystem, LastScan.BodyId)))
                                    {
                                        SystemBody[(CurrentSystem, LastScan.BodyId)] = LastScan;
                                        LastScanValid = true;
                                    }
                                }
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
                else
                {
                    throw new Exception($"Error parsing journal date on line: {logLine}. Process aborted.");
                }
            }


            EventHandler entry = LogEntry;
            entry?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler LogEntry;
    }
}
