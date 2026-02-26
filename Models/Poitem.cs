using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class Poitem
{
    public string PoitemId { get; set; } = null!;

    public string? Poid { get; set; }

    public string? ItemId { get; set; }

    public int OrderedQty { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? TaxPct { get; set; }

    public virtual ICollection<Grnitem> Grnitems { get; set; } = new List<Grnitem>();

    public virtual Item? Item { get; set; }

    public virtual PurchaseOrder? Po { get; set; }
}
