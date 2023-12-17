using ProjectNomad.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjectNomad.Shared.DTOs.WasmToServer
{
    public class JobAdd
    {
        [Required]
        public int HumanId { get; set; }

        [Required]
        public ETaskType Type { get; set; }

        [Required]
        public int PriorityPercentage { get; set; }
    }
}
