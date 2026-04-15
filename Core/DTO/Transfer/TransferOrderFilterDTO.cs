namespace PharmaStock.Core.DTO.Transfer
{
    public class TransferOrderFilterDTO
    {
        public int? Status { get; set; }
        public int? LocationId { get; set; }  // matches on FromLocationId OR ToLocationId
    }
}
