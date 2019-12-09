using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Observatory
{
    static class CodexReader

    {
        public static void ProcessCodex(CodexEntry codexEntry)

        {
                  // line 22 throws an error:
                  // Error	CS1061	'Observatory' does not contain a definition for 'SendToIGAU' and no accessible extension method 'SendToIGAU'
                  // accepting a first argument of type 'Observatory' could be found (are you missing a using directive or an assembly reference?)
                  //
                  // however this is the value set by cbxSendToIGAU checkbox? Was IncludeCodex set as a global variable somewhere?

                  // if (Properties.Observatory.Default.SendToIGAU)

                  // this works, Need to combine this with the "If" above.
                  if (Properties.Observatory.Default.IncludeCodex)
                  {
                  string POST_content = "{ \"timestamp\":\""+codexEntry.Timestamp.ToString("yyyy-MM-ddTHH:mm:ssZ")+"\", \"Name_Localised\":\""+codexEntry.NameLocalised+"\", \"System\":\""+codexEntry.System+"\" }";
                  var request = new System.Net.Http.HttpRequestMessage
                   {
                        Method = System.Net.Http.HttpMethod.Post,
                        RequestUri = new Uri($"https://ddss70885k.execute-api.us-west-1.amazonaws.com/Prod"),
                        Content = new System.Net.Http.StringContent(POST_content)
                   };
                  // disabled for now, otherwise testing will bombard dynamoDB / API Gateway
                  // HttpClient.SendRequest(request);
                  }
        }
    }
}
