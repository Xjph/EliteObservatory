using System.Drawing;
using System.Windows.Forms;

namespace Observatory
{
    public partial class NotifyFrm : Form
    {
        private Timer timer;

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        private const int WS_EX_TOPMOST = 0x00000008;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= WS_EX_TOPMOST;
                return createParams;
            }
        }

        public NotifyFrm(string text)
        {
            InitializeComponent();

            int textHeight = GetTextHeight(text, lblText);
            if (textHeight > lblText.Height)
            {
                int heightChange = textHeight - lblText.Height;
                Height += heightChange;
                lblText.Height = textHeight;
            }
            lblText.Text = text;
            pictureBox_Observatory.Image = Icon.ExtractAssociatedIcon(Application.ExecutablePath).ToBitmap();
            StartPosition = FormStartPosition.Manual;
            Rectangle desktopArea = Screen.GetWorkingArea(this);
            Location = new Point(desktopArea.Right - Width, desktopArea.Bottom - Height);
        }

        public void Show(int timeout)
        {
            timer = new Timer();
            timer.Tick += delegate { Close(); };
            timer.Interval = timeout;
            timer.Start();
            Show();
        }

        private int GetTextHeight(string text, Label lbl)
        {
            int textHeight;

            using (Graphics g = CreateGraphics())
            {
                SizeF size = g.MeasureString(text, lbl.Font, lbl.Width);
                textHeight = (int)System.Math.Ceiling(size.Height);
            }

            return textHeight;
        }
    }
}