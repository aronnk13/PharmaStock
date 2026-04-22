using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public int DrugId { get; set; }

    public int? PackSize { get; set; }

    public int UoM { get; set; }

    public decimal ConversionToEach { get; set; }

    public string? Barcode { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<DispenseRef> DispenseRefs { get; set; } = new List<DispenseRef>();

    public virtual Drug Drug { get; set; } = null!;

    public virtual ICollection<GoodsReceiptItem> GoodsReceiptItems { get; set; } = new List<GoodsReceiptItem>();

    public virtual ICollection<InventoryBalance> InventoryBalances { get; set; } = new List<InventoryBalance>();

    public virtual ICollection<InventoryLot> InventoryLots { get; set; } = new List<InventoryLot>();

    public virtual ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();

    public virtual ICollection<ReplenishmentRequest> ReplenishmentRequests { get; set; } = new List<ReplenishmentRequest>();

    public virtual ICollection<ReplenishmentRule> ReplenishmentRules { get; set; } = new List<ReplenishmentRule>();

    public virtual ICollection<StockAdjustment> StockAdjustments { get; set; } = new List<StockAdjustment>();

    public virtual ICollection<StockCountItem> StockCountItems { get; set; } = new List<StockCountItem>();

    public virtual ICollection<StockTransition> StockTransitions { get; set; } = new List<StockTransition>();

    public virtual ICollection<TransferItem> TransferItems { get; set; } = new List<TransferItem>();

    public virtual UoM UoMNavigation { get; set; } = null!;
}
