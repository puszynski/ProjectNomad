using System.ComponentModel.DataAnnotations;

namespace ProjectNomad.Shared.DTOs.WasmToServer
{
    public class JobUpdate
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Range(1,100)]
        public int PriorityPercentage { get; set; }
    }
}
