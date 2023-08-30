using ProjectNomad.Shared.Enums;

namespace ProjectNomad.Shared.Interfaces
{
    //duration in rounded minutes e.g. 10.00 - 10.10, 10.10 - 10.40, 12.10 - 15.00
    public interface IWorldEvent
    {
        public EWorldEventType Type { get; }
    }
}
