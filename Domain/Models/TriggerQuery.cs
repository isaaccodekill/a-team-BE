using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace Searchify.Domain.Models
{



    public class TriggerQuery
    {

        public enum TriggerType
        {
            [Description("UPDATE")]
            UPDATE = 0,
            [Description("DELETE")]
            DELETE = 1,
            [Description("ADD")]
            ADD = 2,

        }

        [BindRequired]
        [JsonProperty("trigger")]
        public TriggerType trigger { get; set; }

        [BindRequired]
        [JsonProperty("docs")]
        public IEnumerable<Document> docs { get; set; }

    }
}