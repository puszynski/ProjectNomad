using Microsoft.EntityFrameworkCore;

namespace GameModule.Entities.ValueObjects
{
    [Owned]
    internal class Localization
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
