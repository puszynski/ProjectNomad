using Microsoft.EntityFrameworkCore;

namespace AccountModule.DataBaseModels.ValueObject
{
    [Owned]
    internal class GuestAccount
    {
        public string Name { get; set; }
        public string ReLoginToken { get; set; }

    }
}
