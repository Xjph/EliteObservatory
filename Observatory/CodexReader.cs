using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Observatory
{
    static class CodexReader

    {
        public static void ProcessCodex(CodexEntry codexEntry)
        {
            if (Properties.Observatory.Default.SendToIGAU && codexEntry.VoucherAmount > 0)
            {

                JObject POST_content = JObject.FromObject(
                    new
                    {
                        timestamp = codexEntry.Timestamp.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        Name_Localised = codexEntry.NameLocalised,
                        codexEntry.System
                    });

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"https://ddss70885k.execute-api.us-west-1.amazonaws.com/Prod"),
                    Content = new StringContent(POST_content.ToString(Formatting.None))
                };

                QueueRequest(request);

            }
        }

        private static List<HttpRequestMessage> RequestQueue;

        private static bool RequestsSending;

        private static int startTicks;

        private static void QueueRequest(HttpRequestMessage request)
        {
            if (RequestQueue == null)
            {
                RequestQueue = new List<HttpRequestMessage>();
            }

            RequestQueue.Add(request);
            SendRequests();
        }

        private static async void SendRequests()
        {
            if (!RequestsSending)
            {
                RequestsSending = true;
                startTicks = Environment.TickCount;
                var mainForm = System.Windows.Forms.Application.OpenForms.OfType<ObservatoryFrm>().First();
                while (RequestQueue.Count > 0)
                {
#if DEBUG
                    var response =
#endif
                    HttpClient.SendRequestAsync(RequestQueue.First());
#if DEBUG
                    System.IO.File.AppendAllText(@"C:\temp\CodexTransmit.log", $"Time: {Environment.TickCount - startTicks}; ResponseCode: {response.Result.StatusCode.ToString()}; Response: {response.Result.Content.ReadAsStringAsync().Result};\r\n");
#endif
                    await Task.Delay(750);
                    RequestQueue.Remove(RequestQueue.First());

                    mainForm.SetIGAUText(RequestQueue.Count > 0 ? $"{RequestQueue.Count} pending IGAU transmission{(RequestQueue.Count > 1 ? "s" : "")}.":"");
                    
                }
                RequestsSending = false;
            }
        }
    }
}
