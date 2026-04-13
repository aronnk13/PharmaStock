using System.ComponentModel.DataAnnotations;

namespace PharmaStock.Core.DTO.GRNItem
{
    public class CreateGRNItemDTO
    {
        [Required]
        public int GoodsReceiptId { get; set; }

        [Required]
        public int PurchaseOrderItemId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public int BatchNumber { get; set; }

        [Required]
        public DateOnly ExpiryDate { get; set; }

        [Required]
        public int ReceivedQty { get; set; }

        [Required]
        public int AcceptedQty { get; set; }

        [Required]
        public int RejectedQty { get; set; }

        [MaxLength(250)]
        public string? Reason { get; set; }
    }
}
