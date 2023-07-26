using GameModule.Entities.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ProjectNomad.Shared.Interfaces;

namespace GameModule.Entities
{
    internal class Tribe : IId
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public Guid AccountId { get; set; }
        public DateTime Updated { get; set; }
        public string Name { get; set; }

        //todo LastRecalculationDate / UpdatedDate - to get time to recalculate data

        public Localization Localization { get; set; }

        ICollection<HumanUnit> HumanUnits { get; set; }
    }
}
