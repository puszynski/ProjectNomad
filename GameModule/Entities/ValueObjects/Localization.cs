using Microsoft.EntityFrameworkCore;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Entities.ValueObjects
{
    [Owned]
    internal class Localization : IValueObject
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
