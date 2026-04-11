using System.ComponentModel.DataAnnotations;

namespace PharmaStock.Core.DTO.GRNItem
{
    public class DeleteGRNItemDTO
    {
        [Required]
        [MaxLength(250)]
        public string Reason { get; set; } = null!;
    }
}
