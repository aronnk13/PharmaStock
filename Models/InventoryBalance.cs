using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class InventoryBalance
{
    public int InventoryBalanceId { get; set; }

    public int LocationId { get; set; }

    public int BinId { get; set; }

    public int ItemId { get; set; }

    public int InventoryLotId { get; set; }

    public int QuantityOnHand { get; set; }

    public int ReservedQty { get; set; }

    public virtual Bin Bin { get; set; } = null!;

    public virtual InventoryLot InventoryLot { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;

    public virtual Location Location { get; set; } = null!;
}
