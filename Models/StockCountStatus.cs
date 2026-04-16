using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class StockCountStatus
{
    public int StockCountStatusId { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<StockCount> StockCounts { get; set; } = new List<StockCount>();
}
