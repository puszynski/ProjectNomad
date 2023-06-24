using Microsoft.EntityFrameworkCore;

namespace AccountModule.DataBaseModels
{
    [Owned]
    internal class GuestAccount
    {
        public string Name { get; set; }
        public string ReLoginToken { get; set; }

    }
}
