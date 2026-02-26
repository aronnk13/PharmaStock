using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class TransferItem
{
    public string ToitemId { get; set; } = null!;

    public string? Toid { get; set; }

    public string? ItemId { get; set; }

    public string? LotId { get; set; }

    public int Quantity { get; set; }

    public virtual Item? Item { get; set; }

    public virtual InventoryLot? Lot { get; set; }

    public virtual TransferOrder? To { get; set; }
}
