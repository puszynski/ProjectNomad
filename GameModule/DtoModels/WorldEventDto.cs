using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.DtoModels
{
    internal record WorldEventDto(EWorldEventType Type) : IWorldEvent;
}
