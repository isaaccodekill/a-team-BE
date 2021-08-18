using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Searchify.Domain.Models
{
    public class SearchQuery
    {

        [BindRequired]
        public string query { get; set; }
        public bool autocomplete { get; set; }
    }
}