using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class StockTransaction
{
    public string TxnId { get; set; } = null!;

    public string? LocationId { get; set; }

    public string? BinId { get; set; }

    public string? ItemId { get; set; }

    public string? LotId { get; set; }

    public string TxnType { get; set; } = null!;

    public int Quantity { get; set; }

    public DateTime TxnDate { get; set; }

    public string? ReferenceId { get; set; }

    public string? Notes { get; set; }

    public virtual Bin? Bin { get; set; }

    public virtual Item? Item { get; set; }

    public virtual Location? Location { get; set; }

    public virtual InventoryLot? Lot { get; set; }
}
