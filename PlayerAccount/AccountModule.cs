using AccountModule.Configuration;
using AccountModule.Logic.AccountLogIn;
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
            //todo validation - accountName must be unique
            var AccountRegistration = AccountRegisterFactory.GetAccountRegistration(isGuest,
                accountName,
                password,
                _dbContext);

            return await AccountRegistration.Register();
        }

        public async Task<Guid?> LogIn(string? accountName,
            string? password,
            string? guestToken)
        {
            var accountLogInHandler = AccountLogInFactory.GetAccountLogInHandler(IsGuest(),
                accountName,
                password,
                guestToken,
                _dbContext);

            return await accountLogInHandler.LogIn();

            bool IsGuest()
                => !string.IsNullOrEmpty(guestToken);
        }
    }
}