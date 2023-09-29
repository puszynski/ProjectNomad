using ProjectNomad.Client.Logic;
using ProjectNomad.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ProjectNomad.Client.ViewModels.CreateAccount
{
    public class AccountCreateViewModel : IAccount
    {
        [Required]
        [MinLength(4)]
        public string? Name { get; set; }

        [Required]
        [MinLength(4)]
        public string? Password { get; set; }

        [Required]
        [MinLength(4)]        
        public string? PasswordConfirmation { get; set; }

        readonly AccountManager _accountManager;
        public AccountCreateViewModel(AccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        public async Task Register()
        {
            if (!Password.Equals(PasswordConfirmation))
                throw new Exception();//todo

            IAccount sharedModel = this;
            _accountManager.LogIn(sharedModel);
        }
    }
}
