using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class InventoryBalance
{
    public string BalanceId { get; set; } = null!;

    public string? LocationId { get; set; }

    public string? BinId { get; set; }

    public string? ItemId { get; set; }

    public string? LotId { get; set; }

    public int QuantityOnHand { get; set; }

    public int ReservedQty { get; set; }

    public virtual Bin? Bin { get; set; }

    public virtual Item? Item { get; set; }

    public virtual Location? Location { get; set; }

    public virtual InventoryLot? Lot { get; set; }
}
