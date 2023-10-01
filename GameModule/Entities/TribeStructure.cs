using GameModule.Entities.SharedInterfaces;
using ProjectNomad.Shared.Enums;

namespace GameModule.Entities
{
    internal class TribeStructure : IId, ITribeReference
    {
        public int Id { get; set; }

        public int TribeId { get; set; }
        public Tribe Tribe { get ; set; }

        public ETribeStructureType Type { get; set; }

        //CO Z WYGASŁYM OGNIESKIEM, DURABILITY == 1 - OK?
        //[Points] //todo - walidate from 1 to many max-int, in points - higher lvl strusture can have higher MaxPoints
        public int PowerAndDurability { get; set; }


        //public ICollection<HumanUnit> HumanUnits { get; set; }
        //public int Level { get; set; }
    }

    
}
