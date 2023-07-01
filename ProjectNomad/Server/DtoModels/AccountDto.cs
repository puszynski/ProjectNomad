using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Server.DtoModels
{
    public class AccountDto : IAccount
    {
        public string Name { get; set; }

        public string Password { get; set; }
    }

    public record AccountRecord(string? Name, string? Password) : IAccount;
}
