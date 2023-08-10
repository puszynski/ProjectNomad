using ProjectNomad.Shared.Interfaces;

namespace AccountModule.DtoModels
{
    public class AccountDto : IAccount
    {
        public string Name { get; set; }

        public string Password { get; set; }
    }
}
