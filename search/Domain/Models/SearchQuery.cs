using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Searchify.Domain.Models
{
    /// <summary>
    /// model for incoming search requests
    /// </summary>
    public class SearchQuery
    {

        /// <summary>
        /// query string param
        /// </summary>
        [BindRequired]
        public string query { get; set; }
        /// <summary>
        /// autocomplete param to determine whether to search the index or show suggestions
        /// </summary>
        public bool autocomplete { get; set; }
    }
}