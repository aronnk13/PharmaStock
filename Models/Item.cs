using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class Item
{
    public string ItemId { get; set; } = null!;

    public string? DrugId { get; set; }

    public int PackSize { get; set; }

    public string? UoMid { get; set; }

    public decimal? ConversionToEach { get; set; }

    public string? Barcode { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<DispenseRef> DispenseRefs { get; set; } = new List<DispenseRef>();

    public virtual Drug? Drug { get; set; }

    public virtual ICollection<Grnitem> Grnitems { get; set; } = new List<Grnitem>();

    public virtual ICollection<InventoryBalance> InventoryBalances { get; set; } = new List<InventoryBalance>();

    public virtual ICollection<InventoryLot> InventoryLots { get; set; } = new List<InventoryLot>();

    public virtual ICollection<Poitem> Poitems { get; set; } = new List<Poitem>();

    public virtual ICollection<ReplenishmentRequest> ReplenishmentRequests { get; set; } = new List<ReplenishmentRequest>();

    public virtual ICollection<ReplenishmentRule> ReplenishmentRules { get; set; } = new List<ReplenishmentRule>();

    public virtual ICollection<StockAdjustment> StockAdjustments { get; set; } = new List<StockAdjustment>();

    public virtual ICollection<StockCountItem> StockCountItems { get; set; } = new List<StockCountItem>();

    public virtual ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();

    public virtual ICollection<TransferItem> TransferItems { get; set; } = new List<TransferItem>();

    public virtual UoM? UoM { get; set; }
}
