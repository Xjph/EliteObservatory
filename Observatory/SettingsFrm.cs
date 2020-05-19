using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace Observatory
{
    public partial class SettingsFrm : Form
    {
        private Properties.Observatory settings;
        private ObservatoryFrm mainForm;
        private bool Loading;
        private bool BulkChangeInProgress;

        public SettingsFrm(ObservatoryFrm mainForm, bool monitoring)
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            settings = Properties.Observatory.Default;
            this.mainForm = mainForm;
            cbxCapi.Enabled = !monitoring;
        }

        private void SettingsFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.settingsOpen = false;
            
        }

        private void SettingsFrm_Load(object sender, EventArgs e)
        {
            Loading = true;
            BulkChangeInProgress = true;
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
            cbxRinghugger.Checked = settings.RingHugger;
            cbxLandRing.Checked = settings.RingLandable;
            cbxAutoMonitor.Checked = settings.AutoMonitor;
            cbxAutoRead.Checked = settings.AutoRead;
            cbxTelegram.Checked = settings.EnableTelegram;
            cbxEDOverlay.Checked = settings.UseEDOverlay;
            ShowHideTelegram();
            txtTelegramAPIKey.Text = settings.TelegramAPIKey;
            txtTelegramChatId.Text = settings.TelegramChatId;
            numEDONotificationX.Value = settings.EDONotificationX;
            numEDONotificationY.Value = settings.EDONotificationY;
            numEDOTimeout.Value = settings.EDONotificationTimeout;
            cbxCodex.Checked = settings.IncludeCodex;
            cbxSendToIGAU.Checked = settings.SendToIGAU;
            cbxGold.Checked = settings.AllMaterialSystem;
            cbxCapi.Checked = settings.UseCapi;
            cbxAutoClear.Checked = settings.AutoClearList;
            cbxSecondaryStar.Checked = settings.SecondaryStars;
            btnEDOHeaderColor.BackColor = System.Drawing.ColorTranslator.FromHtml(settings.EDOHeaderColor);
            cdlgEDOHeader.Color = btnEDOHeaderColor.BackColor;
            btnEDOBodyColor.BackColor = System.Drawing.ColorTranslator.FromHtml(settings.EDOBodyColor);
            cdlgEDOBody.Color = btnEDOBodyColor.BackColor;
            cbxEDODebug.Checked = settings.EDODebug;
            Loading = false;
            BulkChangeInProgress = false;
        }

        private void Cbx_LandWithTerra_CheckedChanged(object sender, EventArgs e)
        {
            settings.LandWithTerra = ((CheckBox)sender).Checked;
            Save();
        }

        private void Cbx_LandWithAtmo_CheckedChanged(object sender, EventArgs e)
        {
            settings.LandWithAtmo = ((CheckBox)sender).Checked;
            Save();
        }

        private void Cbx_LandHighG_CheckedChanged(object sender, EventArgs e)
        {
            settings.LandHighG = ((CheckBox)sender).Checked;
            Save();
        }

        private void Cbx_CloseOrbit_CheckedChanged(object sender, EventArgs e)
        {
            settings.CloseOrbit = ((CheckBox)sender).Checked;
            Save();
        }

        private void Cbx_ShepherdMoon_CheckedChanged(object sender, EventArgs e)
        {
            settings.ShepherdMoon = ((CheckBox)sender).Checked;
            Save();
        }

        private void Cbx_CloseBinary_CheckedChanged(object sender, EventArgs e)
        {
            settings.CloseBinary = ((CheckBox)sender).Checked;
            Save();
        }

        private void Cbx_CollidingBinary_CheckedChanged(object sender, EventArgs e)
        {
            settings.CollidingBinary = ((CheckBox)sender).Checked;
            Save();
        }

        private void Cbx_NestedMoon_CheckedChanged(object sender, EventArgs e)
        {
            settings.NestedMoon = ((CheckBox)sender).Checked;
            Save();
        }

        private void Cbx_TinyObject_CheckedChanged(object sender, EventArgs e)
        {
            settings.TinyObject = ((CheckBox)sender).Checked;
            Save();
        }

        private void Cbx_FastRotate_CheckedChanged(object sender, EventArgs e)
        {
            settings.FastRotate = ((CheckBox)sender).Checked;
            Save();
        }

        private void Cbx_FastOrbit_CheckedChanged(object sender, EventArgs e)
        {
            settings.FastOrbit = ((CheckBox)sender).Checked;
            Save();
        }

        private void Cbx_HighEccentric_CheckedChanged(object sender, EventArgs e)
        {
            settings.HighEccentric = ((CheckBox)sender).Checked;
            Save();
        }

        private void Cbx_GoodJump_CheckedChanged(object sender, EventArgs e)
        {
            settings.GoodJump = ((CheckBox)sender).Checked;
            Save();
        }

        private void Cbx_AllJumpBody_CheckedChanged(object sender, EventArgs e)
        {
            settings.AllJumpBody = ((CheckBox)sender).Checked;
            Save();
        }

        private void Cbx_AllJumpSystem_CheckedChanged(object sender, EventArgs e)
        {
            settings.AllJumpSystem = ((CheckBox)sender).Checked;
            Save();
        }

        private void Cbx_VeryInteresting_CheckedChanged(object sender, EventArgs e)
        {
            settings.VeryInteresting = ((CheckBox)sender).Checked;
            Save();
        }

        private void CbxToast_CheckedChanged(object sender, EventArgs e)
        {
            settings.Notify = ((CheckBox)sender).Checked;
            ShowHideTelegram();
            Save();
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
            Save();
        }

        private void BtnCopyReset_Click(object sender, EventArgs e)
        {
            txtCopy.Text = "%time% - %bodyL% - %info% - %detail%";
        }

        private void TxtCopy_TextChanged(object sender, EventArgs e)
        {
            settings.CopyTemplate = txtCopy.Text;
            Save();
        }

        private void Cbx_custom_CheckedChanged(object sender, EventArgs e)
        {
            settings.CustomRules = ((CheckBox)sender).Checked;
            Save();
        }

        private void Cbx_LandLarge_CheckedChanged(object sender, EventArgs e)
        {
            settings.LandLarge = ((CheckBox)sender).Checked;
            Save();
        }

        private void Cbx_WideRing_CheckedChanged(object sender, EventArgs e)
        {
            settings.WideRing = ((CheckBox)sender).Checked;
            Save();
        }

        private void TrackBar_Volume_Scroll(object sender, EventArgs e)
        {
            settings.TTSVolume = ((TrackBar)sender).Value;
            Save();
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
            Save();
        }

        private void CbxRinghugger_CheckedChanged(object sender, EventArgs e)
        {
            settings.RingHugger = ((CheckBox)sender).Checked;
            Save();
        }

        private void CbxGold_CheckedChanged(object sender, EventArgs e)
        {
            settings.AllMaterialSystem = ((CheckBox)sender).Checked;
            Save();
        }

        private void CbxAutoRead_CheckedChanged(object sender, EventArgs e)
        {
            settings.AutoRead = ((CheckBox)sender).Checked;
            Save();
        }

        private void CbxAutoMonitor_CheckedChanged(object sender, EventArgs e)
        {
            settings.AutoMonitor = ((CheckBox)sender).Checked;
            Save();
        }

        private void CbxTelegram_CheckedChanged(object sender, EventArgs e)
        {
            settings.EnableTelegram = ((CheckBox)sender).Checked;
            ShowHideTelegram();
            Save();
        }

        private void ShowHideTelegram()
        {
            bool show = cbxTelegram.Checked;
            bool toastEnabled = cbxToast.Checked;
            bool showEDO = cbxEDOverlay.Checked & toastEnabled;

            btn_TestTelegram.Enabled = show;
            txtTelegramAPIKey.Visible = show;
            txtTelegramChatId.Visible = show;
            lblTelegramBot.Visible = show;
            lblTelegramChat.Visible = show;
            groupBox_telegram.Height = 45 + (show ? 1 : 0) * 50;


            groupBox_edoverlay.Visible = toastEnabled;
            cbxEDOverlay.Visible = toastEnabled;
            numEDONotificationX.Visible = showEDO;
            numEDONotificationY.Visible = showEDO;
            numEDOTimeout.Visible = showEDO;
            lblEDONotificationPosX.Visible = showEDO;
            lblEDONotificationPosY.Visible = showEDO;
            lblEDOTimeout.Visible = showEDO;

            groupBox_edoverlay.Top = 504 + (show ? 1 : 0) * 50;
            groupBox_edoverlay.Height = 45 + (showEDO ? 1 : 0) * 55;
            Height = 545 + (show ? 1 : 0) * 50 + 50*(toastEnabled ? 1: 0) + (showEDO ? 1 : 0) * 55;
        }

        private void TxtTelegramAPIKey_TextChanged(object sender, EventArgs e)
        {
            settings.TelegramAPIKey = txtTelegramAPIKey.Text;
            Save();
        }

        private void TxtTelegramChatId_TextChanged(object sender, EventArgs e)
        {
            settings.TelegramChatId = txtTelegramChatId.Text;
            Save();
        }

        private void Btn_TestTelegram_Click(object sender, EventArgs e)
        {
            if (txtTelegramAPIKey.Text != string.Empty && txtTelegramChatId.Text != string.Empty)
            {
                try
                {
                    string message = "A test from Elite Observatory";
                    var request = new System.Net.Http.HttpRequestMessage
                    {
                        Method = System.Net.Http.HttpMethod.Get,
                        RequestUri = new Uri($"https://api.telegram.org/bot{settings.TelegramAPIKey}/sendMessage?chat_id={settings.TelegramChatId}&text={message}")
                    };

                    string response = HttpClient.SendRequest(request).Content.ReadAsStringAsync().Result;

                    MessageBox.Show(response, "Server Response");

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
            Save();
        }

        private void CbxSendToIGAU_CheckedChanged(object sender, EventArgs e)
        {          
            settings.SendToIGAU = ((CheckBox)sender).Checked;
            Save();
        }

        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            BulkChangeInProgress = true;
            foreach (var checkBox in groupBox_Interest.Controls.OfType<CheckBox>())
            {
                checkBox.Checked = true;
            }
            BulkChangeInProgress = false;
            Save();
        }

        private void BtnSelectNone_Click(object sender, EventArgs e)
        {
            BulkChangeInProgress = true;
            foreach (var checkBox in groupBox_Interest.Controls.OfType<CheckBox>())
            {
                checkBox.Checked = false;
            }
            BulkChangeInProgress = false;
            Save();
        }

        private void Save()
        {
            if (!BulkChangeInProgress)
                settings.Save();
        }

        private void CbxCapi_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox useCapi = ((CheckBox)sender);

            if (useCapi.Checked & !Loading)
            {
                var result = MessageBox.Show("This will enable retrieving journals from Frontier's API into your currently selected journal folder location. For more information please check the documentation on github. Continue?", "Enable Companion API", MessageBoxButtons.OKCancel);

                if (result == DialogResult.Cancel)
                {
                    useCapi.Checked = false;
                    return;
                }
            }

            settings.UseCapi = useCapi.Checked;
            Save();

            mainForm.CheckCapi(useCapi.Checked ? ObservatoryFrm.CapiState.Enabled : ObservatoryFrm.CapiState.Disabled);
            
        }

        private void LinkDonate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.me/eliteobservatory");
        }

        private void BtnEditCustom_Click(object sender, EventArgs e)
        {
            var editForm = new frmCriteriaEdit();
            editForm.Show();
        }

        private void CbxAutoClear_CheckedChanged(object sender, EventArgs e)
        {
            settings.AutoClearList = ((CheckBox)sender).Checked;
            Save();
        }

        private void CbxSecondaryStar_CheckedChanged(object sender, EventArgs e)
        {
            settings.SecondaryStars = ((CheckBox)sender).Checked;
            Save();
        }

        private void cbxEDOverlay_CheckedChanged(object sender, EventArgs e)
        {
            settings.UseEDOverlay = ((CheckBox)sender).Checked;
            ShowHideTelegram();
            Save();
            if (!Loading && settings.UseEDOverlay)
            {
                EDOverlay overlay = new EDOverlay();
                overlay.Send("Observatory notifications", "Notifications enabled!");
            }
        }

        private void numEDONotificationX_ValueChanged(object sender, EventArgs e)
        {
            settings.EDONotificationX = Decimal.ToInt32(((NumericUpDown)sender).Value);
            Save();
        }

        private void numEDONotificationY_ValueChanged(object sender, EventArgs e)
        {
            settings.EDONotificationY = Decimal.ToInt32(((NumericUpDown)sender).Value);
            Save();
        }

        private void numEDOTimeout_ValueChanged(object sender, EventArgs e)
        {
            settings.EDONotificationTimeout = Decimal.ToInt32(((NumericUpDown)sender).Value);
            Save();
        }


        private void btnEDOHeaderColor_Click(object sender, EventArgs e)
        {
            if (cdlgEDOHeader.ShowDialog() == DialogResult.Cancel)
                return;

            btnEDOHeaderColor.BackColor = cdlgEDOHeader.Color;
            settings.EDOHeaderColor = System.Drawing.ColorTranslator.ToHtml(cdlgEDOHeader.Color);
            Save();
        }

        private void btnEDOBodyColor_Click(object sender, EventArgs e)
        {
            if (cdlgEDOBody.ShowDialog() == DialogResult.Cancel)
                return;

            btnEDOBodyColor.BackColor = cdlgEDOBody.Color;
            settings.EDOBodyColor = System.Drawing.ColorTranslator.ToHtml(cdlgEDOBody.Color);
            Save();
        }

        private void cbxEDODebug_CheckedChanged(object sender, EventArgs e)
        {
            settings.EDODebug = ((CheckBox)sender).Checked;
            Save();
        }
    }
}
