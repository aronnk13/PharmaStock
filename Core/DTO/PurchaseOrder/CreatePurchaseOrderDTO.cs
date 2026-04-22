using System.ComponentModel.DataAnnotations;

namespace PharmaStock.Core.DTO.PurchaseOrder
{
    public class CreatePurchaseOrderDTO
    {
        [Required]
        public int VendorId { get; set; }

        [Required]
        public int LocationId { get; set; }

        [Required]
        public DateOnly OrderDate { get; set; }

        [Required]
        public DateOnly ExpectedDate { get; set; }
    }
}
