using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class Vendor
{
    public int VendorId { get; set; }

    public string Name { get; set; } = null!;

    public string? ContactInfo { get; set; }

    public int? Rating { get; set; }

    public bool? StatusId { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
}
