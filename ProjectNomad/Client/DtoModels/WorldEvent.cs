using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.DtoModels
{
    public record WorldEvent(EWorldEventType Type) : IWorldEvent;
}
