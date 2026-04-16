using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class ReplenishmentRequest
{
    public int ReplenishmentRequestId { get; set; }

    public int LocationId { get; set; }

    public int ItemId { get; set; }

    public int SuggestedQty { get; set; }

    public DateTime CreatedDate { get; set; }

    public int RuleId { get; set; }

    public int Status { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Location Location { get; set; } = null!;

    public virtual ReplenishmentRule Rule { get; set; } = null!;

    public virtual ReplenishmentStatus StatusNavigation { get; set; } = null!;
}
