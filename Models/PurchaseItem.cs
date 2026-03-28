using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class PurchaseItem
{
    public int PurchaseItemId { get; set; }

    public int PurchaseOrderId { get; set; }

    public int ItemId { get; set; }

    public int OrderedQty { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TaxPct { get; set; }

    public virtual ICollection<GoodsReceiptItem> GoodsReceiptItems { get; set; } = new List<GoodsReceiptItem>();

    public virtual Item Item { get; set; } = null!;

    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;
}
