using Microsoft.EntityFrameworkCore;


namespace Searchify.Domain.Models
{
    /// <summary>
    /// DB model for auto complete suggestions
    /// </summary>
    public class Suggestions
    {

        public string id { get; set; }
        /// <summary>
        /// string query
        /// </summary>
        public string query { get; set; }
        /// <summary>
        /// rank of the suggestion, to help determine how valid the suggestion is to the query
        /// </summary>
        public int rank { get; set; }
    }
}