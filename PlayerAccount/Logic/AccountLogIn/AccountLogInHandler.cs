using AccountModule.Configuration;
using Microsoft.EntityFrameworkCore;

namespace AccountModule.Logic.AccountLogIn
{
    internal class AccountLogInHandler : IAccountLogInHandler
    {
        private readonly AccountModuleDbContext _dbContext;
        private readonly string _name;
        private readonly string _password;


        public AccountLogInHandler(AccountModuleDbContext dbContext,
            string name,
            string password)
        {
            _dbContext = dbContext;
            _name = name;
            _password = password;
        }

        async Task<Guid?> IAccountLogInHandler.LogIn()
        {
            var account = await _dbContext.Accounts.SingleOrDefaultAsync(x => 
                x.RegisteredAccount != null 
                && x.RegisteredAccount.Name == _name 
                && x.RegisteredAccount.Password == _password);

            if (account == null)
                return null;

            return account.Id;
        }
    }
}
