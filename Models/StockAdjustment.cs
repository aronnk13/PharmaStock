using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class StockAdjustment
{
    public string AdjustmentId { get; set; } = null!;

    public string? LocationId { get; set; }

    public string? ItemId { get; set; }

    public string? LotId { get; set; }

    public int QuantityDelta { get; set; }

    public string? Reason { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime PostedDate { get; set; }

    public virtual User? ApprovedByNavigation { get; set; }

    public virtual Item? Item { get; set; }

    public virtual Location? Location { get; set; }

    public virtual InventoryLot? Lot { get; set; }
}
