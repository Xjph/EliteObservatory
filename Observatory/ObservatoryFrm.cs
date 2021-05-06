using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Windows.Forms;
using System.Speech.Synthesis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PicoXLSX;

namespace Observatory
{
    public partial class ObservatoryFrm : Form
    {
        private LogMonitor logMonitor;
        public SpeechSynthesizer speech;
        private SettingsFrm settingsFrm;
        private ListViewColumnSorter columnSorter;
        public bool settingsOpen = false;
        private CapiState capiState;
        private CompanionAPI cAPI;
        private string lastSystem = string.Empty;
        private EDOverlay overlay;

        public ObservatoryFrm()
        {
            InitializeComponent();
            ExtendControl.DoubleBuffered(listEvent, true);
            Text = $"{Text} - v{Application.ProductVersion}";
            logMonitor = new LogMonitor();
            logMonitor.LogEntry += LogEvent;
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            columnSorter = new ListViewColumnSorter();
            listEvent.ListViewItemSorter = columnSorter;
            capiState = Properties.Observatory.Default.UseCapi ? CapiState.Enabled : CapiState.Disabled;
            CheckCapi(capiState);
            overlay = new EDOverlay();

            if (Properties.Observatory.Default.TTS)
            {
                speech = new SpeechSynthesizer();
                speech.SetOutputToDefaultAudioDevice();
            }

            try
            {
                string releasesResponse;

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://api.github.com/repos/xjph/EliteObservatory/releases"),
                    Headers = { { "User-Agent", "Xjph/EliteObservatory" } }
                };

                releasesResponse = HttpClient.SendRequest(request).Content.ReadAsStringAsync().Result;

                if (!string.IsNullOrEmpty(releasesResponse))
                {
                    JArray releases = (JArray)JsonConvert.DeserializeObject(releasesResponse, new JsonSerializerSettings() { DateParseHandling = DateParseHandling.None });
                    foreach (JObject release in releases)
                    {
                        if (release["tag_name"].ToString().CompareTo($"v{Application.ProductVersion}") > 0)
                        {
                            linkUpdate.Enabled = true;
                            linkUpdate.Visible = true;
                            break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking for update: {ex.Message}");
            }

        }

        private void BtnToggleMonitor_Click(object sender, EventArgs e)
        {
            if (settingsOpen)
            {
                MessageBox.Show("Unable to retrieve logs or change monitoring state while settings window is open.", "Settings Open", MessageBoxButtons.OK);
            }
            else
            {
                if (capiState == CapiState.Enabled)
                {
                    
                    cAPI = new CompanionAPI();
                    var capiTask = cAPI.GetJournals(this, ModifierKeys == Keys.Shift);

                }
                else if (capiState == CapiState.InProgress)
                {
                    cAPI.retrieve = false;
                }
                else
                {
                    ToggleMonitor();
                }
            }
        }
            

        private void LogEvent(object source, EventArgs e)
        {
            if (logMonitor.LastScanValid)
            {
                ScanReader scan = new ScanReader(logMonitor);

                if (scan.IsInteresting())
                {
                    Invoke((MethodInvoker)delegate ()
                    {
                        if (!logMonitor.ReadAllInProgress)
                        {
                            RemoveUninteresting();
                        }

                        foreach (var item in scan.Interest)
                        {
                            AddListItem(item);
                        }

                        if (!logMonitor.ReadAllInProgress && scan.Interest.Count > 0)
                        {
                            AnnounceItems(logMonitor.CurrentSystem, scan.Interest);
                        }
                    });
                }
                else if (!logMonitor.ReadAllInProgress)
                {

                    if (Properties.Observatory.Default.AutoClearList && logMonitor.CurrentSystem != lastSystem)
                    {
                        listEvent.Items.Clear();
                        lastSystem = logMonitor.CurrentSystem;
                    }

                    ListViewItem newItem = new ListViewItem(new string[] { scan.Interest[0].BodyName, "Uninteresting", logMonitor.LastScan.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"), string.Empty, string.Empty })
                    {
                        UseItemStyleForSubItems = false
                    };

                    Invoke((MethodInvoker)delegate ()
                    {
                        listEvent.BeginUpdate();
                        RemoveUninteresting();
                        newItem.SubItems[0].ForeColor = Color.DarkGray;
                        newItem.SubItems[1].ForeColor = Color.DarkGray;
                        newItem.SubItems[2].ForeColor = Color.DarkGray;
                        listEvent.Items.Add(newItem).EnsureVisible();
                        listEvent.EndUpdate();
                    });
                }
            }
            else if (logMonitor.LastCodexValid)
            {
                CodexReader.ProcessCodex(logMonitor.LastCodex);

                if (Properties.Observatory.Default.IncludeCodex)
                {
                    Invoke((MethodInvoker)delegate ()
                    {
                        string location;
                        listEvent.BeginUpdate();

                        if (logMonitor.LastCodex.Category != "$Codex_Category_StellarBodies;" && logMonitor.LastCodex.NearestDestinationLocalised?.Length > 0 && logMonitor.LastCodex.Body?.Length > 0)
                            location = logMonitor.LastCodex.Body;
                        else
                            location = logMonitor.LastCodex.System;

                        ListViewItem newItem = new ListViewItem(new string[] { location, logMonitor.LastCodex.NearestDestinationLocalised?.Length > 0 ? logMonitor.LastCodex.NearestDestinationLocalised : "Codex Entry", logMonitor.LastCodex.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"), logMonitor.LastCodex.NameLocalised, string.Empty });
                        listEvent.Items.Add(newItem).EnsureVisible();
                        listEvent.EndUpdate();
                    });
                }
            }
        }

        private void RemoveUninteresting()
        {
            foreach (ListViewItem listItem in listEvent.Items)
            {
                if (listItem.SubItems[1].Text == "Uninteresting")
                {
                    listEvent.Items.Remove(listItem);
                }
            }
        }

        private void AnnounceItems(string currentSystem, List<(string BodyName, string Description, string Detail)> items)
        {
            if (Properties.Observatory.Default.Notify || Properties.Observatory.Default.TTS || Properties.Observatory.Default.EnableTelegram)
            {
                string fullBodyName = items[0].BodyName;
                StringBuilder announceText = new StringBuilder();

                foreach (var item in items)
                {
                    announceText.Append(item.Description);
                    if (!item.Equals(items.Last()))
                    {
                        announceText.AppendLine(", ");
                    }
                }

                if (Properties.Observatory.Default.Notify)
                {
                    if (!Properties.Observatory.Default.UseEDOverlay || !overlay.Send(fullBodyName, announceText.ToString()))
                    {
                        NotifyFrm notifyFrm = new NotifyFrm(fullBodyName + "\r\n" + announceText.ToString());
                        notifyFrm.Show(5000);
                    }
                }

                if (Properties.Observatory.Default.TTS)
                {
                    string spokenName;
                    spokenName = fullBodyName.Replace(currentSystem, string.Empty);

                    if (spokenName.Trim().Length > 0)
                    {
                        if (spokenName.Contains("Ring"))
                        {
                            int ringIndex = spokenName.Length - 6;
                            spokenName = 
                                "Body <say-as interpret-as=\"spell-out\">" + spokenName.Substring(0, ringIndex) + 
                                "</say-as><break strength=\"weak\"/><say-as interpret-as=\"spell-out\">" + 
                                spokenName.Substring(ringIndex, 1) + "</say-as>" + spokenName.Substring(ringIndex + 1, spokenName.Length - (ringIndex + 1));
                        }
                        else
                        {
                            spokenName = "Body <say-as interpret-as=\"spell-out\">" + spokenName + "</say-as>";
                        }
                    }

                    
                    speech.Volume = Properties.Observatory.Default.TTSVolume;
                    speech.SpeakSsmlAsync($"<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"en-US\">{spokenName}:<break strength=\"weak\"/>{announceText}</speak>");
                }

                if (Properties.Observatory.Default.EnableTelegram)
                {
                    try
                    {

                        string message = fullBodyName + "\r\n" + announceText.ToString();
                        var request = new System.Net.Http.HttpRequestMessage
                        {
                            Method = System.Net.Http.HttpMethod.Get,
                            RequestUri = new Uri($"https://api.telegram.org/bot{Properties.Observatory.Default.TelegramAPIKey}/sendMessage?chat_id={Properties.Observatory.Default.TelegramChatId}&text={message}")
                        };

                        HttpClient.SendRequest(request);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error sending Telegram notification: {ex.Message}");
                    }

                }
            }
        }

        private void AddListItem((string BodyName, string Description, string Detail) item)
        {
            ListViewItem newItem = new ListViewItem(
                                        new string[] {
                                        item.BodyName,
                                        item.Description,
                                        logMonitor.LastScan.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                                        item.Detail,
                                        (logMonitor.LastScan.Landable.GetValueOrDefault(false) && !item.Description.Contains("materials in system")) ? "🌐" : string.Empty
                                        });
            if (item.Description.Contains("Multiple criteria"))
            {
                newItem.UseItemStyleForSubItems = false;
                newItem.SubItems[1].Font = new Font(newItem.Font, FontStyle.Bold);
            }

            listEvent.Items.Add(newItem);
            if (!logMonitor.ReadAllInProgress)
            {
                listEvent.Sort();
                newItem.EnsureVisible();
            }
        }

        private void BtnReadAll_Click(object sender, EventArgs e)
        {
            if (logMonitor.ReadAllComplete)
            {
                DialogResult confirmResult;
                confirmResult = MessageBox.Show("This will clear the current list and re-read all journal logs. Contine?", "Confirm Refresh", MessageBoxButtons.OKCancel);
                if (confirmResult == DialogResult.Cancel)
                {
                    return;
                }
            }
            ReadAllJournals();
        }

        private void ReadAllJournals()
        {
            bool resumeMonitor = false;

            if (logMonitor.IsMonitoring())
            {
                logMonitor.MonitorStop();
                resumeMonitor = true;
            }

            listEvent.BeginUpdate();
            listEvent.Items.Clear();
            logMonitor.SystemBody.Clear();
            listEvent.ListViewItemSorter = null;
            logMonitor.ReadAll(progressReadAll);
            listEvent.ListViewItemSorter = columnSorter;
            listEvent.Sort();
            listEvent.EndUpdate();

            if (resumeMonitor)
            {
                logMonitor.MonitorStart();
            }
        }

        private void ListEvent_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listEvent.FocusedItem.Bounds.Contains(e.Location))
                {
                    contextCopy.Show(Cursor.Position);
                    contextCopy.Items[0].Enabled = listEvent.SelectedItems.Count == 1;

                    bool enableMark = false;
                    foreach (ListViewItem item in listEvent.SelectedItems)
                    {
                        if (item.SubItems[1].Text == "Uninteresting")
                        {
                            enableMark = true;
                            break;
                        }
                    }

                    
                    contextCopy.Items[3].Enabled = enableMark;
                    
                }
            }
        }

        private void CopyNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(listEvent.FocusedItem.Text);
        }

        private void CopyAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyAllSelected();
        }

        private void ListEvent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C && e.Control)
            {
                CopyAllSelected();
            }
        }

        private void CopyAllSelected()
        {
            StringBuilder copyText = new StringBuilder();
            foreach (ListViewItem item in listEvent.SelectedItems)
            {
                if (item.SubItems.Count == 5)
                {
                    copyText.AppendLine(
                        Properties.Observatory.Default.CopyTemplate
                            .Replace("%body%", item.SubItems[0].Text)
                            .Replace("%bodyL%", item.SubItems[0].Text + (item.SubItems[4].Text.Length > 0 ? " - Landable" : string.Empty))
                            .Replace("%info%", item.SubItems[1].Text)
                            .Replace("%time%", item.SubItems[2].Text)
                            .Replace("%detail%", item.SubItems[3].Text)
                            .TrimEnd("- ".ToCharArray())
                            );
                }
                else
                {
                    copyText.AppendLine(item.SubItems[0].Text + " - Uninteresting");
                }
  
            }
            Clipboard.SetText(copyText.ToString());
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            if (!settingsOpen)
            {
                settingsFrm = new SettingsFrm(this, logMonitor.IsMonitoring());
                settingsFrm.Show();
                settingsOpen = true;
            }
            else
            {
                settingsFrm.Activate();
            }
        }

        private void LinkUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Xjph/EliteObservatory/releases");
        }

        private void ObservatoryFrm_Load(object sender, EventArgs e)
        {
            if (Properties.Observatory.Default.WindowSize.Height != 0)
            {
                bool offScreen = true;
                Screen[] screens = Screen.AllScreens;
                foreach (Screen screen in screens)
                {
                    Rectangle formRectangle = new Rectangle(Properties.Observatory.Default.WindowLocation, Properties.Observatory.Default.WindowSize);

                    //Shrink rectangle used for position check slightly to allow some wiggle room if positioned slightly off-screen
                    formRectangle.X += 10;
                    formRectangle.Y += 10;
                    formRectangle.Height -= 20;
                    formRectangle.Width -= 20;

                    if (screen.WorkingArea.Contains(formRectangle))
                    {
                        offScreen = false;
                    }
                }

                if (!offScreen)
                {
                    Location = Properties.Observatory.Default.WindowLocation;
                    Size = Properties.Observatory.Default.WindowSize;
                }
            }
        }

        private void ObservatoryFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            speech?.Dispose();
            overlay?.Close();
        }

        private void ListEvent_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == columnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (columnSorter.Order == SortOrder.Ascending)
                {
                    columnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    columnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                columnSorter.SortColumn = e.Column;
                columnSorter.Order = SortOrder.Ascending;
            }
            listEvent.Sort();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            bool resumeMonitor = logMonitor.IsMonitoring();
            listEvent.Items.Clear();
            logMonitor.MonitorStop();
            logMonitor = new LogMonitor();
            logMonitor.LogEntry += LogEvent;
            if (resumeMonitor)
            {
                ToggleMonitor();
            }
        }

        private void ToggleMonitor()
        {
            if (logMonitor.IsMonitoring())
            {
                logMonitor.MonitorStop();
                btnToggleMonitor.Text = "Start Monitoring";
            }
            else
            {
                logMonitor.MonitorStart();
                btnToggleMonitor.Text = "Stop Monitoring";
            }
        }

        private void ObservatoryFrm_Shown(object sender, EventArgs e)
        {
            if (Properties.Observatory.Default.AutoRead)
                ReadAllJournals();

            if (Properties.Observatory.Default.AutoMonitor)
                ToggleMonitor();
        }

        private void CopyJournalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder copyText = new StringBuilder();
            foreach (ListViewItem item in listEvent.SelectedItems)
            {
                var bodyData = logMonitor.SystemBody.Where(body => body.Value.BodyName == item.SubItems[0].Text);
                if (bodyData.Count() > 0)
                    copyText.AppendLine(logMonitor.SystemBody.Where(body => body.Value.BodyName == item.SubItems[0].Text).First().Value.JournalEntry);
            }
            if (copyText.Length == 0)
            {
                MessageBox.Show("No Journal entries in selected rows to copy.", "No Data", MessageBoxButtons.OK);
            }
            else
            {
                Clipboard.SetText(copyText.ToString());
            }
        }

        public void SetStatusText(string text)
        {
            lblStatus.Visible = text.Length > 0;
            lblStatus.Text = text;
        }

        public void CheckCapi(CapiState capiState)
        {
            if (capiState == CapiState.Enabled)
            {
                btnToggleMonitor.Text = "Retrieve Logs";
            }
            else if (capiState == CapiState.Disabled)
            {
                btnToggleMonitor.Text = $"{(logMonitor.IsMonitoring() ? "Stop" : "Start")} Monitoring";
            }
            else if (capiState == CapiState.InProgress)
            {
                btnToggleMonitor.Text = "Cancel Retrieval";
            }
            this.capiState = capiState;
        }

        public enum CapiState
        { Disabled, Enabled, InProgress }

        private void MarkAsInterestingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listEvent.SelectedItems)
            {

                if (item.SubItems[1].Text == "Uninteresting")
                {
                    item.SubItems[1].Text = "Interesting";
                    item.SubItems[3].Text = "Manually Marked As Interesting";
                    item.SubItems[0].ForeColor = Color.Black;
                    item.SubItems[1].ForeColor = Color.Black;
                    item.SubItems[2].ForeColor = Color.Black;
                }
            }
        }

        private void ObservatoryFrm_ResizeEnd(object sender, EventArgs e)
        {
            Properties.Observatory.Default.WindowSize = Size;
            Properties.Observatory.Default.WindowLocation = Location;
            Properties.Observatory.Default.Save();
        }

        private void ExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportLinesToSpreadsheet(listEvent.SelectedItems.OfType<ListViewItem>().Where(item => item.SubItems.Count == 5));
        }

        private void ExportAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportLinesToSpreadsheet(listEvent.Items.OfType<ListViewItem>().Where(item => item.SubItems.Count == 5));
        }

        private void ExportLinesToSpreadsheet(IEnumerable<ListViewItem> items)
        {
            if (items.Count() > 0)
            {
                var saveFile = new SaveFileDialog();
                saveFile.Title = "Choose Export Location";
                saveFile.Filter = "Office Open XML Spreadsheet (*.xlsx)|*.xlsx|Semicolon Delimited Text (*.csv)|*.csv";
                saveFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    if (saveFile.FilterIndex == 1)
                    {
                        var exportSheet = new Workbook(saveFile.FileName, "Observatory Export");
                        int exportRow = 0;

                        foreach (ListViewItem item in items)
                        {
                            exportRow++;
                            exportSheet.WS.Value(
                                DateTime.Parse(item.SubItems[2].Text),
                                new Style()
                                {
                                    CurrentNumberFormat = new Style.NumberFormat()
                                    {
                                        Number = Style.NumberFormat.FormatNumber.custom,
                                        CustomFormatID = 165,
                                        CustomFormatCode = @"yyyy/mm/dd\ hh:mm:ss"
                                    }
                                });

                            exportSheet.WS.Value(item.SubItems[0].Text);
                            exportSheet.WS.Value(item.SubItems[4].Text.Length > 0 ? "Landable" : string.Empty);
                            exportSheet.WS.Value(item.SubItems[1].Text);
                            exportSheet.WS.Value(item.SubItems[3].Text);
                            exportSheet.WS.Down();
                        }

                        exportSheet.Worksheets[0].Columns.Add(0, new Worksheet.Column() { Width = 20.0f });
                        exportSheet.Worksheets[0].Columns.Add(1, new Worksheet.Column() { Width = 30.0f });
                        exportSheet.Worksheets[0].Columns.Add(2, new Worksheet.Column() { Width = 10.0f });
                        exportSheet.Worksheets[0].Columns.Add(3, new Worksheet.Column() { Width = 35.0f });
                        exportSheet.Worksheets[0].Columns.Add(4, new Worksheet.Column() { Width = 60.0f });
                        exportSheet.Save();
                    }
                    else
                    {
                        StringBuilder csvBuilder = new StringBuilder();

                        foreach (ListViewItem item in items)
                        {
                            csvBuilder.Append(item.SubItems[2].Text + ";");
                            csvBuilder.Append(item.SubItems[0].Text + ";");
                            csvBuilder.Append((item.SubItems[4].Text.Length > 0 ? "Landable" : string.Empty) + ";");
                            csvBuilder.Append(item.SubItems[1].Text + ";");
                            csvBuilder.AppendLine(item.SubItems[3].Text);
                        }

                        System.IO.File.WriteAllText(saveFile.FileName, csvBuilder.ToString());
                    }
                }
            }
        }
    }
}
