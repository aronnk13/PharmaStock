using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class InventoryLot
{
    public int InventoryLotId { get; set; }

    public int ItemId { get; set; }

    public string BatchNumber { get; set; } = null!;

    public DateOnly ExpiryDate { get; set; }

    public int? ManufacturerId { get; set; }

    public int Status { get; set; }

    public virtual ICollection<DispenseRef> DispenseRefs { get; set; } = new List<DispenseRef>();

    public virtual ICollection<ExpiryWatch> ExpiryWatches { get; set; } = new List<ExpiryWatch>();

    public virtual ICollection<InventoryBalance> InventoryBalances { get; set; } = new List<InventoryBalance>();

    public virtual Item Item { get; set; } = null!;

    public virtual Manufacturer? Manufacturer { get; set; }

    public virtual ICollection<QuarantaineAction> QuarantaineActions { get; set; } = new List<QuarantaineAction>();

    public virtual InventoryLotStatus StatusNavigation { get; set; } = null!;

    public virtual ICollection<StockAdjustment> StockAdjustments { get; set; } = new List<StockAdjustment>();

    public virtual ICollection<StockCountItem> StockCountItems { get; set; } = new List<StockCountItem>();

    public virtual ICollection<StockTransition> StockTransitions { get; set; } = new List<StockTransition>();

    public virtual ICollection<TransferItem> TransferItems { get; set; } = new List<TransferItem>();
}
