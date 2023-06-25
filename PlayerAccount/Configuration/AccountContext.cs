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
        //public DbSet<RegisteredAccount> RegisteredAccounts { get; set; }
        //public DbSet<GuestAccount> GuestAccounts { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
            // note:    concept of Owned Entity Types, which can be used to implement DDD value types.
            //          This would store Access objects in the same database table as AccessLevel objects,
            //          and therefore requires no primary key.
            // https://stackoverflow.com/questions/60600835/the-entity-type-access-requires-a-primary-key-to-be-defined-if-you-intended-t
            //modelBuilder.Entity<Account>().OwnsOne(x => x.RegisteredAccount);
            //modelBuilder.Entity<Account>().OwnsOne(x => x.GuestAccount);


            //modelBuilder.Entity<Account>().HasData(
            //        new Account { Id = 1, GuestAccount = new GuestAccount { Name = "guest123", ReLoginToken = "123" }, RegisteredAccount = null },
            //        new Account { Id = 2, GuestAccount = new GuestAccount { Name = "guest234", ReLoginToken = "123" }, RegisteredAccount = null },
            //        new Account { Id = 3, GuestAccount = null, RegisteredAccount = new RegisteredAccount { Name = "puch", Password = "123" } }
            //    );

            //modelBuilder.Entity<GuestAccount>().HasData(
            //        new GuestAccount { Name = "guest123", ReLoginToken = "123" },
            //        new GuestAccount { Name = "guest234", ReLoginToken = "123" }
            //    );

            //modelBuilder.Entity<RegisteredAccount>().HasData(
            //        new RegisteredAccount { Name = "puch", Password = "123" }
            //    );
        //}
    }
}
