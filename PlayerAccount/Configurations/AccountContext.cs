using AccountModule.DataBaseModels;
using Microsoft.EntityFrameworkCore;

namespace AccountModule.Configuration
{
    internal class AccountContext : DbContext
    {
        public AccountContext(DbContextOptions<AccountContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
    }
}
