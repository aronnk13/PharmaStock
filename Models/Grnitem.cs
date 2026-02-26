using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class Grnitem
{
    public string GrnitemId { get; set; } = null!;

    public string? Grnid { get; set; }

    public string? PoitemId { get; set; }

    public string? ItemId { get; set; }

    public string? BatchNumber { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public int ReceivedQty { get; set; }

    public int AcceptedQty { get; set; }

    public int RejectedQty { get; set; }

    public string? Reason { get; set; }

    public virtual GoodsReceipt? Grn { get; set; }

    public virtual Item? Item { get; set; }

    public virtual Poitem? Poitem { get; set; }

    public virtual ICollection<PutAwayTask> PutAwayTasks { get; set; } = new List<PutAwayTask>();
}
