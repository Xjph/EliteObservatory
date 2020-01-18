namespace Observatory
{
    partial class ObservatoryFrm
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
            if (speech != null) speech.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ObservatoryFrm));
            this.btnToggleMonitor = new System.Windows.Forms.Button();
            this.listEvent = new System.Windows.Forms.ListView();
            this.body = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.interest = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timestamp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.detail = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.landable = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnReadAll = new System.Windows.Forms.Button();
            this.progressReadAll = new System.Windows.Forms.ProgressBar();
            this.contextCopy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyJournalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSettings = new System.Windows.Forms.Button();
            this.linkUpdate = new System.Windows.Forms.LinkLabel();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblIGAUTransmit = new System.Windows.Forms.Label();
            this.contextCopy.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnToggleMonitor
            // 
            this.btnToggleMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToggleMonitor.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToggleMonitor.Location = new System.Drawing.Point(588, 496);
            this.btnToggleMonitor.Name = "btnToggleMonitor";
            this.btnToggleMonitor.Size = new System.Drawing.Size(109, 23);
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
            this.listEvent.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listEvent.FullRowSelect = true;
            this.listEvent.HideSelection = false;
            this.listEvent.Location = new System.Drawing.Point(12, 12);
            this.listEvent.Name = "listEvent";
            this.listEvent.Size = new System.Drawing.Size(685, 478);
            this.listEvent.TabIndex = 2;
            this.listEvent.UseCompatibleStateImageBehavior = false;
            this.listEvent.View = System.Windows.Forms.View.Details;
            this.listEvent.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ListEvent_ColumnClick);
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
            this.btnReadAll.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReadAll.Location = new System.Drawing.Point(494, 496);
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
            this.progressReadAll.Size = new System.Drawing.Size(683, 23);
            this.progressReadAll.TabIndex = 4;
            this.progressReadAll.Visible = false;
            // 
            // contextCopy
            // 
            this.contextCopy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyNameToolStripMenuItem,
            this.copyAllToolStripMenuItem,
            this.copyJournalToolStripMenuItem});
            this.contextCopy.Name = "contextCopy";
            this.contextCopy.Size = new System.Drawing.Size(144, 70);
            // 
            // copyNameToolStripMenuItem
            // 
            this.copyNameToolStripMenuItem.Name = "copyNameToolStripMenuItem";
            this.copyNameToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.copyNameToolStripMenuItem.Text = "Copy Name";
            this.copyNameToolStripMenuItem.Click += new System.EventHandler(this.CopyNameToolStripMenuItem_Click);
            // 
            // copyAllToolStripMenuItem
            // 
            this.copyAllToolStripMenuItem.Name = "copyAllToolStripMenuItem";
            this.copyAllToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.copyAllToolStripMenuItem.Text = "Copy All";
            this.copyAllToolStripMenuItem.Click += new System.EventHandler(this.CopyAllToolStripMenuItem_Click);
            // 
            // copyJournalToolStripMenuItem
            // 
            this.copyJournalToolStripMenuItem.Name = "copyJournalToolStripMenuItem";
            this.copyJournalToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.copyJournalToolStripMenuItem.Text = "Copy Journal";
            this.copyJournalToolStripMenuItem.Click += new System.EventHandler(this.CopyJournalToolStripMenuItem_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSettings.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSettings.Location = new System.Drawing.Point(332, 496);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(75, 23);
            this.btnSettings.TabIndex = 8;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.BtnSettings_Click);
            // 
            // linkUpdate
            // 
            this.linkUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkUpdate.AutoSize = true;
            this.linkUpdate.Enabled = false;
            this.linkUpdate.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkUpdate.Location = new System.Drawing.Point(12, 501);
            this.linkUpdate.Name = "linkUpdate";
            this.linkUpdate.Size = new System.Drawing.Size(94, 13);
            this.linkUpdate.TabIndex = 9;
            this.linkUpdate.TabStop = true;
            this.linkUpdate.Text = "Update Available";
            this.linkUpdate.Visible = false;
            this.linkUpdate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkUpdate_LinkClicked);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(413, 496);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 10;
            this.btnClear.Text = "Clear List";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // lblIGAUTransmit
            // 
            this.lblIGAUTransmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblIGAUTransmit.BackColor = System.Drawing.SystemColors.Control;
            this.lblIGAUTransmit.Location = new System.Drawing.Point(112, 501);
            this.lblIGAUTransmit.Name = "lblIGAUTransmit";
            this.lblIGAUTransmit.Size = new System.Drawing.Size(214, 13);
            this.lblIGAUTransmit.TabIndex = 11;
            this.lblIGAUTransmit.Text = "IGAU";
            this.lblIGAUTransmit.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblIGAUTransmit.Visible = false;
            // 
            // ObservatoryFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 530);
            this.Controls.Add(this.progressReadAll);
            this.Controls.Add(this.lblIGAUTransmit);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.linkUpdate);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnReadAll);
            this.Controls.Add(this.listEvent);
            this.Controls.Add(this.btnToggleMonitor);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ObservatoryFrm";
            this.Text = "Elite Observatory";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ObservatoryFrm_FormClosing);
            this.Load += new System.EventHandler(this.ObservatoryFrm_Load);
            this.Shown += new System.EventHandler(this.ObservatoryFrm_Shown);
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
        private System.Windows.Forms.ColumnHeader timestamp;
        private System.Windows.Forms.ColumnHeader detail;
        private System.Windows.Forms.ColumnHeader landable;
        private System.Windows.Forms.ContextMenuStrip contextCopy;
        private System.Windows.Forms.ToolStripMenuItem copyNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyAllToolStripMenuItem;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.LinkLabel linkUpdate;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ToolStripMenuItem copyJournalToolStripMenuItem;
        private System.Windows.Forms.Label lblIGAUTransmit;
    }
}

