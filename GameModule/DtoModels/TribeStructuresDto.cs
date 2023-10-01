using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.DtoModels
{
    public record TribeStructuresDto(int Id, ETribeStructureType Type, int PowerAndDurability) : ITribeStructure;
}
