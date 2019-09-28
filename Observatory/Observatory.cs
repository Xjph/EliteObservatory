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
            Application.Run(new ObservatoryFrm());
        }
    }
}
