using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class ReplenishmentRequest
{
    public string ReqId { get; set; } = null!;

    public string? LocationId { get; set; }

    public string? ItemId { get; set; }

    public int SuggestedQty { get; set; }

    public DateTime CreatedDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Item? Item { get; set; }

    public virtual Location? Location { get; set; }
}
