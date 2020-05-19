namespace Observatory
{
    partial class NotifyFrm
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
            this.pictureBox_Observatory = new System.Windows.Forms.PictureBox();
            this.lblText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Observatory)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_Observatory
            // 
            this.pictureBox_Observatory.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_Observatory.Name = "pictureBox_Observatory";
            this.pictureBox_Observatory.Size = new System.Drawing.Size(66, 65);
            this.pictureBox_Observatory.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox_Observatory.TabIndex = 0;
            this.pictureBox_Observatory.TabStop = false;
            // 
            // lblText
            // 
            this.lblText.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblText.Location = new System.Drawing.Point(60, 5);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(235, 52);
            this.lblText.TabIndex = 1;
            this.lblText.Text = "Notification Text";
            // 
            // NotifyFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 66);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.pictureBox_Observatory);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NotifyFrm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "NotifyFrm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Observatory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_Observatory;
        private System.Windows.Forms.Label lblText;
    }
}