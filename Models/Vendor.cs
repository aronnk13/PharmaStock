using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class Vendor
{
    public string VendorId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? ContactInfo { get; set; }

    public int? Rating { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
}
