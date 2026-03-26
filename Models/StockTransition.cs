using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class StockTransition
{
    public int StockTransitionId { get; set; }

    public int LocationId { get; set; }

    public int BinId { get; set; }

    public int ItemId { get; set; }

    public int InventoryLotId { get; set; }

    public int StockTransitionType { get; set; }

    public int Quantity { get; set; }

    public DateTime StockTransitionTypeDate { get; set; }

    public string? ReferenceId { get; set; }

    public string? Notes { get; set; }

    public int PerformedBy { get; set; }

    public virtual Bin Bin { get; set; } = null!;

    public virtual InventoryLot InventoryLot { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;

    public virtual Location Location { get; set; } = null!;

    public virtual User PerformedByNavigation { get; set; } = null!;

    public virtual StockTransitionType StockTransitionTypeNavigation { get; set; } = null!;
}
