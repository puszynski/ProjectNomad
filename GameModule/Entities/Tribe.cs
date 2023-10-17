using GameModule.Entities.SharedInterfaces;
using GameModule.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using ProjectNomad.Shared.Interfaces;
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
        public Localization Localization { get; set; }
        public Resources Resources { get; set; }

        public TribeRelocation? TribeRelocation { get; set; }
        public ICollection<Human>? Humans { get; set; } = new List<Human>();
        public ICollection<HumanTask>? HumanTasks { get; set; } = new List<HumanTask>();
        public ICollection<HumanTaskOrder>? HumanTaskOrders { get; set; } = new List<HumanTaskOrder>();
        public ICollection<TribeStructure>? TribeStructures { get; set; } = new List<TribeStructure>();
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
