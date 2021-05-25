using System;
using Newtonsoft.Json;

namespace Observatory
{
    public class ScanOrganicEvent
    {
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("ScanType")]
        public string ScanType { get; set; }

        [JsonProperty("Genus")]
        public string Genus { get; set; }

        [JsonProperty("Genus_Localised")]
        public string Genus_Localised { get; set; }

        [JsonProperty("Species")]
        public string Species { get; set; }

        [JsonProperty("Species_Localised")]
        public string Species_Localised { get; set; }

        [JsonProperty("SystemAddress")]
        public ulong SystemAddress { get; set; }

        [JsonProperty("Body")]
        public int Body { get; set; }

        public string CurrentSystem;
    }
}
