using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class StockCount
{
    public int StockCountId { get; set; }

    public int LocationId { get; set; }

    public int Cycle { get; set; }

    public DateOnly ScheduledDate { get; set; }

    public int Status { get; set; }

    public virtual CountCycle CycleNavigation { get; set; } = null!;

    public virtual Location Location { get; set; } = null!;

    public virtual StockCountStatus StatusNavigation { get; set; } = null!;

    public virtual ICollection<StockCountItem> StockCountItems { get; set; } = new List<StockCountItem>();
}
