namespace PharmaStock.Core.DTO.InventoryBalance
{
    public class InventoryBalanceDTO
    {
        public int InventoryBalanceId { get; set; }
        public int LocationId { get; set; }
        public string? LocationName { get; set; }
        public int BinId { get; set; }
        public string? BinCode { get; set; }
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public int InventoryLotId { get; set; }
        public string? BatchNumber { get; set; }
        public DateOnly? ExpiryDate { get; set; }
        public int QuantityOnHand { get; set; }
        public int ReservedQty { get; set; }
        public int AvailableQty => QuantityOnHand - ReservedQty;
    }
}
