using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.DtoModels
{
    internal record HumanTaskOrderDto(int Id, 
        int TribeId, 
        DateTime Added, 
        ETaskType Type,
        int? X,
        int? Y) : IHumanUnitTaskOrder;
}
