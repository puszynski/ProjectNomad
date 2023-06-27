using AccountModule.Configuration;

namespace AccountModule.Logic.AccountRegistration
{
    internal class GuestAccountRegistration : IAccountRegistration
    {
        private readonly AccountModuleDbContext _dbContext;
        public GuestAccountRegistration(AccountModuleDbContext dbContext) 
            => _dbContext = dbContext;

        async Task<Guid> IAccountRegistration.Register()
        {
            throw new NotImplementedException();
        }
    }
}
