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
    }
}