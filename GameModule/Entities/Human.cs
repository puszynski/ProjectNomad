using GameModule.Entities.SharedInterfaces;
using GameModule.Entities.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace GameModule.Entities
{
    internal class Human : IId, ITribeReference
    {
        public int Id { get; set; }

        public int TribeId { get; set; }
        public Tribe Tribe { get; set; }
        public string Name { get; set; }
        public Localization Localization { get; set; }

        [Range(0, 100)]
        public int FoodLevelPercentage { get; set; }

        [Range(0, 100)]
        //0 - death from freezing
        //50 - optimal
        //100 - death from overheating
        public int ThermalLevelPercentage { get; set; }

        public HumanTask? HumanTask { get; set; } //todo 1:1 relation
        public ICollection<Job> Jobs { get; set; }
    }
}