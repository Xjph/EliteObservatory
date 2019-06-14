namespace EDDisco
{
    partial class EDDiscoFrm
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
            speech?.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EDDiscoFrm));
            this.btnToggleMonitor = new System.Windows.Forms.Button();
            this.listEvent = new System.Windows.Forms.ListView();
            this.body = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.interest = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timestamp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.detail = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.landable = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnReadAll = new System.Windows.Forms.Button();
            this.progressReadAll = new System.Windows.Forms.ProgressBar();
            this.cbxToast = new System.Windows.Forms.CheckBox();
            this.cbxTts = new System.Windows.Forms.CheckBox();
            this.contextCopy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cbxTtsDetail = new System.Windows.Forms.CheckBox();
            this.contextCopy.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnToggleMonitor
            // 
            this.btnToggleMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToggleMonitor.Location = new System.Drawing.Point(597, 496);
            this.btnToggleMonitor.Name = "btnToggleMonitor";
            this.btnToggleMonitor.Size = new System.Drawing.Size(100, 23);
            this.btnToggleMonitor.TabIndex = 1;
            this.btnToggleMonitor.Text = "Start Monitoring";
            this.btnToggleMonitor.UseVisualStyleBackColor = true;
            this.btnToggleMonitor.Click += new System.EventHandler(this.BtnToggleMonitor_Click);
            // 
            // listEvent
            // 
            this.listEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listEvent.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.body,
            this.interest,
            this.timestamp,
            this.detail,
            this.landable});
            this.listEvent.FullRowSelect = true;
            this.listEvent.Location = new System.Drawing.Point(12, 12);
            this.listEvent.Name = "listEvent";
            this.listEvent.Size = new System.Drawing.Size(685, 478);
            this.listEvent.TabIndex = 2;
            this.listEvent.UseCompatibleStateImageBehavior = false;
            this.listEvent.View = System.Windows.Forms.View.Details;
            this.listEvent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ListEvent_KeyDown);
            this.listEvent.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ListEvent_MouseClick);
            // 
            // body
            // 
            this.body.DisplayIndex = 1;
            this.body.Text = "Body";
            this.body.Width = 146;
            // 
            // interest
            // 
            this.interest.DisplayIndex = 3;
            this.interest.Text = "Description";
            this.interest.Width = 177;
            // 
            // timestamp
            // 
            this.timestamp.DisplayIndex = 0;
            this.timestamp.Text = "Time";
            this.timestamp.Width = 114;
            // 
            // detail
            // 
            this.detail.DisplayIndex = 4;
            this.detail.Text = "Detail";
            this.detail.Width = 200;
            // 
            // landable
            // 
            this.landable.DisplayIndex = 2;
            this.landable.Text = "";
            this.landable.Width = 28;
            // 
            // btnReadAll
            // 
            this.btnReadAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReadAll.Location = new System.Drawing.Point(503, 496);
            this.btnReadAll.Name = "btnReadAll";
            this.btnReadAll.Size = new System.Drawing.Size(88, 23);
            this.btnReadAll.TabIndex = 3;
            this.btnReadAll.Text = "Read All Logs";
            this.btnReadAll.UseVisualStyleBackColor = true;
            this.btnReadAll.Click += new System.EventHandler(this.BtnReadAll_Click);
            // 
            // progressReadAll
            // 
            this.progressReadAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressReadAll.Location = new System.Drawing.Point(14, 496);
            this.progressReadAll.Name = "progressReadAll";
            this.progressReadAll.Size = new System.Drawing.Size(483, 23);
            this.progressReadAll.TabIndex = 4;
            this.progressReadAll.Visible = false;
            // 
            // cbxToast
            // 
            this.cbxToast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxToast.AutoSize = true;
            this.cbxToast.Location = new System.Drawing.Point(14, 500);
            this.cbxToast.Name = "cbxToast";
            this.cbxToast.Size = new System.Drawing.Size(113, 17);
            this.cbxToast.TabIndex = 5;
            this.cbxToast.Text = "Popup Notification";
            this.cbxToast.UseVisualStyleBackColor = true;
            this.cbxToast.CheckedChanged += new System.EventHandler(this.CbxToast_CheckedChanged);
            // 
            // cbxTts
            // 
            this.cbxTts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxTts.AutoSize = true;
            this.cbxTts.Location = new System.Drawing.Point(129, 500);
            this.cbxTts.Name = "cbxTts";
            this.cbxTts.Size = new System.Drawing.Size(99, 17);
            this.cbxTts.TabIndex = 6;
            this.cbxTts.Text = "Text-to-Speech";
            this.cbxTts.UseVisualStyleBackColor = true;
            this.cbxTts.CheckedChanged += new System.EventHandler(this.CbxTts_CheckedChanged);
            // 
            // contextCopy
            // 
            this.contextCopy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyNameToolStripMenuItem,
            this.copyAllToolStripMenuItem});
            this.contextCopy.Name = "contextCopy";
            this.contextCopy.Size = new System.Drawing.Size(138, 48);
            // 
            // copyNameToolStripMenuItem
            // 
            this.copyNameToolStripMenuItem.Name = "copyNameToolStripMenuItem";
            this.copyNameToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.copyNameToolStripMenuItem.Text = "Copy Name";
            this.copyNameToolStripMenuItem.Click += new System.EventHandler(this.CopyNameToolStripMenuItem_Click);
            // 
            // copyAllToolStripMenuItem
            // 
            this.copyAllToolStripMenuItem.Name = "copyAllToolStripMenuItem";
            this.copyAllToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.copyAllToolStripMenuItem.Text = "Copy All";
            this.copyAllToolStripMenuItem.Click += new System.EventHandler(this.CopyAllToolStripMenuItem_Click);
            // 
            // cbxTtsDetail
            // 
            this.cbxTtsDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxTtsDetail.AutoSize = true;
            this.cbxTtsDetail.Location = new System.Drawing.Point(235, 500);
            this.cbxTtsDetail.Name = "cbxTtsDetail";
            this.cbxTtsDetail.Size = new System.Drawing.Size(115, 17);
            this.cbxTtsDetail.TabIndex = 7;
            this.cbxTtsDetail.Text = "TTS Include Detail";
            this.cbxTtsDetail.UseVisualStyleBackColor = true;
            this.cbxTtsDetail.Visible = false;
            // 
            // EDDiscoFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 530);
            this.Controls.Add(this.progressReadAll);
            this.Controls.Add(this.cbxTtsDetail);
            this.Controls.Add(this.btnReadAll);
            this.Controls.Add(this.listEvent);
            this.Controls.Add(this.btnToggleMonitor);
            this.Controls.Add(this.cbxTts);
            this.Controls.Add(this.cbxToast);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EDDiscoFrm";
            this.Text = "EDDiscoMon";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EDDiscoFrm_FormClosing);
            this.contextCopy.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnToggleMonitor;
        private System.Windows.Forms.ListView listEvent;
        private System.Windows.Forms.ColumnHeader body;
        private System.Windows.Forms.ColumnHeader interest;
        private System.Windows.Forms.Button btnReadAll;
        private System.Windows.Forms.ProgressBar progressReadAll;
        private System.Windows.Forms.CheckBox cbxToast;
        private System.Windows.Forms.CheckBox cbxTts;
        private System.Windows.Forms.ColumnHeader timestamp;
        private System.Windows.Forms.ColumnHeader detail;
        private System.Windows.Forms.ColumnHeader landable;
        private System.Windows.Forms.ContextMenuStrip contextCopy;
        private System.Windows.Forms.ToolStripMenuItem copyNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyAllToolStripMenuItem;
        private System.Windows.Forms.CheckBox cbxTtsDetail;
    }
}

