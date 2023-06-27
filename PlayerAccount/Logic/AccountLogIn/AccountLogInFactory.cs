using AccountModule.Configuration;

namespace AccountModule.Logic.AccountLogIn
{
    internal static class AccountLogInFactory
    {
        internal static IAccountLogInHandler GetAccountLogInHandler(bool isGuest,
            string? name,
            string? password,
            string? reLoginToken,
            AccountModuleDbContext _dbContext)
        {
            switch (isGuest)
            {
                case true: return new GuestAccountLogInHandler(_dbContext, 
                    reLoginToken ?? throw new ArgumentException(nameof(name), "ReLoginToken not provided :/"));

                case false:
                    return new AccountLogInHandler(_dbContext,
                        name ?? throw new ArgumentException(nameof(name), "Name not provided :/"),
                        password ?? throw new ArgumentException(nameof(password), "Password not provided :/"));
            }
        }
    }
}
