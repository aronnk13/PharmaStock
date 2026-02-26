using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class StockCountItem
{
    public string CountItemId { get; set; } = null!;

    public string? CountId { get; set; }

    public string? BinId { get; set; }

    public string? ItemId { get; set; }

    public string? LotId { get; set; }

    public int SystemQty { get; set; }

    public int CountedQty { get; set; }

    public int Variance { get; set; }

    public string? ReasonCode { get; set; }

    public virtual Bin? Bin { get; set; }

    public virtual StockCount? Count { get; set; }

    public virtual Item? Item { get; set; }

    public virtual InventoryLot? Lot { get; set; }
}
