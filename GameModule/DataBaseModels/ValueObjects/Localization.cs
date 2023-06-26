using Microsoft.EntityFrameworkCore;

namespace GameModule.DataBaseModels.ValueObjects
{
    [Owned]
    internal class Localization
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
