using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class TransferOrder
{
    public string Toid { get; set; } = null!;

    public string? FromLocationId { get; set; }

    public string? ToLocationId { get; set; }

    public DateTime CreatedDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Location? FromLocation { get; set; }

    public virtual Location? ToLocation { get; set; }

    public virtual ICollection<TransferItem> TransferItems { get; set; } = new List<TransferItem>();
}
