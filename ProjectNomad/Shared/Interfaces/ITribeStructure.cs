using ProjectNomad.Shared.Enums;

namespace ProjectNomad.Shared.Interfaces
{
    public interface ITribeStructure
    {
        public int Id { get; }
        public ETribeStructureType Type { get; }
        public int PowerAndDurability { get; }
    }
}
