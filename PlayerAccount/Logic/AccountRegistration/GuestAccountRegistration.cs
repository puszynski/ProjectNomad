using AccountModule.Configuration;
using AccountModule.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountModule.Logic.AccountRegistration
{
    internal class GuestAccountRegistration : IAccountRegistration
    {
        private readonly AccountModuleDbContext _dbContext;
        public GuestAccountRegistration(AccountModuleDbContext dbContext) 
            => _dbContext = dbContext;

        async Task<Guid> IAccountRegistration.Register()
        {
            var reLoginToken = Guid.NewGuid().ToString();
            var account = new Account()
            {
                GuestAccount = new Entities.ValueObject.GuestAccount
                {
                    LastLoginDate = DateTime.UtcNow,
                    ReLoginToken = reLoginToken
                }
            };
            await _dbContext.AddAsync(account);
            await _dbContext.SaveChangesAsync();

            return (await _dbContext.Accounts.SingleAsync(x => x.GuestAccount != null && x.GuestAccount.ReLoginToken == reLoginToken)).Id;
        }
    }
}
