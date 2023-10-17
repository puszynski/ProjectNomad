using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.DtoModels
{
    internal record HumanUnitTaskOrderDto(int Id, 
        int TribeId, 
        DateTime Added, 
        ETaskType Type, 
        int? HumanTaskId,
        int? X,
        int? Y) : IHumanUnitTaskOrder;
}
