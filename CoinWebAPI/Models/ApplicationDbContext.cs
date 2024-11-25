using CoinWebAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoinWebAPI.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Currency> Currencies { get; set; }
    }
}
