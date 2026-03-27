using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class TransferOrder
{
    public int TransferOrderId { get; set; }

    public int FromLocationId { get; set; }

    public int ToLocationId { get; set; }

    public DateTime CreatedDate { get; set; }

    public int Status { get; set; }

    public virtual Location FromLocation { get; set; } = null!;

    public virtual TransferOrderStatus StatusNavigation { get; set; } = null!;

    public virtual Location ToLocation { get; set; } = null!;

    public virtual ICollection<TransferItem> TransferItems { get; set; } = new List<TransferItem>();
}
