using System.ComponentModel.DataAnnotations;

namespace PharmaStock.Core.DTO.Bin
{
    public class CreateBinDTO
    {
        [Required]
        public int LocationId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = null!;

        [Required]
        public int BinStorageClassId { get; set; }

        [Required]
        public bool IsQuarantine { get; set; }

        public int MaxCapacity { get; set; }
    }
}
