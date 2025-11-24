using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ServiceDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options) { }
    }
}