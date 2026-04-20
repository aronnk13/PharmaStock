namespace PharmaStock.Core.DTO.QCO
{
    public class QuarantineActionDTO
    {
        public int QuarantaineActionId { get; set; }
        public int InventoryLotId { get; set; }
        public int? BatchNumber { get; set; }
        public string? ItemName { get; set; }
        public DateTime QuarantineDate { get; set; }
        public string Reason { get; set; } = null!;
        public DateTime? ReleasedDate { get; set; }
        public int Status { get; set; }
        public string? StatusName { get; set; }
    }

    public class CreateQuarantineActionDTO
    {
        public int InventoryLotId { get; set; }
        public string Reason { get; set; } = null!;
    }

    public class ReleaseQuarantineDTO
    {
        public DateTime ReleasedDate { get; set; } = DateTime.UtcNow;
    }
}
