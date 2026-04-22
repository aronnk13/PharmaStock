using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class DestinationType
{
    public int DestinationTypeId { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<DispenseRef> DispenseRefs { get; set; } = new List<DispenseRef>();
}
