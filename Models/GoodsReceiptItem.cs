using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class GoodsReceiptItem
{
    public int GoodsReceiptItemId { get; set; }

    public int GoodsReceiptId { get; set; }

    public int PurchaseOrderItemId { get; set; }

    public int ItemId { get; set; }

    public int BatchNumber { get; set; }

    public DateOnly ExpiryDate { get; set; }

    public int ReceivedQty { get; set; }

    public int AcceptedQty { get; set; }

    public int RejectedQty { get; set; }

    public string? Reason { get; set; }

    public virtual GoodsReciept GoodsReceipt { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;

    public virtual PurchaseItem PurchaseOrderItem { get; set; } = null!;

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
