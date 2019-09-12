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
            Application.Run(new ObservatoryFrm());
        }
    }
}
