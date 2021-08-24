using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


namespace Searchify.Domain.Models
{
    public class SearchifyContext : DbContext, ISearchifyContext
    {
        public SearchifyContext(DbContextOptions<SearchifyContext> options) : base(options)
        {
        }

        public DbSet<Document> Documents { get; set; }
        public DbSet<Suggestions> Suggestions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();

        }
    }
}