using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class DrugStorageClass
{
    public int DrugStorageClassId { get; set; }

    public string Class { get; set; } = null!;

    public virtual ICollection<Drug> Drugs { get; set; } = new List<Drug>();
}
