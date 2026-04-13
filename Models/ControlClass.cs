using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class ControlClass
{
    public int ControlClassId { get; set; }

    public string Class { get; set; } = null!;

    public virtual ICollection<Drug> Drugs { get; set; } = new List<Drug>();
}
