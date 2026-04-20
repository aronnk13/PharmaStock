using System.ComponentModel.DataAnnotations;

namespace PharmaStock.Core.DTO.Item
{
    public class ItemDTO
    {
        [Required]
        public int DrugId { get; set; }

        public int? PackSize { get; set; }

        [Required]
        public int UoMId { get; set; }

        [Required]
        public decimal ConversionToEach { get; set; }

        public string? Barcode { get; set; }

        public bool Status { get; set; } = true;
    }
}
