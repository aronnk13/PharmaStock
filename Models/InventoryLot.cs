using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class InventoryLot
{
    public string LotId { get; set; } = null!;

    public string? ItemId { get; set; }

    public string BatchNumber { get; set; } = null!;

    public DateOnly ExpiryDate { get; set; }

    public string? Manufacturer { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<DispenseRef> DispenseRefs { get; set; } = new List<DispenseRef>();

    public virtual ICollection<ExpiryWatch> ExpiryWatches { get; set; } = new List<ExpiryWatch>();

    public virtual ICollection<InventoryBalance> InventoryBalances { get; set; } = new List<InventoryBalance>();

    public virtual Item? Item { get; set; }

    public virtual ICollection<QuarantineAction> QuarantineActions { get; set; } = new List<QuarantineAction>();

    public virtual ICollection<StockAdjustment> StockAdjustments { get; set; } = new List<StockAdjustment>();

    public virtual ICollection<StockCountItem> StockCountItems { get; set; } = new List<StockCountItem>();

    public virtual ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();

    public virtual ICollection<TransferItem> TransferItems { get; set; } = new List<TransferItem>();
}
