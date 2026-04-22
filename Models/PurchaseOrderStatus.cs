using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class PurchaseOrderStatus
{
    public int PurchaseOrderStatusId { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
}
