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
            var SendTask = SendRequests();
        }

        private static async Task SendRequests()
        {
            if (!RequestsSending)
            {
                RequestsSending = true;
                startTicks = Environment.TickCount;
                var mainForm = System.Windows.Forms.Application.OpenForms.OfType<ObservatoryFrm>().First();
                while (RequestQueue.Count > 0)
                {
#if DEBUG
                    var response = HttpClient.SendRequestAsync(RequestQueue.First());
                    System.IO.File.AppendAllText(@"C:\temp\CodexTransmit.log", $"Time: {Environment.TickCount - startTicks}; ResponseCode: {response.Result.StatusCode.ToString()}; Response: {response.Result.Content.ReadAsStringAsync().Result};\r\n");
                    await Task.Delay(750);
#else
                    var SendRequestTask = HttpClient.SendRequestAsync(RequestQueue.First());

                    //Requests complete much more quickly than this, but the IGAU endpoint will start throwing HTTP429 "Too Many Requests"
                    //so we have to wait a moment between transmissions.
                    await Task.Delay(1000);
                    await SendRequestTask;

                    if (!SendRequestTask.Result.IsSuccessStatusCode)
                    {
                        System.Windows.Forms.MessageBox.Show($"HTTP Error: {SendRequestTask.Result.StatusCode.ToString()}\r\nContent: {SendRequestTask.Result.Content}\r\nTransmission disabled.", "Error Sending IGAU Data");
                        RequestQueue.Clear();
                        mainForm.SetIGAUText(string.Empty);
                        Properties.Observatory.Default.SendToIGAU = false;
                        Properties.Observatory.Default.Save();
                        break;
                    }
#endif

                    RequestQueue.Remove(RequestQueue.First());

                    mainForm.SetIGAUText(RequestQueue.Count > 0 ? $"{RequestQueue.Count} pending IGAU transmission{(RequestQueue.Count > 1 ? "s" : "")}.":"");
                    
                }
                RequestsSending = false;
            }
        }
    }
}
