using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class GoodsReciept
{
    public int GoodsRecieptId { get; set; }

    public int PurchaseOrderId { get; set; }

    public DateTime ReceivedDate { get; set; }

    public int Status { get; set; }

    public string? ReceivedBy { get; set; }

    public virtual ICollection<GoodsReceiptItem> GoodsReceiptItems { get; set; } = new List<GoodsReceiptItem>();

    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;

    public virtual GoodsReceiptStatus StatusNavigation { get; set; } = null!;
}
