using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Observatory
{
    public partial class frmCriteriaEdit : Form
    {
        const string CriteriaFile = "ObservatoryCriteria.xml";
        public frmCriteriaEdit()
        {
            InitializeComponent();
        }

        private void FrmCriteriaEdit_Load(object sender, EventArgs e)
        {
            if (File.Exists(CriteriaFile))
            {
                txtCriteriaXml.Text = File.ReadAllText(CriteriaFile);
                txtCriteriaXml.Select(0,0);
            }
            AddLineNumbers();
            txtCriteriaXml.Scroll += CriteriaScroll;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            bool originalSetting = Properties.Observatory.Default.CustomRules;
            try
            {
                Properties.Observatory.Default.CustomRules = true;
                var userInterest = new UserInterest(txtCriteriaXml.Text);

                if (File.Exists(CriteriaFile))
                {
                    File.Delete(CriteriaFile);
                }
                File.WriteAllText(CriteriaFile, txtCriteriaXml.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Criteria XML Error");
            }
            finally
            {
                Properties.Observatory.Default.CustomRules = originalSetting;
            }
        }

        private void AddLineNumbers()
        {
            int First_Index = txtCriteriaXml.GetCharIndexFromPosition(Point.Empty + txtCriteriaXml.Margin.Size);
            int First_Line = txtCriteriaXml.GetLineFromCharIndex(First_Index);
            int Last_Index = txtCriteriaXml.GetCharIndexFromPosition(new Point(txtCriteriaXml.ClientSize - txtCriteriaXml.Margin.Size));
            int Last_Line = txtCriteriaXml.GetLineFromCharIndex(Last_Index);
            txtLineNum.Text = "";
            StringBuilder lineNumbers = new StringBuilder();
            for (int i = First_Line; i <= Last_Line + 2; i++)
            {
                lineNumbers.AppendLine((i+1).ToString());
            }
            txtLineNum.Text = lineNumbers.ToString();
        }

        private void frmCriteriaEdit_Resize(object sender, EventArgs e)
        {
            AddLineNumbers();
        }

        private void CriteriaScroll(object sender, EventArgs e)
        {
            AddLineNumbers();
        }
    }

    public partial class ScrollEventTextBox : TextBox
    {
        private const int WM_HSCROLL = 0x114;
        private const int WM_VSCROLL = 0x115;
        private const int WM_MOUSEWHEEL = 0x20A;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_VSCROLL || m.Msg == WM_HSCROLL || m.Msg == WM_MOUSEWHEEL)
            {
                OnScroll(null);
            }
        }

        protected virtual void OnScroll(EventArgs e)
        {
            Scroll?.Invoke(this, e);
        }

        public event EventHandler Scroll;
        

    }

}
