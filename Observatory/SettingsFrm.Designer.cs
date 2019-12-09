namespace Observatory
{
    partial class SettingsFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox_Interest = new System.Windows.Forms.GroupBox();
            this.cbxCodex = new System.Windows.Forms.CheckBox();
            this.cbxLandRing = new System.Windows.Forms.CheckBox();
            this.cbxRinghugger = new System.Windows.Forms.CheckBox();
            this.cbx_WideRing = new System.Windows.Forms.CheckBox();
            this.cbx_LandLarge = new System.Windows.Forms.CheckBox();
            this.cbx_VeryInteresting = new System.Windows.Forms.CheckBox();
            this.cbx_AllJumpSystem = new System.Windows.Forms.CheckBox();
            this.cbx_AllJumpBody = new System.Windows.Forms.CheckBox();
            this.cbx_GoodJump = new System.Windows.Forms.CheckBox();
            this.cbx_HighEccentric = new System.Windows.Forms.CheckBox();
            this.cbx_FastOrbit = new System.Windows.Forms.CheckBox();
            this.cbx_FastRotate = new System.Windows.Forms.CheckBox();
            this.cbx_TinyObject = new System.Windows.Forms.CheckBox();
            this.cbx_NestedMoon = new System.Windows.Forms.CheckBox();
            this.cbx_CollidingBinary = new System.Windows.Forms.CheckBox();
            this.cbx_CloseBinary = new System.Windows.Forms.CheckBox();
            this.cbx_ShepherdMoon = new System.Windows.Forms.CheckBox();
            this.cbx_CloseOrbit = new System.Windows.Forms.CheckBox();
            this.cbx_LandHighG = new System.Windows.Forms.CheckBox();
            this.cbx_LandWithAtmo = new System.Windows.Forms.CheckBox();
            this.cbx_LandWithTerra = new System.Windows.Forms.CheckBox();
            this.groupBox_misc = new System.Windows.Forms.GroupBox();
            this.cbxSendToIGAU = new System.Windows.Forms.CheckBox();
            this.cbxAutoMonitor = new System.Windows.Forms.CheckBox();
            this.cbxAutoRead = new System.Windows.Forms.CheckBox();
            this.cbx_custom = new System.Windows.Forms.CheckBox();
            this.btnCopyReset = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCopy = new System.Windows.Forms.TextBox();
            this.cbxTts = new System.Windows.Forms.CheckBox();
            this.cbxToast = new System.Windows.Forms.CheckBox();
            this.tipCopy = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox_TTS = new System.Windows.Forms.GroupBox();
            this.btn_TestVol = new System.Windows.Forms.Button();
            this.trackBar_Volume = new System.Windows.Forms.TrackBar();
            this.linkGit = new System.Windows.Forms.LinkLabel();
            this.groupBox_telegram = new System.Windows.Forms.GroupBox();
            this.btn_TestTelegram = new System.Windows.Forms.Button();
            this.txtTelegramChatId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTelegramAPIKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxTelegram = new System.Windows.Forms.CheckBox();
            this.groupBox_Interest.SuspendLayout();
            this.groupBox_misc.SuspendLayout();
            this.groupBox_TTS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Volume)).BeginInit();
            this.groupBox_telegram.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_Interest
            // 
            this.groupBox_Interest.Controls.Add(this.cbxCodex);
            this.groupBox_Interest.Controls.Add(this.cbxLandRing);
            this.groupBox_Interest.Controls.Add(this.cbxRinghugger);
            this.groupBox_Interest.Controls.Add(this.cbx_WideRing);
            this.groupBox_Interest.Controls.Add(this.cbx_LandLarge);
            this.groupBox_Interest.Controls.Add(this.cbx_VeryInteresting);
            this.groupBox_Interest.Controls.Add(this.cbx_AllJumpSystem);
            this.groupBox_Interest.Controls.Add(this.cbx_AllJumpBody);
            this.groupBox_Interest.Controls.Add(this.cbx_GoodJump);
            this.groupBox_Interest.Controls.Add(this.cbx_HighEccentric);
            this.groupBox_Interest.Controls.Add(this.cbx_FastOrbit);
            this.groupBox_Interest.Controls.Add(this.cbx_FastRotate);
            this.groupBox_Interest.Controls.Add(this.cbx_TinyObject);
            this.groupBox_Interest.Controls.Add(this.cbx_NestedMoon);
            this.groupBox_Interest.Controls.Add(this.cbx_CollidingBinary);
            this.groupBox_Interest.Controls.Add(this.cbx_CloseBinary);
            this.groupBox_Interest.Controls.Add(this.cbx_ShepherdMoon);
            this.groupBox_Interest.Controls.Add(this.cbx_CloseOrbit);
            this.groupBox_Interest.Controls.Add(this.cbx_LandHighG);
            this.groupBox_Interest.Controls.Add(this.cbx_LandWithAtmo);
            this.groupBox_Interest.Controls.Add(this.cbx_LandWithTerra);
            this.groupBox_Interest.Location = new System.Drawing.Point(13, 13);
            this.groupBox_Interest.Name = "groupBox_Interest";
            this.groupBox_Interest.Size = new System.Drawing.Size(340, 273);
            this.groupBox_Interest.TabIndex = 0;
            this.groupBox_Interest.TabStop = false;
            this.groupBox_Interest.Text = "Interest Criteria";
            // 
            // cbxCodex
            // 
            this.cbxCodex.AutoSize = true;
            this.cbxCodex.Location = new System.Drawing.Point(6, 249);
            this.cbxCodex.Name = "cbxCodex";
            this.cbxCodex.Size = new System.Drawing.Size(114, 17);
            this.cbxCodex.TabIndex = 20;
            this.cbxCodex.Text = "Codex Discoveries";
            this.cbxCodex.UseVisualStyleBackColor = true;
            this.cbxCodex.CheckedChanged += new System.EventHandler(this.CbxCodex_CheckedChanged);
            // 
            // cbxLandRing
            // 
            this.cbxLandRing.AutoSize = true;
            this.cbxLandRing.Location = new System.Drawing.Point(165, 20);
            this.cbxLandRing.Name = "cbxLandRing";
            this.cbxLandRing.Size = new System.Drawing.Size(111, 17);
            this.cbxLandRing.TabIndex = 19;
            this.cbxLandRing.Text = "Landable w/ Ring";
            this.cbxLandRing.UseVisualStyleBackColor = true;
            this.cbxLandRing.CheckedChanged += new System.EventHandler(this.CbxLandRing_CheckedChanged);
            // 
            // cbxRinghugger
            // 
            this.cbxRinghugger.AutoSize = true;
            this.cbxRinghugger.Location = new System.Drawing.Point(6, 226);
            this.cbxRinghugger.Name = "cbxRinghugger";
            this.cbxRinghugger.Size = new System.Drawing.Size(121, 17);
            this.cbxRinghugger.TabIndex = 18;
            this.cbxRinghugger.Text = "Close Ring Proximity";
            this.cbxRinghugger.UseVisualStyleBackColor = true;
            this.cbxRinghugger.CheckedChanged += new System.EventHandler(this.CbxRinghugger_CheckedChanged);
            // 
            // cbx_WideRing
            // 
            this.cbx_WideRing.AutoSize = true;
            this.cbx_WideRing.Location = new System.Drawing.Point(6, 157);
            this.cbx_WideRing.Name = "cbx_WideRing";
            this.cbx_WideRing.Size = new System.Drawing.Size(76, 17);
            this.cbx_WideRing.TabIndex = 17;
            this.cbx_WideRing.Text = "Wide Ring";
            this.cbx_WideRing.UseVisualStyleBackColor = true;
            this.cbx_WideRing.CheckedChanged += new System.EventHandler(this.Cbx_WideRing_CheckedChanged);
            // 
            // cbx_LandLarge
            // 
            this.cbx_LandLarge.AutoSize = true;
            this.cbx_LandLarge.Location = new System.Drawing.Point(6, 88);
            this.cbx_LandLarge.Name = "cbx_LandLarge";
            this.cbx_LandLarge.Size = new System.Drawing.Size(100, 17);
            this.cbx_LandLarge.TabIndex = 16;
            this.cbx_LandLarge.Text = "Landable Large";
            this.cbx_LandLarge.UseVisualStyleBackColor = true;
            this.cbx_LandLarge.CheckedChanged += new System.EventHandler(this.Cbx_LandLarge_CheckedChanged);
            // 
            // cbx_VeryInteresting
            // 
            this.cbx_VeryInteresting.AutoSize = true;
            this.cbx_VeryInteresting.Location = new System.Drawing.Point(165, 226);
            this.cbx_VeryInteresting.Name = "cbx_VeryInteresting";
            this.cbx_VeryInteresting.Size = new System.Drawing.Size(153, 17);
            this.cbx_VeryInteresting.TabIndex = 15;
            this.cbx_VeryInteresting.Text = "Multiple Criteria Notification";
            this.cbx_VeryInteresting.UseVisualStyleBackColor = true;
            this.cbx_VeryInteresting.CheckedChanged += new System.EventHandler(this.Cbx_VeryInteresting_CheckedChanged);
            // 
            // cbx_AllJumpSystem
            // 
            this.cbx_AllJumpSystem.AutoSize = true;
            this.cbx_AllJumpSystem.Location = new System.Drawing.Point(165, 203);
            this.cbx_AllJumpSystem.Name = "cbx_AllJumpSystem";
            this.cbx_AllJumpSystem.Size = new System.Drawing.Size(165, 17);
            this.cbx_AllJumpSystem.TabIndex = 14;
            this.cbx_AllJumpSystem.Text = "All Jumponium Avail. (System)";
            this.cbx_AllJumpSystem.UseVisualStyleBackColor = true;
            this.cbx_AllJumpSystem.CheckedChanged += new System.EventHandler(this.Cbx_AllJumpSystem_CheckedChanged);
            // 
            // cbx_AllJumpBody
            // 
            this.cbx_AllJumpBody.AutoSize = true;
            this.cbx_AllJumpBody.Location = new System.Drawing.Point(165, 180);
            this.cbx_AllJumpBody.Name = "cbx_AllJumpBody";
            this.cbx_AllJumpBody.Size = new System.Drawing.Size(155, 17);
            this.cbx_AllJumpBody.TabIndex = 13;
            this.cbx_AllJumpBody.Text = "All Jumponium Avail. (Body)";
            this.cbx_AllJumpBody.UseVisualStyleBackColor = true;
            this.cbx_AllJumpBody.CheckedChanged += new System.EventHandler(this.Cbx_AllJumpBody_CheckedChanged);
            // 
            // cbx_GoodJump
            // 
            this.cbx_GoodJump.AutoSize = true;
            this.cbx_GoodJump.Location = new System.Drawing.Point(165, 157);
            this.cbx_GoodJump.Name = "cbx_GoodJump";
            this.cbx_GoodJump.Size = new System.Drawing.Size(137, 17);
            this.cbx_GoodJump.TabIndex = 12;
            this.cbx_GoodJump.Text = "Good Jumponium Avail.";
            this.cbx_GoodJump.UseVisualStyleBackColor = true;
            this.cbx_GoodJump.CheckedChanged += new System.EventHandler(this.Cbx_GoodJump_CheckedChanged);
            // 
            // cbx_HighEccentric
            // 
            this.cbx_HighEccentric.AutoSize = true;
            this.cbx_HighEccentric.Location = new System.Drawing.Point(165, 134);
            this.cbx_HighEccentric.Name = "cbx_HighEccentric";
            this.cbx_HighEccentric.Size = new System.Drawing.Size(106, 17);
            this.cbx_HighEccentric.TabIndex = 11;
            this.cbx_HighEccentric.Text = "High Eccentricity";
            this.cbx_HighEccentric.UseVisualStyleBackColor = true;
            this.cbx_HighEccentric.CheckedChanged += new System.EventHandler(this.Cbx_HighEccentric_CheckedChanged);
            // 
            // cbx_FastOrbit
            // 
            this.cbx_FastOrbit.AutoSize = true;
            this.cbx_FastOrbit.Location = new System.Drawing.Point(165, 111);
            this.cbx_FastOrbit.Name = "cbx_FastOrbit";
            this.cbx_FastOrbit.Size = new System.Drawing.Size(71, 17);
            this.cbx_FastOrbit.TabIndex = 10;
            this.cbx_FastOrbit.Text = "Fast Orbit";
            this.cbx_FastOrbit.UseVisualStyleBackColor = true;
            this.cbx_FastOrbit.CheckedChanged += new System.EventHandler(this.Cbx_FastOrbit_CheckedChanged);
            // 
            // cbx_FastRotate
            // 
            this.cbx_FastRotate.AutoSize = true;
            this.cbx_FastRotate.Location = new System.Drawing.Point(165, 88);
            this.cbx_FastRotate.Name = "cbx_FastRotate";
            this.cbx_FastRotate.Size = new System.Drawing.Size(89, 17);
            this.cbx_FastRotate.TabIndex = 9;
            this.cbx_FastRotate.Text = "Fast Rotation";
            this.cbx_FastRotate.UseVisualStyleBackColor = true;
            this.cbx_FastRotate.CheckedChanged += new System.EventHandler(this.Cbx_FastRotate_CheckedChanged);
            // 
            // cbx_TinyObject
            // 
            this.cbx_TinyObject.AutoSize = true;
            this.cbx_TinyObject.Location = new System.Drawing.Point(165, 66);
            this.cbx_TinyObject.Name = "cbx_TinyObject";
            this.cbx_TinyObject.Size = new System.Drawing.Size(80, 17);
            this.cbx_TinyObject.TabIndex = 8;
            this.cbx_TinyObject.Text = "Tiny Object";
            this.cbx_TinyObject.UseVisualStyleBackColor = true;
            this.cbx_TinyObject.CheckedChanged += new System.EventHandler(this.Cbx_TinyObject_CheckedChanged);
            // 
            // cbx_NestedMoon
            // 
            this.cbx_NestedMoon.AutoSize = true;
            this.cbx_NestedMoon.Location = new System.Drawing.Point(165, 43);
            this.cbx_NestedMoon.Name = "cbx_NestedMoon";
            this.cbx_NestedMoon.Size = new System.Drawing.Size(90, 17);
            this.cbx_NestedMoon.TabIndex = 7;
            this.cbx_NestedMoon.Text = "Nested Moon";
            this.cbx_NestedMoon.UseVisualStyleBackColor = true;
            this.cbx_NestedMoon.CheckedChanged += new System.EventHandler(this.Cbx_NestedMoon_CheckedChanged);
            // 
            // cbx_CollidingBinary
            // 
            this.cbx_CollidingBinary.AutoSize = true;
            this.cbx_CollidingBinary.Location = new System.Drawing.Point(6, 203);
            this.cbx_CollidingBinary.Name = "cbx_CollidingBinary";
            this.cbx_CollidingBinary.Size = new System.Drawing.Size(97, 17);
            this.cbx_CollidingBinary.TabIndex = 6;
            this.cbx_CollidingBinary.Text = "Colliding Binary";
            this.cbx_CollidingBinary.UseVisualStyleBackColor = true;
            this.cbx_CollidingBinary.CheckedChanged += new System.EventHandler(this.Cbx_CollidingBinary_CheckedChanged);
            // 
            // cbx_CloseBinary
            // 
            this.cbx_CloseBinary.AutoSize = true;
            this.cbx_CloseBinary.Location = new System.Drawing.Point(6, 180);
            this.cbx_CloseBinary.Name = "cbx_CloseBinary";
            this.cbx_CloseBinary.Size = new System.Drawing.Size(84, 17);
            this.cbx_CloseBinary.TabIndex = 5;
            this.cbx_CloseBinary.Text = "Close Binary";
            this.cbx_CloseBinary.UseVisualStyleBackColor = true;
            this.cbx_CloseBinary.CheckedChanged += new System.EventHandler(this.Cbx_CloseBinary_CheckedChanged);
            // 
            // cbx_ShepherdMoon
            // 
            this.cbx_ShepherdMoon.AutoSize = true;
            this.cbx_ShepherdMoon.Location = new System.Drawing.Point(6, 134);
            this.cbx_ShepherdMoon.Name = "cbx_ShepherdMoon";
            this.cbx_ShepherdMoon.Size = new System.Drawing.Size(102, 17);
            this.cbx_ShepherdMoon.TabIndex = 4;
            this.cbx_ShepherdMoon.Text = "Shepherd Moon";
            this.cbx_ShepherdMoon.UseVisualStyleBackColor = true;
            this.cbx_ShepherdMoon.CheckedChanged += new System.EventHandler(this.Cbx_ShepherdMoon_CheckedChanged);
            // 
            // cbx_CloseOrbit
            // 
            this.cbx_CloseOrbit.AutoSize = true;
            this.cbx_CloseOrbit.Location = new System.Drawing.Point(6, 111);
            this.cbx_CloseOrbit.Name = "cbx_CloseOrbit";
            this.cbx_CloseOrbit.Size = new System.Drawing.Size(77, 17);
            this.cbx_CloseOrbit.TabIndex = 3;
            this.cbx_CloseOrbit.Text = "Close Orbit";
            this.cbx_CloseOrbit.UseVisualStyleBackColor = true;
            this.cbx_CloseOrbit.CheckedChanged += new System.EventHandler(this.Cbx_CloseOrbit_CheckedChanged);
            // 
            // cbx_LandHighG
            // 
            this.cbx_LandHighG.AutoSize = true;
            this.cbx_LandHighG.Location = new System.Drawing.Point(6, 65);
            this.cbx_LandHighG.Name = "cbx_LandHighG";
            this.cbx_LandHighG.Size = new System.Drawing.Size(106, 17);
            this.cbx_LandHighG.TabIndex = 2;
            this.cbx_LandHighG.Text = "Landable High-G";
            this.cbx_LandHighG.UseVisualStyleBackColor = true;
            this.cbx_LandHighG.CheckedChanged += new System.EventHandler(this.Cbx_LandHighG_CheckedChanged);
            // 
            // cbx_LandWithAtmo
            // 
            this.cbx_LandWithAtmo.AutoSize = true;
            this.cbx_LandWithAtmo.Location = new System.Drawing.Point(6, 43);
            this.cbx_LandWithAtmo.Name = "cbx_LandWithAtmo";
            this.cbx_LandWithAtmo.Size = new System.Drawing.Size(145, 17);
            this.cbx_LandWithAtmo.TabIndex = 1;
            this.cbx_LandWithAtmo.Text = "Landable w/ Atmosphere";
            this.cbx_LandWithAtmo.UseVisualStyleBackColor = true;
            this.cbx_LandWithAtmo.CheckedChanged += new System.EventHandler(this.Cbx_LandWithAtmo_CheckedChanged);
            // 
            // cbx_LandWithTerra
            // 
            this.cbx_LandWithTerra.AutoSize = true;
            this.cbx_LandWithTerra.Location = new System.Drawing.Point(6, 20);
            this.cbx_LandWithTerra.Name = "cbx_LandWithTerra";
            this.cbx_LandWithTerra.Size = new System.Drawing.Size(147, 17);
            this.cbx_LandWithTerra.TabIndex = 0;
            this.cbx_LandWithTerra.Text = "Landable && Terraformable";
            this.cbx_LandWithTerra.UseVisualStyleBackColor = true;
            this.cbx_LandWithTerra.CheckedChanged += new System.EventHandler(this.Cbx_LandWithTerra_CheckedChanged);
            // 
            // groupBox_misc
            // 
            this.groupBox_misc.Controls.Add(this.cbxSendToIGAU);
            this.groupBox_misc.Controls.Add(this.cbxAutoMonitor);
            this.groupBox_misc.Controls.Add(this.cbxAutoRead);
            this.groupBox_misc.Controls.Add(this.cbx_custom);
            this.groupBox_misc.Controls.Add(this.btnCopyReset);
            this.groupBox_misc.Controls.Add(this.label1);
            this.groupBox_misc.Controls.Add(this.txtCopy);
            this.groupBox_misc.Controls.Add(this.cbxTts);
            this.groupBox_misc.Controls.Add(this.cbxToast);
            this.groupBox_misc.Location = new System.Drawing.Point(13, 292);
            this.groupBox_misc.Name = "groupBox_misc";
            this.groupBox_misc.Size = new System.Drawing.Size(340, 123);
            this.groupBox_misc.TabIndex = 1;
            this.groupBox_misc.TabStop = false;
            this.groupBox_misc.Text = "Misc.";
            // 
            // cbxSendToIGAU
            // 
            this.cbxSendToIGAU.AutoSize = true;
            this.cbxSendToIGAU.Location = new System.Drawing.Point(6, 92);
            this.cbxSendToIGAU.Name = "cbxSendToIGAU";
            this.cbxSendToIGAU.Size = new System.Drawing.Size(284, 17);
            this.cbxSendToIGAU.TabIndex = 21;
            this.cbxSendToIGAU.Text = "Transmit Codex Discoveries to IGAU discovery catalog";
            this.tipCopy.SetToolTip(this.cbxSendToIGAU, "https://raw.githubusercontent.com/Elite-IGAU/publications/master/IGAU_Codex.csv");
            this.cbxSendToIGAU.UseVisualStyleBackColor = true;
            this.cbxSendToIGAU.CheckedChanged += new System.EventHandler(this.CbxSendToIGAU_CheckedChanged);
            // 
            // cbxAutoMonitor
            // 
            this.cbxAutoMonitor.AutoSize = true;
            this.cbxAutoMonitor.Location = new System.Drawing.Point(121, 69);
            this.cbxAutoMonitor.Name = "cbxAutoMonitor";
            this.cbxAutoMonitor.Size = new System.Drawing.Size(111, 17);
            this.cbxAutoMonitor.TabIndex = 16;
            this.cbxAutoMonitor.Text = "Monitor on launch";
            this.tipCopy.SetToolTip(this.cbxAutoMonitor, "Automatically start monitoring logs at start up");
            this.cbxAutoMonitor.UseVisualStyleBackColor = true;
            this.cbxAutoMonitor.CheckedChanged += new System.EventHandler(this.CbxAutoMonitor_CheckedChanged);
            // 
            // cbxAutoRead
            // 
            this.cbxAutoRead.AutoSize = true;
            this.cbxAutoRead.Location = new System.Drawing.Point(6, 69);
            this.cbxAutoRead.Name = "cbxAutoRead";
            this.cbxAutoRead.Size = new System.Drawing.Size(102, 17);
            this.cbxAutoRead.TabIndex = 15;
            this.cbxAutoRead.Text = "Read on launch";
            this.tipCopy.SetToolTip(this.cbxAutoRead, "Automatically read all logs at start up");
            this.cbxAutoRead.UseVisualStyleBackColor = true;
            this.cbxAutoRead.CheckedChanged += new System.EventHandler(this.CbxAutoRead_CheckedChanged);
            // 
            // cbx_custom
            // 
            this.cbx_custom.AutoSize = true;
            this.cbx_custom.Location = new System.Drawing.Point(226, 46);
            this.cbx_custom.Name = "cbx_custom";
            this.cbx_custom.Size = new System.Drawing.Size(96, 17);
            this.cbx_custom.TabIndex = 14;
            this.cbx_custom.Text = "Custom Criteria";
            this.cbx_custom.UseVisualStyleBackColor = true;
            this.cbx_custom.CheckedChanged += new System.EventHandler(this.Cbx_custom_CheckedChanged);
            // 
            // btnCopyReset
            // 
            this.btnCopyReset.Location = new System.Drawing.Point(289, 19);
            this.btnCopyReset.Name = "btnCopyReset";
            this.btnCopyReset.Size = new System.Drawing.Size(45, 23);
            this.btnCopyReset.TabIndex = 13;
            this.btnCopyReset.Text = "Reset";
            this.btnCopyReset.UseVisualStyleBackColor = true;
            this.btnCopyReset.Click += new System.EventHandler(this.BtnCopyReset_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Copy Template:";
            // 
            // txtCopy
            // 
            this.txtCopy.Location = new System.Drawing.Point(93, 20);
            this.txtCopy.Name = "txtCopy";
            this.txtCopy.Size = new System.Drawing.Size(190, 20);
            this.txtCopy.TabIndex = 11;
            this.tipCopy.SetToolTip(this.txtCopy, "%time% - Timestamp\r\n%body% - Body name\r\n%bodyL% - Body name with landable status\r" +
        "\n%info% - Interest criteria\r\n%detail% - Detailed information");
            this.txtCopy.TextChanged += new System.EventHandler(this.TxtCopy_TextChanged);
            // 
            // cbxTts
            // 
            this.cbxTts.AutoSize = true;
            this.cbxTts.Location = new System.Drawing.Point(121, 46);
            this.cbxTts.Name = "cbxTts";
            this.cbxTts.Size = new System.Drawing.Size(99, 17);
            this.cbxTts.TabIndex = 9;
            this.cbxTts.Text = "Text-to-Speech";
            this.cbxTts.UseVisualStyleBackColor = true;
            this.cbxTts.CheckedChanged += new System.EventHandler(this.CbxTts_CheckedChanged);
            // 
            // cbxToast
            // 
            this.cbxToast.AutoSize = true;
            this.cbxToast.Location = new System.Drawing.Point(6, 46);
            this.cbxToast.Name = "cbxToast";
            this.cbxToast.Size = new System.Drawing.Size(113, 17);
            this.cbxToast.TabIndex = 8;
            this.cbxToast.Text = "Popup Notification";
            this.cbxToast.UseVisualStyleBackColor = true;
            this.cbxToast.CheckedChanged += new System.EventHandler(this.CbxToast_CheckedChanged);
            // 
            // tipCopy
            // 
            this.tipCopy.ShowAlways = true;
            // 
            // groupBox_TTS
            // 
            this.groupBox_TTS.Controls.Add(this.btn_TestVol);
            this.groupBox_TTS.Controls.Add(this.trackBar_Volume);
            this.groupBox_TTS.Location = new System.Drawing.Point(359, 13);
            this.groupBox_TTS.Name = "groupBox_TTS";
            this.groupBox_TTS.Size = new System.Drawing.Size(58, 305);
            this.groupBox_TTS.TabIndex = 2;
            this.groupBox_TTS.TabStop = false;
            this.groupBox_TTS.Text = "Volume";
            // 
            // btn_TestVol
            // 
            this.btn_TestVol.Location = new System.Drawing.Point(6, 270);
            this.btn_TestVol.Name = "btn_TestVol";
            this.btn_TestVol.Size = new System.Drawing.Size(45, 23);
            this.btn_TestVol.TabIndex = 1;
            this.btn_TestVol.Text = "Test";
            this.btn_TestVol.UseVisualStyleBackColor = true;
            this.btn_TestVol.Click += new System.EventHandler(this.Btn_TestVol_Click);
            // 
            // trackBar_Volume
            // 
            this.trackBar_Volume.LargeChange = 20;
            this.trackBar_Volume.Location = new System.Drawing.Point(7, 19);
            this.trackBar_Volume.Maximum = 100;
            this.trackBar_Volume.Name = "trackBar_Volume";
            this.trackBar_Volume.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar_Volume.Size = new System.Drawing.Size(45, 254);
            this.trackBar_Volume.TabIndex = 0;
            this.trackBar_Volume.TickFrequency = 10;
            this.trackBar_Volume.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar_Volume.Scroll += new System.EventHandler(this.TrackBar_Volume_Scroll);
            // 
            // linkGit
            // 
            this.linkGit.AutoSize = true;
            this.linkGit.Location = new System.Drawing.Point(370, 321);
            this.linkGit.Name = "linkGit";
            this.linkGit.Size = new System.Drawing.Size(36, 13);
            this.linkGit.TabIndex = 3;
            this.linkGit.TabStop = true;
            this.linkGit.Text = "github";
            this.linkGit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkGit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkGit_LinkClicked);
            // 
            // groupBox_telegram
            // 
            this.groupBox_telegram.Controls.Add(this.btn_TestTelegram);
            this.groupBox_telegram.Controls.Add(this.txtTelegramChatId);
            this.groupBox_telegram.Controls.Add(this.label3);
            this.groupBox_telegram.Controls.Add(this.txtTelegramAPIKey);
            this.groupBox_telegram.Controls.Add(this.label2);
            this.groupBox_telegram.Controls.Add(this.cbxTelegram);
            this.groupBox_telegram.Location = new System.Drawing.Point(14, 421);
            this.groupBox_telegram.Name = "groupBox_telegram";
            this.groupBox_telegram.Size = new System.Drawing.Size(339, 95);
            this.groupBox_telegram.TabIndex = 4;
            this.groupBox_telegram.TabStop = false;
            this.groupBox_telegram.Text = "Telegram";
            // 
            // btn_TestTelegram
            // 
            this.btn_TestTelegram.Location = new System.Drawing.Point(289, 15);
            this.btn_TestTelegram.Name = "btn_TestTelegram";
            this.btn_TestTelegram.Size = new System.Drawing.Size(41, 21);
            this.btn_TestTelegram.TabIndex = 5;
            this.btn_TestTelegram.Text = "Test";
            this.btn_TestTelegram.UseVisualStyleBackColor = true;
            this.btn_TestTelegram.Click += new System.EventHandler(this.Btn_TestTelegram_Click);
            // 
            // txtTelegramChatId
            // 
            this.txtTelegramChatId.Location = new System.Drawing.Point(126, 68);
            this.txtTelegramChatId.Name = "txtTelegramChatId";
            this.txtTelegramChatId.Size = new System.Drawing.Size(194, 20);
            this.txtTelegramChatId.TabIndex = 4;
            this.txtTelegramChatId.TextChanged += new System.EventHandler(this.TxtTelegramChatId_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Telegram Chat ID:";
            // 
            // txtTelegramAPIKey
            // 
            this.txtTelegramAPIKey.Location = new System.Drawing.Point(126, 42);
            this.txtTelegramAPIKey.Name = "txtTelegramAPIKey";
            this.txtTelegramAPIKey.Size = new System.Drawing.Size(194, 20);
            this.txtTelegramAPIKey.TabIndex = 2;
            this.txtTelegramAPIKey.TextChanged += new System.EventHandler(this.TxtTelegramAPIKey_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Telegram Bot API Key:";
            // 
            // cbxTelegram
            // 
            this.cbxTelegram.AutoSize = true;
            this.cbxTelegram.Location = new System.Drawing.Point(6, 19);
            this.cbxTelegram.Name = "cbxTelegram";
            this.cbxTelegram.Size = new System.Drawing.Size(154, 17);
            this.cbxTelegram.TabIndex = 0;
            this.cbxTelegram.Text = "Send Telegram Notification";
            this.cbxTelegram.UseVisualStyleBackColor = true;
            this.cbxTelegram.CheckedChanged += new System.EventHandler(this.CbxTelegram_CheckedChanged);
            // 
            // SettingsFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 528);
            this.Controls.Add(this.groupBox_telegram);
            this.Controls.Add(this.linkGit);
            this.Controls.Add(this.groupBox_TTS);
            this.Controls.Add(this.groupBox_misc);
            this.Controls.Add(this.groupBox_Interest);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsFrm";
            this.Text = "Observatory Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingsFrm_FormClosed);
            this.Load += new System.EventHandler(this.SettingsFrm_Load);
            this.groupBox_Interest.ResumeLayout(false);
            this.groupBox_Interest.PerformLayout();
            this.groupBox_misc.ResumeLayout(false);
            this.groupBox_misc.PerformLayout();
            this.groupBox_TTS.ResumeLayout(false);
            this.groupBox_TTS.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Volume)).EndInit();
            this.groupBox_telegram.ResumeLayout(false);
            this.groupBox_telegram.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_Interest;
        private System.Windows.Forms.CheckBox cbx_LandWithTerra;
        private System.Windows.Forms.CheckBox cbx_VeryInteresting;
        private System.Windows.Forms.CheckBox cbx_AllJumpSystem;
        private System.Windows.Forms.CheckBox cbx_AllJumpBody;
        private System.Windows.Forms.CheckBox cbx_GoodJump;
        private System.Windows.Forms.CheckBox cbx_HighEccentric;
        private System.Windows.Forms.CheckBox cbx_FastOrbit;
        private System.Windows.Forms.CheckBox cbx_FastRotate;
        private System.Windows.Forms.CheckBox cbx_TinyObject;
        private System.Windows.Forms.CheckBox cbx_NestedMoon;
        private System.Windows.Forms.CheckBox cbx_CollidingBinary;
        private System.Windows.Forms.CheckBox cbx_CloseBinary;
        private System.Windows.Forms.CheckBox cbx_ShepherdMoon;
        private System.Windows.Forms.CheckBox cbx_CloseOrbit;
        private System.Windows.Forms.CheckBox cbx_LandHighG;
        private System.Windows.Forms.CheckBox cbx_LandWithAtmo;
        private System.Windows.Forms.GroupBox groupBox_misc;
        private System.Windows.Forms.CheckBox cbxTts;
        private System.Windows.Forms.CheckBox cbxToast;
        private System.Windows.Forms.Button btnCopyReset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCopy;
        private System.Windows.Forms.ToolTip tipCopy;
        private System.Windows.Forms.CheckBox cbx_custom;
        private System.Windows.Forms.CheckBox cbx_WideRing;
        private System.Windows.Forms.CheckBox cbx_LandLarge;
        private System.Windows.Forms.GroupBox groupBox_TTS;
        private System.Windows.Forms.Button btn_TestVol;
        private System.Windows.Forms.TrackBar trackBar_Volume;
        private System.Windows.Forms.CheckBox cbxLandRing;
        private System.Windows.Forms.CheckBox cbxRinghugger;
        private System.Windows.Forms.LinkLabel linkGit;
        private System.Windows.Forms.CheckBox cbxAutoRead;
        private System.Windows.Forms.CheckBox cbxAutoMonitor;
        private System.Windows.Forms.GroupBox groupBox_telegram;
        private System.Windows.Forms.TextBox txtTelegramChatId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTelegramAPIKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbxTelegram;
        private System.Windows.Forms.Button btn_TestTelegram;
        private System.Windows.Forms.CheckBox cbxCodex;
        private System.Windows.Forms.CheckBox cbxSendToIGAU;
    }
}
