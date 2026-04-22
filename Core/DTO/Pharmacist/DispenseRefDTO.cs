namespace PharmaStock.Core.DTO.Pharmacist
{
    public class DispenseRefDTO
    {
        public int DispenseRefId { get; set; }
        public int LocationId { get; set; }
        public string? LocationName { get; set; }
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public int InventoryLotId { get; set; }
        public int? BatchNumber { get; set; }
        public int Quantity { get; set; }
        public DateTime DispenseDate { get; set; }
        public bool Status { get; set; }
        public int Destination { get; set; }
        public string? DestinationName { get; set; }
    }

    public class CreateDispenseRefDTO
    {
        public int LocationId { get; set; }
        public int ItemId { get; set; }
        public int InventoryLotId { get; set; }
        public int Quantity { get; set; }
        public int Destination { get; set; }
    }
}
