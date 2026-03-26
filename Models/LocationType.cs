using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class LocationType
{
    public int LocationTypeId { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
}
