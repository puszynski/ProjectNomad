using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using AccountModule.DataBaseModels;

namespace AccountModule.Configuration
{
    internal class AccountContext : DbContext
    {
        public AccountContext() : base("NomadDatabaseContext")
        {
            
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<RegisteredAccount> RegisteredAccounts { get; set; }
        public DbSet<GuestAccount> GuestAccounts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }

    internal class AccountContextInitializer : DropCreateDatabaseIfModelChanges<AccountContext>
    {
        protected override void Seed(AccountContext context)
        {
            var accounts = new List<Account>()
            {
                new Account { Id = 1 },
                new Account { Id = 2 },
                new Account { Id = 3 }
            };

            var guests = new List<GuestAccount>()
            { 
                new GuestAccount { AccountId = 2, Name = "guest123", ReLoginToken = "123" },
                new GuestAccount { AccountId = 3, Name = "guest234", ReLoginToken = "123" }
            };

            var registeredAccounts = new List<RegisteredAccount>() 
            { 
                new RegisteredAccount { AccountId = 1, Name = "puch", Password = "123" } 
            };

            accounts.ForEach(x => context.Accounts.Add(x));
            context.SaveChanges();

            guests.ForEach(x => context.GuestAccounts.Add(x));
            context.SaveChanges();

            registeredAccounts.ForEach(x => context.RegisteredAccounts.Add(x));
            context.SaveChanges();
        }
    }

}
