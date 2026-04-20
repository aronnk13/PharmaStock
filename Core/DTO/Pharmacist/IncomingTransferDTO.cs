namespace PharmaStock.Core.DTO.Pharmacist
{
    public class IncomingTransferDTO
    {
        public int TransferOrderId { get; set; }
        public int FromLocationId { get; set; }
        public string? FromLocationName { get; set; }
        public int ToLocationId { get; set; }
        public string? ToLocationName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
        public string? StatusName { get; set; }
        public List<TransferItemDTO> Items { get; set; } = new();
    }

    public class TransferItemDTO
    {
        public int TransferItemId { get; set; }
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public int InventoryLotId { get; set; }
        public int? BatchNumber { get; set; }
        public int Quantity { get; set; }
    }
}
