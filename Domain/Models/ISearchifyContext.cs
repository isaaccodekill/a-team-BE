using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Searchify.Domain.Models
{
    public interface ISearchifyContext
    {
        DbSet<Document> Documents { get; set; }
    }
}