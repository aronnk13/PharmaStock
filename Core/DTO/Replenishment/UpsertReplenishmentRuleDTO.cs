namespace PharmaStock.Core.DTO.Replenishment
{
    public class UpsertReplenishmentRuleDTO
    {
        public int RuleId { get; set; }
        public int LocationId { get; set; }
        public int ItemId { get; set; }
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public int ParLevel { get; set; }
        public bool ReviewCycle {get;set;}
        public bool IsCreate {get;set;}
    }
}