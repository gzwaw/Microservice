using Microsoft.EntityFrameworkCore;
using MilleniumTest.Model;

namespace MilleniumTest.Data
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
    }
}