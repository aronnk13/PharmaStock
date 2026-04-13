using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class Reason
{
    public int ReasonId { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<StockAdjustment> StockAdjustments { get; set; } = new List<StockAdjustment>();

    public virtual ICollection<StockCountItem> StockCountItems { get; set; } = new List<StockCountItem>();
}
