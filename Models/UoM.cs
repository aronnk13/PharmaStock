using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class UoM
{
    public string UoMid { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
