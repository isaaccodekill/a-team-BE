using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations.Schema;

namespace Searchify.Domain.Models
{
    public class Document
    {


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint id { get; set; }

        [JsonRequired]
        [JsonProperty("url")]
        public string url { get; set; }
        [JsonRequired]
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonRequired]
        [JsonProperty("preview_text")]
        public string preview_text { get; set; }

        [JsonIgnore]
        public bool black_listed { get; set; }
        [JsonIgnore]
        public bool white_listed { get; set; }
    }
}
