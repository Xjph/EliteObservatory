using System;
using System.Net.Http;
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

    public sealed class HttpClient
    {
        private HttpClient()
        { }

        private static readonly Lazy<System.Net.Http.HttpClient> lazy = new Lazy<System.Net.Http.HttpClient>(() => new System.Net.Http.HttpClient());

        public static System.Net.Http.HttpClient Client
        {
            get
            {
                return lazy.Value;
            }
        }

        public static string GetString(string url)
        {
            return lazy.Value.GetStringAsync(url).Result;
        }

        public static HttpResponseMessage SendRequest(HttpRequestMessage request)
        {
            return lazy.Value.SendAsync(request).Result;
        }
    }
}
