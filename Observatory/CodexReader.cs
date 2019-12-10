using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                  if (Properties.Observatory.Default.SendToIGAU)
                  {
                  JObject POST_content = JObject.FromObject(
                new {
                    timestamp = codexEntry.Timestamp.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    Name_Localised = codexEntry.NameLocalised,
                    codexEntry.System
                });
                  var request = new System.Net.Http.HttpRequestMessage
                   {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri($"https://ddss70885k.execute-api.us-west-1.amazonaws.com/Prod"),
                        Content = new StringContent(POST_content)
                   };
                  HttpClient.SendRequest(request);                  
                  }
        }
    }
}
