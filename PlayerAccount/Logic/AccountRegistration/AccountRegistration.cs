using AccountModule.Configuration;
using AccountModule.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountModule.Logic.AccountRegistration
{
    internal class AccountRegistration : IAccountRegistration
    {
        private readonly AccountModuleDbContext _dbContext;
        private readonly string _name;
        private readonly string _password;

        public AccountRegistration(AccountModuleDbContext dbContext, string name, string password)
        {
            _dbContext = dbContext;
            _name = name;
            _password = password;
        }

        async Task<Guid> IAccountRegistration.Register()
        {
            CanCreate();

            var account = new Account()
            {
                RegisteredAccount = new Entities.ValueObject.RegisteredAccount { Name = _name, Password = _password }
            };
            await _dbContext.AddAsync(account);
            await _dbContext.SaveChangesAsync();

            return (await _dbContext.Accounts.SingleAsync(x => x.RegisteredAccount != null && x.RegisteredAccount.Name == _name)).Id;

            void CanCreate() //TODO THINK DDD - VALIDATION IN ENTITY?
            {
                if (_dbContext.Accounts.Where(x => x.RegisteredAccount != null && x.RegisteredAccount.Name.Equals(_name)).Any())
                    throw new ArgumentException(nameof(_name), "Account with provided name already exists :/");
            }
        }
    }
}
