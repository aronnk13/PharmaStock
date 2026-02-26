using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class DispenseRef
{
    public string DispenseId { get; set; } = null!;

    public string? LocationId { get; set; }

    public string? ItemId { get; set; }

    public string? LotId { get; set; }

    public int Quantity { get; set; }

    public DateTime DispenseDate { get; set; }

    public string? Destination { get; set; }

    public string Status { get; set; } = null!;

    public virtual Item? Item { get; set; }

    public virtual Location? Location { get; set; }

    public virtual InventoryLot? Lot { get; set; }
}
