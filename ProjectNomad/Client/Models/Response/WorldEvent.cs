using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.Models.Response
{
    public record WorldEvent(EWorldEventType Type) : IWorldEvent;
}
