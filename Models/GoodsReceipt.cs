using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class GoodsReceipt
{
    public string Grnid { get; set; } = null!;

    public string? Poid { get; set; }

    public int? ReceivedBy { get; set; }

    public DateTime ReceivedDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Grnitem> Grnitems { get; set; } = new List<Grnitem>();

    public virtual PurchaseOrder? Po { get; set; }

    public virtual User? ReceivedByNavigation { get; set; }
}
