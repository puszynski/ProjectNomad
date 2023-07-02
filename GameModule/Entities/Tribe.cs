using GameModule.Entities.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GameModule.Entities
{
    internal class Tribe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public Guid AccountId { get; set; }//todo migration
        public DateTime Updated { get; set; }
        public string Name { get; set; }//todo migration

        //todo LastRecalculationDate / UpdatedDate - to get time to recalculate data

        public Localization Localization { get; set; }

        ICollection<HumanUnit> HumanUnits { get; set; }
    }
}
