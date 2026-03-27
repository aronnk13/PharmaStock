using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class StockAdjustment
{
    public int StockAdjustmentId { get; set; }

    public int LocationId { get; set; }

    public int ItemId { get; set; }

    public int InventoryLotId { get; set; }

    public int QuantityDelta { get; set; }

    public int ReasonCode { get; set; }

    public int ApprovedBy { get; set; }

    public DateTime PostedDate { get; set; }

    public virtual User ApprovedByNavigation { get; set; } = null!;

    public virtual InventoryLot InventoryLot { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;

    public virtual Location Location { get; set; } = null!;

    public virtual Reason ReasonCodeNavigation { get; set; } = null!;
}
