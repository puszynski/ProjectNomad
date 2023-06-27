using AccountModule.Configuration;
using AccountModule.Logic.AccountRegistration;

namespace AccountModule
{
    internal class AccountModule : IAccountModule
    {
        private readonly AccountModuleDbContext _dbContext;
        public AccountModule(AccountModuleDbContext dbContext) 
            => _dbContext = dbContext;

        public async Task<Guid> Register(string? accountName,
            string? password,
            bool isGuest = false)
        {
            var registerFactory = AccountRegisterFactory.GetAccountRegistration(isGuest,
                accountName,
                password,
                _dbContext);
            return await registerFactory.Register();
        }

        public async Task<Guid> LogIn(string? accountName,
            string? password,
            string? guestToken)
        {
            throw new NotImplementedException();
        }
    }
}