namespace PharmaStock.Core.DTO.Replenishment
{
    public class UpsertReplenishmentRequestDTO
    {
        public int RequestId { get; set; }
        public int LocationId { get; set; }
        public int LocationType { get; set; }
        public int ItemId { get; set; }
        public int RuleId { get; set; }
        public int StatusId { get; set; }
        public int SuggestedQuantity { get; set; }
        public bool IsCreate {get;set;}
    }
}