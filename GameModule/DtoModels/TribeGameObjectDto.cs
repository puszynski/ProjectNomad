using ProjectNomad.Shared.Interfaces;

namespace GameModule.DtoModels
{
    internal record TribeGameObjectDto(ITribe Tribe, 
        IEnumerable<IHuman> HumanUnits,
        IWorldParameters WorldParameters) : ITribeGameObjects;
}
