using AccountModule.Configuration;
using Microsoft.EntityFrameworkCore;

namespace AccountModule.Logic.AccountLogIn
{
    internal class GuestAccountLogInHandler : IAccountLogInHandler
    {
        private readonly AccountModuleDbContext _dbContext;
        private readonly string _reLoginToken;

        public GuestAccountLogInHandler(AccountModuleDbContext dbContext,
            string reLoginToken)
        {
            _dbContext = dbContext;
            _reLoginToken = reLoginToken;
        }

        async Task<Guid?> IAccountLogInHandler.LogIn()
        {
            var guestAccount = await _dbContext.Accounts
                .SingleOrDefaultAsync(x => x.GuestAccount != null && x.GuestAccount.ReLoginToken == _reLoginToken);

            if (guestAccount == null)
                return null;

            return guestAccount.Id;
        }
    }
}