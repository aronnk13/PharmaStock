using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class UoM
{
    public int UoMid { get; set; }

    public string Code { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
