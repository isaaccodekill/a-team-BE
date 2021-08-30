using Newtonsoft.Json;

using System.ComponentModel.DataAnnotations.Schema;

namespace Searchify.Domain.Models
{
    /// <summary>
    /// Database model for Document
    /// </summary>
    public class Document
    {

        /// <summary>
        /// auto generated id the doc
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        /// <summary>
        /// document url
        /// </summary>
        [JsonRequired]
        [JsonProperty("url")]
        public string url { get; set; }


        /// <summary>
        /// name of the document
        /// </summary>
        [JsonRequired]
        [JsonProperty("name")]
        public string name { get; set; }

        /// <summary>
        /// preview text of the document
        /// </summary>
        [JsonRequired]
        [JsonProperty("preview_text")]
        public string preview_text { get; set; }

        [JsonIgnore]
        public bool black_listed { get; set; }
        [JsonIgnore]
        public bool white_listed { get; set; }
    }
}
