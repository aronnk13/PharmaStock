using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class TransferOrderStatus
{
    public int TransferOrderStatusId { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<TransferOrder> TransferOrders { get; set; } = new List<TransferOrder>();
}
