using GameModule.Entities.ValueObjects;
using ProjectNomad.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ProjectNomad.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

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
