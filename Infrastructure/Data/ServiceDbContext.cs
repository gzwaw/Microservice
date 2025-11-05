using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
    }
}