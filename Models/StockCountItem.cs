using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class StockCountItem
{
    public int StockCountItemId { get; set; }

    public int CountId { get; set; }

    public int BinId { get; set; }

    public int ItemId { get; set; }

    public int InventoryLotId { get; set; }

    public int SystemQty { get; set; }

    public int CountedQty { get; set; }

    public int Variance { get; set; }

    public int ReasonCode { get; set; }

    public virtual Bin Bin { get; set; } = null!;

    public virtual StockCount Count { get; set; } = null!;

    public virtual InventoryLot InventoryLot { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;

    public virtual Reason ReasonCodeNavigation { get; set; } = null!;
}
