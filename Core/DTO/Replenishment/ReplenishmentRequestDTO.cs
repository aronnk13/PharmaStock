namespace PharmaStock.Core.DTO.Replenishment
{
    public class ReplenishmentRequestDTO
    {
        public int ReplenishmentRequestId { get; set; }
        public int LocationId { get; set; }
        public string? LocationName { get; set; }
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public int SuggestedQty { get; set; }
        public DateTime CreatedDate { get; set; }
        public int RuleId { get; set; }
        public int Status { get; set; }
        public string? StatusName { get; set; }
    }

    public class CreateReplenishmentRequestDTO
    {
        public int LocationId { get; set; }
        public int ItemId { get; set; }
        public int SuggestedQty { get; set; }
        public int RuleId { get; set; }
    }

    public class ReplenishmentRuleDTO
    {
        public int ReplenishmentRuleId { get; set; }
        public int LocationId { get; set; }
        public string? LocationName { get; set; }
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public int ParLevel { get; set; }
        public bool ReviewCycle { get; set; }
    }
}
