using System;
using System.Windows.Forms;

namespace Observatory
{
    static class Observatory
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (Properties.Observatory.Default.SettingsImport)
            {
                Properties.Observatory.Default.Upgrade();
                Properties.Observatory.Default.SettingsImport = false;
                Properties.Observatory.Default.Save();
            }
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            Application.Run(new ObservatoryFrm());
        }
    }

    static class ExtendControl
    {
        public static void DoubleBuffered(this Control control, bool enable)
        {
            var doubleBufferPropertyInfo = control.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            doubleBufferPropertyInfo.SetValue(control, enable, null);
        }
    }
}
