using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class StockCount
{
    public string CountId { get; set; } = null!;

    public string? LocationId { get; set; }

    public string? Cycle { get; set; }

    public DateTime ScheduledDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Location? Location { get; set; }

    public virtual ICollection<StockCountItem> StockCountItems { get; set; } = new List<StockCountItem>();
}
