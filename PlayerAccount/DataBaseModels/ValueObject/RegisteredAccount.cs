using Microsoft.EntityFrameworkCore;

namespace AccountModule.DataBaseModels.ValueObject
{
    [Owned]
    internal class RegisteredAccount
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
