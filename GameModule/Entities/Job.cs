using GameModule.Entities.SharedInterfaces;
using ProjectNomad.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace GameModule.Entities
{
    internal class Job : IId, IHumanReference
    {
        public int Id { get; set; }

        public int HumanId { get; set; }
        public Human Human { get; set; }

        public ETaskType Type { get; set; }

        [Range(1,100)]
        public int PriorityPercentage { get; set; }
    }
}
