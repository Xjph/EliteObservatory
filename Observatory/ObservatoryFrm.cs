using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Speech.Synthesis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Observatory
{
    public partial class ObservatoryFrm : Form
    {
        private LogMonitor logMonitor;
        public SpeechSynthesizer speech;
        private SettingsFrm settingsFrm;
        private ListViewColumnSorter columnSorter;
        public bool settingsOpen = false;
        
        public ObservatoryFrm()
        {
            InitializeComponent();
            logMonitor = new LogMonitor("");
            logMonitor.LogEntry += LogEvent;
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            columnSorter = new ListViewColumnSorter();
            listEvent.ListViewItemSorter = columnSorter;
            if (Properties.Observatory.Default.TTS)
            {
                speech = new SpeechSynthesizer();
                speech.SetOutputToDefaultAudioDevice();
            }

            try
            {
                string releasesResponse;
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    client.Headers["user-agent"] = "Xjph/EliteObservatory";
                    releasesResponse = client.DownloadString("https://api.github.com/repos/xjph/EliteObservatory/releases");
                }

                if (!string.IsNullOrEmpty(releasesResponse))
                {
                    JArray releases = (JArray)JsonConvert.DeserializeObject(releasesResponse, new JsonSerializerSettings() { DateParseHandling = DateParseHandling.None });
                    foreach (JObject release in releases)
                    {
                        if (release["tag_name"].ToString().CompareTo($"v{Application.ProductVersion}") > 0)
                        {
                            linkUpdate.Enabled = true;
                            linkUpdate.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking for update: {ex.Message}");
            }

        }

        private void BtnToggleMonitor_Click(object sender, EventArgs e) =>
            ToggleMonitor();

        private void LogEvent(object source, EventArgs e)
        {
            if (logMonitor.LastScanValid)
            {
                ScanReader scan = new ScanReader(logMonitor);

                if (scan.IsInteresting())
                {
                    Invoke((MethodInvoker)delegate ()
                    {
                        listEvent.BeginUpdate();
                        if (!logMonitor.ReadAllInProgress)
                        {
                            RemoveUninteresting();
                        }

                        foreach (var item in scan.Interest)
                        {
                            AddListItem(item);
                        }

                        if (!logMonitor.ReadAllInProgress && scan.Interest.Count > 0) { AnnounceItems(logMonitor.CurrentSystem, scan.Interest); }
                        listEvent.EndUpdate();
                    });
                }
                else if (!logMonitor.ReadAllInProgress)
                {

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
                    NotifyFrm notifyFrm = new NotifyFrm(fullBodyName + "\r\n" + announceText.ToString());
                    notifyFrm.Show(5000);
                }
                if (Properties.Observatory.Default.TTS)
                {
                    string spokenName;
                    spokenName = fullBodyName.Replace(currentSystem, string.Empty);
                    if (spokenName.Trim().Length > 0)
                    {
                        spokenName = "Body " + spokenName;
                    }
                    speech.Volume = Properties.Observatory.Default.TTSVolume;
                    speech.SpeakSsmlAsync($"<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"en-US\">{spokenName}:<break strength=\"weak\"/>{announceText}</speak>");
                }
                if(Properties.Observatory.Default.EnableTelegram)
                {
                    try
                    {
                        using (System.Net.WebClient client = new System.Net.WebClient())
                        {
                            string message = fullBodyName + "\r\n" + announceText.ToString();
                            string urlString = $"https://api.telegram.org/bot{Properties.Observatory.Default.TelegramAPIKey}/sendMessage?chat_id={Properties.Observatory.Default.TelegramChatId}&text={message}";
                            string result = client.DownloadString(urlString);
                        }

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
                                        (logMonitor.LastScan.Landable.GetValueOrDefault(false) && !(item.Description == $"All {Properties.Observatory.Default.FSDBoostSynthName} materials in system")) ? "🌐" : string.Empty
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
            readAllJournals();
        }

        void readAllJournals()
        {
            listEvent.BeginUpdate();
            listEvent.Items.Clear();
            logMonitor.SystemBody.Clear();
            listEvent.ListViewItemSorter = null;
            logMonitor.ReadAll(progressReadAll);
            listEvent.ListViewItemSorter = columnSorter;
            listEvent.Sort();
            listEvent.EndUpdate();
        }

        private void ListEvent_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listEvent.FocusedItem.Bounds.Contains(e.Location))
                {
                    contextCopy.Show(Cursor.Position);
                    contextCopy.Items[0].Enabled = listEvent.SelectedItems.Count == 1;
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
                settingsFrm = new SettingsFrm(this);
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
                Location = Properties.Observatory.Default.WindowLocation;
                Size = Properties.Observatory.Default.WindowSize;
            }
        }

        private void ObservatoryFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Observatory.Default.WindowSize = Size;
            Properties.Observatory.Default.WindowLocation = Location;
            Properties.Observatory.Default.Save();
            speech?.Dispose();
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
            logMonitor = new LogMonitor("");
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
                readAllJournals();

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
            Clipboard.SetText(copyText.ToString());
        }
    }
}
