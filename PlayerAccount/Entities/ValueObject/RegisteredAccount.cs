using Microsoft.EntityFrameworkCore;

namespace AccountModule.Entities.ValueObject
{
    [Owned]
    internal class RegisteredAccount
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
