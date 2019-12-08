using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Observatory
{
    static class CodexReader
    {
        public static void ProcessCodex(CodexEntry codexEntry)
        {
                  // the if statement below barfs this error: Error	CS0103	The name 'settings' does not exist in the current context
                  // if (settings.IncludeCodex)
                  // {
                  // need to build strings from the CodexEntry data. Specifically timestamp, Name_Localised, and system
                  // might not be needed if the values can be pulled directly when building the HTTP POST data payload (line 35)
                  // the line below barfs out this error: Error	CS0120	An object reference is required for the non-static field, method, or property 'CodexEntry.Timestamp'
                  // string timestamp = CodexEntry.Timestamp;

                  // this is all straight forward HTTP calls.
                  // var request = new System.Net.Http.HttpRequestMessage
                  // {
                  //    Method = System.Net.Http.HttpMethod.Post,
                  //    RequestUri = new Uri($"https://ddss70885k.execute-api.us-west-1.amazonaws.com/Prod")

                  // Need to add the actual "content" to send via POST - the Telegram example code throws everything in the URL.
                  // API Gateway/DynamoDB expect a JSON payload sent to the endpoint URL via HTTP POST


                  // Need to convert this python to C++ / c# :
                  // DATA_STR = '{{ "timestamp":"{}", "Name_Localised":"{}", "System":"{}" }}'.format(entry['timestamp'], entry['Name_Localised'], entry['System'])
                  // the attempt below sort of works, however the CodexEntry portions need to be the data from CodexEntry.cs

                  string igau_data = "{ \"timestamp\":\"CodexEntry.Timestamp\", \"Name_Localised\":\"CodexEntry.NameLocalised\", \"System\":\"CodexEntry.System\" }" ;
                  // };

                  // modify this below to send the igau_data as the content via HTTP POST to RequestUri above
                  // string response = HttpClient.SendRequest(request).Content.ReadAsStringAsync().Result;

                  // the line below is for debugging - will pop up an os window alert box upon CodexEntry event
                  //System.Windows.Forms.MessageBox.Show(igau_data);

                  //}
        }
    }
}
