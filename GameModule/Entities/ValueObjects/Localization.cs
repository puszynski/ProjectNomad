using Microsoft.EntityFrameworkCore;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Entities.ValueObjects
{
    [Owned]
    internal class Localization : IValueObject //todo record? becouse IT SHOULD BE IMMUTABLE - ONCE CREATED, CAN NOT BE CHANGED => https://code-maze.com/csharp-value-objects/
    {
        public int X { get; set; }
        public int Y { get; set; }

        //https://stackoverflow.com/questions/59503354/c-sharp-how-can-i-compare-two-lists-of-objects-in-the-equals-method
        public override bool Equals(object obj)
        {
            return X == ((Localization)obj)?.X &&
                   Y == ((Localization)obj)?.Y;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                return hashCode;
            }
        }
    }
}
