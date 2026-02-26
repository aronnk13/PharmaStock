using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class PurchaseOrder
{
    public string Poid { get; set; } = null!;

    public string? VendorId { get; set; }

    public string? LocationId { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? ExpectedDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<GoodsReceipt> GoodsReceipts { get; set; } = new List<GoodsReceipt>();

    public virtual Location? Location { get; set; }

    public virtual ICollection<Poitem> Poitems { get; set; } = new List<Poitem>();

    public virtual Vendor? Vendor { get; set; }
}
