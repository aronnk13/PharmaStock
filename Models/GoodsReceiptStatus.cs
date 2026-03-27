using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class GoodsReceiptStatus
{
    public int GoodsReceiptStatusId { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<GoodsReciept> GoodsReciepts { get; set; } = new List<GoodsReciept>();
}
