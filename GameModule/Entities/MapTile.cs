using GameModule.Entities.SharedInterfaces;
using GameModule.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameModule.Entities
{
    internal class MapTile : IId
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Localization Localization { get; set; }
        public EMapType Type { get; set; } //idea? change to enum flags - allow mix e.g. forest + mountains
        public TileRecourse Food { get; set; }
        public TileRecourse Wood { get; set; }
    }

    [Owned]
    internal class TileRecourse : IValueObject
    {
        public int ActualPoints { get; set; }
        public int MaxLimitPoints { get; set; }
    }
}
