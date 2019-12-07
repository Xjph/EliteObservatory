using System;
using System.Drawing;
using System.Windows.Forms;

namespace Observatory
{
    public partial class SettingsFrm : Form
    {
        private Properties.Observatory settings;
        private ObservatoryFrm mainForm;
        private bool Loading;

        public SettingsFrm(ObservatoryFrm mainForm)
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            settings = Properties.Observatory.Default;
            this.mainForm = mainForm;
        }

        private void SettingsFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.settingsOpen = false;
        }

        private void SettingsFrm_Load(object sender, EventArgs e)
        {
            Loading = true;

            cbx_LandWithTerra.Checked = settings.LandWithTerra;
            cbx_LandWithAtmo.Checked = settings.LandWithAtmo;
            cbx_LandHighG.Checked = settings.LandHighG;
            cbx_AllJumpBody.Checked = settings.AllJumpBody;
            cbx_AllJumpSystem.Checked = settings.AllJumpSystem;
            cbx_CloseBinary.Checked = settings.CloseBinary;
            cbx_CloseOrbit.Checked = settings.CloseOrbit;
            cbx_CollidingBinary.Checked = settings.CollidingBinary;
            cbx_FastOrbit.Checked = settings.FastOrbit;
            cbx_FastRotate.Checked = settings.FastRotate;
            cbx_GoodJump.Checked = settings.GoodJump;
            cbx_HighEccentric.Checked = settings.HighEccentric;
            cbx_NestedMoon.Checked = settings.NestedMoon;
            cbx_ShepherdMoon.Checked = settings.ShepherdMoon;
            cbx_TinyObject.Checked = settings.TinyObject;
            cbx_VeryInteresting.Checked = settings.VeryInteresting;
            cbxToast.Checked = settings.Notify;
            cbxTts.Checked = settings.TTS;
            txtCopy.Text = settings.CopyTemplate;
            cbx_custom.Checked = settings.CustomRules;
            cbx_LandLarge.Checked = settings.LandLarge;
            cbx_WideRing.Checked = settings.WideRing;
            trackBar_Volume.Value = settings.TTSVolume;
            btn_TestVol.Enabled = settings.TTS;
            Loading = false;
            cbxRinghugger.Checked = settings.RingHugger;
            cbxLandRing.Checked = settings.RingLandable;
            cbxAutoMonitor.Checked = settings.AutoMonitor;
            cbxAutoRead.Checked = settings.AutoRead;
            cbxTelegram.Checked = settings.EnableTelegram;
            btn_TestTelegram.Enabled = settings.EnableTelegram;
            txtTelegramAPIKey.Text = settings.TelegramAPIKey;
            txtTelegramChatId.Text = settings.TelegramChatId;
            cbxCodex.Checked = settings.IncludeCodex;
        }

        private void Cbx_LandWithTerra_CheckedChanged(object sender, EventArgs e)
        {
            settings.LandWithTerra = ((CheckBox)sender).Checked;
            settings.Save();
        }

        private void Cbx_LandWithAtmo_CheckedChanged(object sender, EventArgs e)
        {
            settings.LandWithAtmo = ((CheckBox)sender).Checked;
            settings.Save();
        }

        private void Cbx_LandHighG_CheckedChanged(object sender, EventArgs e)
        {
            settings.LandHighG = ((CheckBox)sender).Checked;
            settings.Save();
        }

        private void Cbx_CloseOrbit_CheckedChanged(object sender, EventArgs e)
        {
            settings.CloseOrbit = ((CheckBox)sender).Checked;
            settings.Save();
        }

        private void Cbx_ShepherdMoon_CheckedChanged(object sender, EventArgs e)
        {
            settings.ShepherdMoon = ((CheckBox)sender).Checked;
            settings.Save();
        }

        private void Cbx_CloseBinary_CheckedChanged(object sender, EventArgs e)
        {
            settings.CloseBinary = ((CheckBox)sender).Checked;
            settings.Save();
        }

        private void Cbx_CollidingBinary_CheckedChanged(object sender, EventArgs e)
        {
            settings.CollidingBinary = ((CheckBox)sender).Checked;
            settings.Save();
        }

        private void Cbx_NestedMoon_CheckedChanged(object sender, EventArgs e)
        {
            settings.NestedMoon = ((CheckBox)sender).Checked;
            settings.Save();
        }

        private void Cbx_TinyObject_CheckedChanged(object sender, EventArgs e)
        {
            settings.TinyObject = ((CheckBox)sender).Checked;
            settings.Save();
        }

        private void Cbx_FastRotate_CheckedChanged(object sender, EventArgs e)
        {
            settings.FastRotate = ((CheckBox)sender).Checked;
            settings.Save();
        }

        private void Cbx_FastOrbit_CheckedChanged(object sender, EventArgs e)
        {
            settings.FastOrbit = ((CheckBox)sender).Checked;
            settings.Save();
        }

        private void Cbx_HighEccentric_CheckedChanged(object sender, EventArgs e)
        {
            settings.HighEccentric = ((CheckBox)sender).Checked;
            settings.Save();
        }

        private void Cbx_GoodJump_CheckedChanged(object sender, EventArgs e)
        {
            settings.GoodJump = ((CheckBox)sender).Checked;
            settings.Save();
        }

        private void Cbx_AllJumpBody_CheckedChanged(object sender, EventArgs e)
        {
            settings.AllJumpBody = ((CheckBox)sender).Checked;
            settings.Save();
        }

        private void Cbx_AllJumpSystem_CheckedChanged(object sender, EventArgs e)
        {
            settings.AllJumpSystem = ((CheckBox)sender).Checked;
            settings.Save();
        }

        private void Cbx_VeryInteresting_CheckedChanged(object sender, EventArgs e)
        {
            settings.VeryInteresting = ((CheckBox)sender).Checked;
            settings.Save();
        }

        private void CbxToast_CheckedChanged(object sender, EventArgs e)
        {
            settings.Notify = ((CheckBox)sender).Checked;
            settings.Save();
            if (!Loading && settings.Notify)
            {
                NotifyFrm notifyFrm = new NotifyFrm("Notification Popups Enabled");
                notifyFrm.Show(5000);
            }
        }

        private void CbxTts_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxTts.Checked)
            {
                try
                {
                    mainForm.speech = new System.Speech.Synthesis.SpeechSynthesizer();
                    mainForm.speech.SetOutputToDefaultAudioDevice();
                    //cbxTtsDetail.Visible = true;
                }
                catch
                {
                    MessageBox.Show("There was an error while initializing Text-To-Speech. Windows Text-To-Speech services may not be available on this system.");
                    cbxTts.Checked = false;
                }
            }
            else
            {
                mainForm.speech.Dispose();
                //cbxTtsDetail.Visible = false;
            }
            btn_TestVol.Enabled = cbxTts.Checked;
            settings.TTS = cbxTts.Checked;
            settings.Save();
        }

        private void BtnCopyReset_Click(object sender, EventArgs e)
        {
            txtCopy.Text = "%time% - %bodyL% - %info% - %detail%";
        }

        private void TxtCopy_TextChanged(object sender, EventArgs e)
        {
            settings.CopyTemplate = txtCopy.Text;
            settings.Save();
        }

        private void Cbx_custom_CheckedChanged(object sender, EventArgs e)
        {
            settings.CustomRules = ((CheckBox)sender).Checked;
            settings.Save();
        }

        private void Cbx_LandLarge_CheckedChanged(object sender, EventArgs e)
        {
            settings.LandLarge = ((CheckBox)sender).Checked;
            settings.Save();
        }

        private void Cbx_WideRing_CheckedChanged(object sender, EventArgs e)
        {
            settings.WideRing = ((CheckBox)sender).Checked;
            settings.Save();
        }

        private void TrackBar_Volume_Scroll(object sender, EventArgs e)
        {
            settings.TTSVolume = ((TrackBar)sender).Value;
            settings.Save();
        }

        private void Btn_TestVol_Click(object sender, EventArgs e)
        {
            mainForm.speech.Volume = settings.TTSVolume;
            mainForm.speech.SpeakAsync("This is your current text-to-speech volume.");
        }

        private void LinkGit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Xjph/EliteObservatory");
        }

        private void CbxLandRing_CheckedChanged(object sender, EventArgs e)
        {
            settings.RingLandable = ((CheckBox)sender).Checked;
            settings.Save();
        }

        private void CbxRinghugger_CheckedChanged(object sender, EventArgs e)
        {
            settings.RingHugger = ((CheckBox)sender).Checked;
            settings.Save();
        }

        private void CbxAutoRead_CheckedChanged(object sender, EventArgs e)
        {
            settings.AutoRead = ((CheckBox)sender).Checked;
            settings.Save();
        }

        private void CbxAutoMonitor_CheckedChanged(object sender, EventArgs e)
        {
            settings.AutoMonitor = ((CheckBox)sender).Checked;
            settings.Save();
        }

        private void cbxTelegram_CheckedChanged(object sender, EventArgs e)
        {
            settings.EnableTelegram = ((CheckBox)sender).Checked;
            btn_TestTelegram.Enabled = ((CheckBox)sender).Checked;
            settings.Save();
        }

        private void txtTelegramAPIKey_TextChanged(object sender, EventArgs e)
        {
            settings.TelegramAPIKey = txtTelegramAPIKey.Text;
            settings.Save();
        }

        private void txtTelegramChatId_TextChanged(object sender, EventArgs e)
        {
            settings.TelegramChatId = txtTelegramChatId.Text;
            settings.Save();
        }

        private void btn_TestTelegram_Click(object sender, EventArgs e)
        {
            if (txtTelegramAPIKey.Text != string.Empty && txtTelegramChatId.Text != string.Empty)
            {
                try
                {
                    using (System.Net.WebClient client = new System.Net.WebClient())
                    {
                        string message = "A test from Elite Observatory";
                        string urlString = $"https://api.telegram.org/bot{settings.TelegramAPIKey}/sendMessage?chat_id={settings.TelegramChatId}&text={message}";
                        MessageBox.Show(client.DownloadString(urlString), "Server Response");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error trying to send notification: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please provide an API Key and Chat ID");
            }
        }

        private void CbxCodex_CheckedChanged(object sender, EventArgs e)
        {
            settings.IncludeCodex = ((CheckBox)sender).Checked;
            settings.Save();
        }
    }
}
