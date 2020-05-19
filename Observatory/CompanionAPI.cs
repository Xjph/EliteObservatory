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
        private bool logging;
        private string path;

        public async Task GetJournals(ObservatoryFrm mainForm, bool logging = false)
        {
            this.logging = logging;
            const string apiEndpoint = "https://companion.orerve.net/";
            retrieve = true;
            uint gap = 0;
            uint failCount = 0;
            bool firstFile = true;
            bool checkContinue = true;
            var journalItems = new List<JObject>();
            var auth = new OAuth();
            path = Properties.Observatory.Default.JournalPath;
            DateTime date = DateTime.UtcNow;
            DirectoryInfo journalInfo = new DirectoryInfo(path);

            Log("Beginning journal retrieval.");
            Log("Journal path: " + path);
            Log($"Current Frontier authentication token (partial): {auth.GetToken().Substring(0, 8)}...{auth.GetToken().Substring(auth.GetToken().Length - 8, 8)}");
            while (retrieve && failCount < 3)
            {
                mainForm.SetStatusText("Requesting journal for: " + date.ToString("yyyy-MM-dd"));
                mainForm.CheckCapi(ObservatoryFrm.CapiState.InProgress);

                //The most recent local journal may have updated, so retrieve it even if already present.
                if (firstFile || journalInfo.GetFiles($"Journal.{date.ToString("yyMMdd")}??????.01.log").Count() == 0)
                {
                    string endpointSuffix = $"journal/{date.Year}/{date.Month}/{date.Day}";

                    Log($"Creating initial request for {date.ToShortDateString()}.");

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apiEndpoint + endpointSuffix);
                    request.Headers.Add("Authorization", "Bearer " + auth.GetToken());

                    Log("Sending request...");
                    var response = await HttpClient.SendRequestAsync(request);
                    Log($"...initial request complete, response status: {response.StatusCode.ToString()}");

                    uint retries = 0;

                    while (retries < 10 && response.StatusCode == HttpStatusCode.PartialContent)
                    {
                        Log("Response incomplete, re-requesting...");
                        HttpRequestMessage requestRetry = new HttpRequestMessage(HttpMethod.Get, apiEndpoint + endpointSuffix);
                        requestRetry.Headers.Add("Authorization", "Bearer " + auth.GetToken());
                        System.Threading.Thread.Sleep(1000);
                        response = await HttpClient.SendRequestAsync(requestRetry);
                        Log($"...request complete, response status: {response.StatusCode.ToString()}");
                        retries++;
                        if (retries == 10)
                            Log("Incomplete response after 10 retries, continuing with incomplete journal data.");
                    }

                    if (response.IsSuccessStatusCode && !(response.StatusCode == HttpStatusCode.NoContent))
                    {
                        Log("Request complete.");
                        string responseText = response.Content.ReadAsStringAsync().Result;
                        string incomplete = string.Empty;
                        Log($"Received content length: {responseText.Length} character{(responseText.Length > 1 ? "s" : "")}.");

                        if (response.StatusCode == HttpStatusCode.PartialContent)
                        {
                            Log("Incomplete response, tagging file as incomplete.");
                            incomplete = "Incomplete";
                        }

                        string filename = $"{path}\\{incomplete}Journal.{date.ToString("yyMMdd")}000000.01.log";

                        if (File.Exists(filename))
                        {
                            Log($"Journal for {date.ToShortDateString()} already exists, deleting.");
                            File.Delete(filename);
                            firstFile = false;
                        }

                        Log($"Writing journal file for {date.ToShortDateString()}.");
                        File.AppendAllText($"{path}\\{incomplete}Journal.{date.ToString("yyMMdd")}000000.01.log", responseText);
                        date = date.AddDays(-1);
                        gap = 0;
                        Log("Journal write complete.");
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        failCount++;
                        Log("Frontier authentication expired or invalid, reauthenticating.");
                        string newToken = auth.GetNewToken();
                        Properties.Observatory.Default.FrontierToken = newToken;
                        Properties.Observatory.Default.Save();
                        Log($"Authentication Complete, new token (partial): {newToken.Substring(0,8)}...{newToken.Substring(newToken.Length - 8, 8)}");
                    }
                    else if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        Log($"No data for {date.ToShortDateString()}.");
                        date = date.AddDays(-1);
                        gap++;
                        Log($"Continuing for {date.ToShortDateString()}, data gap {gap} day{(gap > 1 ? "s" : "")}");
                    }
                }
                else
                {
                    Log($"Journal already present for {date.ToShortDateString()}");
                    if (checkContinue)
                    {
                        Log($"Prompting to continue requests...");
                        var result = MessageBox.Show($"Local journal already present for {date.ToString("yyyy-MM-dd")}, continue?", "Local Journal Found", MessageBoxButtons.YesNo);
                        if (result == DialogResult.No)
                        {
                            retrieve = false;
                            Log("...declined, stopping.");
                        }
                        else
                        {
                            checkContinue = false;
                            Log("...confirmed, continuing.");
                        }
                    }
                    date = date.AddDays(-1);
                    gap = 0;
                }

                if (gap > 14)
                {
                    Log($"No data for 14 consecutive days, prompting to continue requests...");
                    var result = MessageBox.Show($"No data available between {date.ToString("yyyy-MM-dd")} and {date.AddDays(14).ToString("yyyy-MM-dd")}. Continue requesting data for older journals?", "Continue Retrieval?", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        gap = 0;
                        Log($"...confirmed, continuing.");
                    }
                    else
                    {
                        retrieve = false;
                        Log("...declined, stopping.");
                    }
                }
            }
            Log("Process Complete");
            Log("");
            mainForm.SetStatusText(string.Empty);
            mainForm.CheckCapi(ObservatoryFrm.CapiState.Enabled);

        }

        private void Log(string text)
        {
            if (logging)
            {
                Observatory.Log("cAPI> " + text);
            }
        }
    }
}