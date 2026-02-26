using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class ReplenishmentRule
{
    public string RuleId { get; set; } = null!;

    public string? LocationId { get; set; }

    public string? ItemId { get; set; }

    public int MinLevel { get; set; }

    public int MaxLevel { get; set; }

    public int ParLevel { get; set; }

    public string? ReviewCycle { get; set; }

    public virtual Item? Item { get; set; }

    public virtual Location? Location { get; set; }
}
