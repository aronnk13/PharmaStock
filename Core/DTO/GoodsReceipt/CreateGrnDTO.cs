using System.ComponentModel.DataAnnotations;

namespace PharmaStock.Core.DTO.GoodsReceipt
{
    public class CreateGrnDTO
    {
        [Required]
        public int PurchaseOrderId { get; set; }

        [Required]
        public DateTime ReceivedDate { get; set; }
    }
}
