using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;

namespace ProjectNomad.Client.Models.Response
{
    public record TribeStructure(int Id, ETribeStructureType Type, int PowerAndDurability) : ITribeStructure;
}
