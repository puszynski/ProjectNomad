using Microsoft.EntityFrameworkCore;

namespace AccountModule.DataBaseModels
{
    [Owned]
    internal class RegisteredAccount
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
