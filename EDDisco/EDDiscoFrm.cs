using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;

namespace EDDisco
{
    public partial class EDDiscoFrm : Form
    {
        private LogMonitor logMonitor;
        private SpeechSynthesizer speech;

        public EDDiscoFrm()
        {
            InitializeComponent();
            logMonitor = new LogMonitor("");
            logMonitor.LogEntry += LogEvent;
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            notifyIcon.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            notifyIcon.Visible = false;
        }

        private void BtnToggleMonitor_Click(object sender, EventArgs e)
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

        private void LogEvent(object source, EventArgs e)
        {
            if (logMonitor.LastScanValid)
            {
                ScanReader scan = new ScanReader(logMonitor.LastScan, logMonitor.SystemBody, logMonitor.CurrentSystem);

                if (scan.IsInteresting())
                {
                    Invoke((MethodInvoker)delegate ()
                    {
                        if (!logMonitor.ReadAllInProgress && listEvent.Items[listEvent.Items.Count - 1].SubItems[1].Text == "Uninteresting")
                        {
                            listEvent.Items.RemoveAt(listEvent.Items.Count - 1);
                        }
                        bool addItem;
                        List<(string, string, string)> pendingRemoval = new List<(string, string, string)>();
                        foreach (var item in scan.Interest)
                        {
                            addItem = true;
                            if (item.Item2 == "All jumponium materials in system")
                            {
                                for (int i = Math.Max(0, listEvent.Items.Count - 10); i < listEvent.Items.Count; i++)
                                {
                                    if (listEvent.Items[i].SubItems[0].Text.Contains(logMonitor.CurrentSystem) && listEvent.Items[i].SubItems[1].Text == "All jumponium materials in system")
                                    {
                                        addItem = false;
                                    }
                                }
                            }

                            if (addItem)
                            {
                                AddListItem(item);
                            }
                            else
                            {
                                pendingRemoval.Add(item);
                            }
                        }
                        foreach (var item in pendingRemoval)
                        {
                            scan.Interest.Remove(item);
                        }
                        if (!logMonitor.ReadAllInProgress && scan.Interest.Count > 0) { AnnounceItems(scan.Interest); }
                    });
                }
                else if (!logMonitor.ReadAllInProgress)
                {

                    ListViewItem newItem = new ListViewItem(new string[] { scan.Interest[0].Item1, "Uninteresting" })
                    {
                        UseItemStyleForSubItems = false
                    };
                    Invoke((MethodInvoker)delegate () {
                    if (listEvent.Items.Count > 0 && listEvent.Items[listEvent.Items.Count - 1].SubItems[1].Text == "Uninteresting")
                    {
                        listEvent.Items.RemoveAt(listEvent.Items.Count - 1); 
                    }

                    newItem.SubItems[0].ForeColor = Color.DarkGray;
                    newItem.SubItems[1].ForeColor = Color.DarkGray;
                    listEvent.Items.Add(newItem).EnsureVisible(); });
                }
            }
        }

        private void AnnounceItems(List<(string,string,string)> items)
        {
            if (cbxToast.Checked || cbxTts.Checked)
            {
                notifyIcon.BalloonTipTitle = "Discovery:";
                string fullSystemName = items[0].Item1;
                StringBuilder announceText = new StringBuilder();
                
                foreach (var item in items)
                {
                    announceText.Append(item.Item2);
                    if (!item.Equals(items.Last()))
                    {
                        announceText.AppendLine(", ");
                    }
                }
                if (cbxToast.Checked)
                {
                    notifyIcon.BalloonTipText = fullSystemName + ": " + announceText.ToString();
                    notifyIcon.ShowBalloonTip(3000);
                }
                if (cbxTts.Checked)
                {
                    string sector = fullSystemName.Substring(0, fullSystemName.IndexOf('-') - 2);
                    string system = fullSystemName.Remove(0, sector.Length).Replace('-', '–'); //Want it to say "dash", not "hyphen".

                    speech.SpeakSsmlAsync($"<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xml:lang=\"en-US\">{sector}<say-as interpret-as=\"spell-out\">{system}</say-as><break strength=\"weak\"/>{announceText}</speak>");
                }
            }
        }

        private void AddListItem((string,string,string) item)
        {
            ListViewItem newItem = new ListViewItem(
                                        new string[] {
                                        item.Item1,
                                        item.Item2,
                                        logMonitor.LastScan.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                                        item.Item3,
                                        (logMonitor.LastScan.Landable.GetValueOrDefault(false) && !(item.Item2 == "All jumponium materials in system")) ? "🌐" : string.Empty
                                        });
            if (item.Item2.Contains("Interesting Object"))
            {
                newItem.UseItemStyleForSubItems = false;
                newItem.SubItems[1].Font = new Font(newItem.Font, FontStyle.Bold);
            }

            listEvent.Items.Add(newItem).EnsureVisible();
        }

        private void BtnReadAll_Click(object sender, EventArgs e)
        {
            if (logMonitor.ReadAllComplete)
            {
                DialogResult confirmResult;
                confirmResult = MessageBox.Show("This will clear the current list and re-read all journal logs. Contine?", "Confirm Refresh", MessageBoxButtons.OKCancel);
                if (confirmResult == DialogResult.OK)
                {
                    listEvent.Items.Clear();
                }
                else
                {
                    return;
                }
            }
            logMonitor.ReadAll(progressReadAll);
        }

        private void CbxToast_CheckedChanged(object sender, EventArgs e)
        {
            notifyIcon.Visible = cbxToast.Checked;
        }

        private void CbxTts_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxTts.Checked)
            {
                speech = new SpeechSynthesizer();
                speech.SetOutputToDefaultAudioDevice();
                //cbxTtsDetail.Visible = true;
            }
            else
            {
                speech.Dispose();
                //cbxTtsDetail.Visible = false;
            }
        }

        private void EDDiscoFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            notifyIcon.Icon = null;
            speech?.Dispose();
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
                        item.SubItems[2].Text + " - " +
                        item.SubItems[0].Text + " - " +
                        (item.SubItems[4].Text.Length > 0 ? "Landable - " : string.Empty) +
                        item.SubItems[1].Text +
                        (item.SubItems[3].Text.Length > 0 ? " - " + item.SubItems[3].Text : string.Empty)
                        );
                }
                else
                {
                    copyText.AppendLine(item.SubItems[0].Text + " - Uninteresting");
                }
                    

            }
            Clipboard.SetText(copyText.ToString());
        }
    }
}
