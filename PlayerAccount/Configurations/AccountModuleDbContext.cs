using AccountModule.DataBaseModels;
using Microsoft.EntityFrameworkCore;

namespace AccountModule.Configuration
{
    internal class AccountModuleDbContext : DbContext
    {
        public AccountModuleDbContext(DbContextOptions<AccountModuleDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("AccountModule");
        }

        public DbSet<Account> Accounts { get; set; }
    }
}
