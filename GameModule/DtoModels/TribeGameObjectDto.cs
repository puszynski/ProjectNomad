using ProjectNomad.Shared.Interfaces;

namespace GameModule.DtoModels
{
    internal record TribeGameObjectDto(ITribe Tribe, 
        IEnumerable<IHumanUnit> HumanUnits) : ITribeGameObjects;
}
