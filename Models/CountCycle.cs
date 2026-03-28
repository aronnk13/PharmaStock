using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class CountCycle
{
    public int CountCycleId { get; set; }

    public string Cycle { get; set; } = null!;

    public virtual ICollection<StockCount> StockCounts { get; set; } = new List<StockCount>();
}
