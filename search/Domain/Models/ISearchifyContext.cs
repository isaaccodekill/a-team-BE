using Microsoft.EntityFrameworkCore;

namespace Searchify.Domain.Models
{
    public interface ISearchifyContext
    {
        DbSet<Document> Documents { get; set; }
        DbSet<Suggestions> Suggestions { get; set; }

    }
}