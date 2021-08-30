using Microsoft.EntityFrameworkCore;



namespace Searchify.Domain.Models
{

    /// <summary>
    /// Database context for the searchify api
    /// </summary>
    public class SearchifyContext : DbContext, ISearchifyContext
    {
        public SearchifyContext(DbContextOptions<SearchifyContext> options) : base(options)
        {
        }


        /// <summary>
        /// Documents set
        /// </summary>
        public DbSet<Document> Documents { get; set; }
        
        /// <summary>
        /// Suggestions set
        /// </summary>
        public DbSet<Suggestions> Suggestions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();

        }
    }
}