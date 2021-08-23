using Microsoft.EntityFrameworkCore;


namespace Searchify.Domain.Models
{
    public class Suggestions
    {

        public string id { get; set; }
        public string query { get; set; }
        public int rank { get; set; }
    }
}