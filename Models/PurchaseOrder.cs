using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class PurchaseOrder
{
    public int PurchaseOrderId { get; set; }

    public int VendorId { get; set; }

    public int LocationId { get; set; }

    public DateOnly OrderDate { get; set; }

    public DateOnly ExpectedDate { get; set; }

    public int PurchaseOrderStatusId { get; set; }

    public virtual ICollection<GoodsReciept> GoodsReciepts { get; set; } = new List<GoodsReciept>();

    public virtual Location Location { get; set; } = null!;

    public virtual ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();

    public virtual PurchaseOrderStatus PurchaseOrderStatus { get; set; } = null!;

    public virtual Vendor Vendor { get; set; } = null!;
}
