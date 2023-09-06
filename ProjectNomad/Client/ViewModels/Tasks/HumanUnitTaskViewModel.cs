using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.ViewModels.Tasks
{
    public record HumanUnitTaskViewModel(int TribeId, 
        int HumanUnitId,
        string HumanUnitName,
        EHumanUnitTaskType Type, 
        DateTime From, 
        DateTime To) : IHumanUnitTaskDto;
}
