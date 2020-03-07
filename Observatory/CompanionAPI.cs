using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net;
using System.IO;

namespace Observatory
{
    public class CompanionAPI
    {
        public bool retrieve;

        public async Task GetJournals(ObservatoryFrm mainForm)
        {
            const string apiEndpoint = "https://companion.orerve.net/";
            retrieve = true;
            uint gap = 0;
            bool firstFile = true;
            bool checkContinue = true;
            var journalItems = new List<JObject>();
            var auth = new OAuth();
            string path = Properties.Observatory.Default.JournalPath;
            DateTime date = DateTime.UtcNow;
            DirectoryInfo journalInfo = new DirectoryInfo(path);
            
            while (retrieve)
            {
                mainForm.SetStatusText("Requesting journal for: " + date.ToString("yyyy-MM-dd"));
                mainForm.CheckCapi(ObservatoryFrm.CapiState.InProgress);

                //The most recent local journal may have updated, so retrieve it even if already present.
                if (firstFile || journalInfo.GetFiles($"Journal.{date.ToString("yyMMdd")}??????.01.log").Count() == 0)
                {
                    string endpointSuffix = $"journal/{date.Year}/{date.Month}/{date.Day}";

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apiEndpoint + endpointSuffix);
                    request.Headers.Add("Authorization", "Bearer " + auth.GetToken());
                    
                    var response = await HttpClient.SendRequestAsync(request);

                    uint retries = 0;

                    while (retries < 10 && response.StatusCode == HttpStatusCode.PartialContent)
                    {
                        HttpRequestMessage requestRetry = new HttpRequestMessage(HttpMethod.Get, apiEndpoint + endpointSuffix);
                        requestRetry.Headers.Add("Authorization", "Bearer " + auth.GetToken());
                        System.Threading.Thread.Sleep(1000);
                        response = await HttpClient.SendRequestAsync(requestRetry);
                        retries++;
                    }

                    if (response.IsSuccessStatusCode && !(response.StatusCode == HttpStatusCode.NoContent))
                    {
                        string responseText = response.Content.ReadAsStringAsync().Result;
                        string incomplete = string.Empty;

                        if (response.StatusCode == HttpStatusCode.PartialContent)
                            incomplete = "Incomplete";

                        string filename = $"{path}\\{incomplete}Journal.{date.ToString("yyMMdd")}000000.01.log";

                        if (File.Exists(filename))
                        {
                            File.Delete(filename);
                            firstFile = false;
                        }

                        File.AppendAllText($"{path}\\{incomplete}Journal.{date.ToString("yyMMdd")}000000.01.log", responseText);
                        date = date.AddDays(-1);
                        gap = 0;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        string newToken = auth.GetNewToken();
                        Properties.Observatory.Default.FrontierToken = newToken;
                        Properties.Observatory.Default.Save();
                    }
                    else if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        date = date.AddDays(-1);
                        gap++;
                    }
                }
                else
                {
                    if (checkContinue)
                    {
                        var result = MessageBox.Show($"Local journal already present for {date.ToString("yyyy-MM-dd")}, continue?", "Local Journal Found", MessageBoxButtons.YesNo);
                        if (result == DialogResult.No)
                        {
                            retrieve = false;
                        }
                        else
                        {
                            checkContinue = false;
                        }
                    }
                    date = date.AddDays(-1);
                    gap = 0;
                }

                if (gap > 14)
                {
                    var result = MessageBox.Show($"No data available between {date.ToString("yyyy-MM-dd")} and {date.AddDays(14).ToString("yyyy-MM-dd")}. Continue requesting data for older journals?", "Continue Retrieval?", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        gap = 0;
                    }
                    else
                    {
                        retrieve = false;
                    }
                }
            }

            mainForm.SetStatusText(string.Empty);
            mainForm.CheckCapi(ObservatoryFrm.CapiState.Enabled);

        }
    }
}