using System;
using Newtonsoft.Json;

namespace Observatory
{
    public partial class CodexEntry
    {
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("EntryID")]
        public uint EntryID { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Name_Localised")]
        public string NameLocalised { get; set; }

        [JsonProperty("SubCategory")]
        public string SubCategory { get; set; }

        [JsonProperty("SubCategory_Localised")]
        public string SubCategoryLocalised { get; set; }

        [JsonProperty("Category")]
        public string Category { get; set; }

        [JsonProperty("Category_Localised")]
        public string CategoryLocalised { get; set; }

        [JsonProperty("Region")]
        public string Region { get; set; }

        [JsonProperty("Region_Localised")]
        public string RegionLocalised { get; set; }

        [JsonProperty("System")]
        public string System { get; set; }

        [JsonProperty("SystemAddress")]
        public ulong SystemAddress { get; set; }

        [JsonProperty("NearestDestination", NullValueHandling = NullValueHandling.Ignore)]
        public string NearestDestination { get; set; }

        [JsonProperty("NearestDestination_Localised", NullValueHandling = NullValueHandling.Ignore)]
        public string NearestDestinationLocalised { get; set; }

        [JsonProperty("IsNewEntry")]
        public bool IsNewEntry { get; set; }

        [JsonProperty("VoucherAmount", NullValueHandling = NullValueHandling.Ignore)]
        public uint VoucherAmount { get; set; }

        public string Body;
    }
}
