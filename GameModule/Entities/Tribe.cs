using GameModule.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using ProjectNomad.Shared.Interfaces;
using ProjectNomad.Shared.Interfaces.Properties;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public Localization Localization { get; set; } //MUSICZ BYĆ SPÓJNY - ALBO WSZĘDZIE LOCALIZATION - ALOB MAP-TILE-ID
        public Resources Resources { get; set; }

        public TribeRelocation? TribeRelocation { get; set; }
        public ICollection<HumanUnit> HumanUnits { get; set; }
        public ICollection<HumanUnitTask> HumanUnitTasks { get; set; }
        public ICollection<HumanUnitTaskOrder> HumanUnitTaskOrders { get; set; }
    }

    [Owned]
    internal class Resources : IValueObject
    {
        [DefaultValue(0)]
        public int FreshFood { get; set; }

        [DefaultValue(0)]
        public int Wood { get; set; }
    }
}
