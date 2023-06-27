using Microsoft.EntityFrameworkCore;

namespace AccountModule.Entities.ValueObject
{
    [Owned]
    internal class GuestAccount
    {
        public string ReLoginToken { get; set; }
        public DateTime LastLoginDate { get; set; }
    }
}
