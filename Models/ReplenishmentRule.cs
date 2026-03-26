using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class ReplenishmentRule
{
    public int ReplenishmentRuleId { get; set; }

    public int LocationId { get; set; }

    public int ItemId { get; set; }

    public int MinLevel { get; set; }

    public int MaxLevel { get; set; }

    public int ParLevel { get; set; }

    public bool ReviewCycle { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Location Location { get; set; } = null!;

    public virtual ICollection<ReplenishmentRequest> ReplenishmentRequests { get; set; } = new List<ReplenishmentRequest>();
}
