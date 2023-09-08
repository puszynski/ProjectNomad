using GameModule.Entities.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ProjectNomad.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using ProjectNomad.Shared.Interfaces.Properties;

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
        public Localization Localization { get; set; }
        public Resources Resources { get; set; }

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
