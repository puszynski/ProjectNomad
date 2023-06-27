using AccountModule.Configuration;

namespace AccountModule.Logic.AccountRegistration
{
    internal static class AccountRegisterFactory
    {
        internal static IAccountRegistration GetAccountRegistration(bool isGuest,
            string? name,
            string? password,
            AccountModuleDbContext _dbContext)
        {
            switch (isGuest)
            {
                case true: return new GuestAccountRegistration(_dbContext);

                case false: 
                    return new AccountRegistration(_dbContext, 
                        name ?? throw new ArgumentException(nameof(name), "Name not provided :/"), 
                        password ?? throw new ArgumentException(nameof(password), "Password not provided :/"));
            }
        }
    }
}
