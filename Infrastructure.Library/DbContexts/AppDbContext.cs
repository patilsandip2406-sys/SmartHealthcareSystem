using Microsoft.EntityFrameworkCore;
using Shared.Library.Models;

namespace Infrastructure.Library.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
        public DbSet<User> Users => Set<User>();
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Claim> Claims => Set<Claim>();

    }
}
