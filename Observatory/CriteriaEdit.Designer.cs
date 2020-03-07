namespace Observatory
{
    partial class frmCriteriaEdit
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtCriteriaXml = new ScrollEventTextBox();
            this.txtLineNum = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(605, 415);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(102, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Verify && Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(713, 415);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // txtCriteriaXml
            // 
            this.txtCriteriaXml.AcceptsReturn = true;
            this.txtCriteriaXml.AcceptsTab = true;
            this.txtCriteriaXml.AllowDrop = true;
            this.txtCriteriaXml.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCriteriaXml.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCriteriaXml.Location = new System.Drawing.Point(58, 12);
            this.txtCriteriaXml.MaxLength = 131072;
            this.txtCriteriaXml.Multiline = true;
            this.txtCriteriaXml.Name = "txtCriteriaXml";
            this.txtCriteriaXml.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCriteriaXml.Size = new System.Drawing.Size(730, 397);
            this.txtCriteriaXml.TabIndex = 0;
            this.txtCriteriaXml.WordWrap = false;
            // 
            // txtLineNum
            // 
            this.txtLineNum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtLineNum.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtLineNum.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLineNum.Location = new System.Drawing.Point(12, 12);
            this.txtLineNum.Multiline = true;
            this.txtLineNum.Name = "txtLineNum";
            this.txtLineNum.Size = new System.Drawing.Size(49, 397);
            this.txtLineNum.TabIndex = 3;
            this.txtLineNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // frmCriteriaEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ControlBox = false;
            this.Controls.Add(this.txtLineNum);
            this.Controls.Add(this.txtCriteriaXml);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmCriteriaEdit";
            this.Text = "Custom Criteria XML";
            this.Load += new System.EventHandler(this.FrmCriteriaEdit_Load);
            this.Resize += new System.EventHandler(this.frmCriteriaEdit_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private ScrollEventTextBox txtCriteriaXml;
        private System.Windows.Forms.TextBox txtLineNum;
    }
}