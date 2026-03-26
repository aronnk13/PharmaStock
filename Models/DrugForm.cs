using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class DrugForm
{
    public int DrugFormId { get; set; }

    public string Form { get; set; } = null!;

    public virtual ICollection<Drug> Drugs { get; set; } = new List<Drug>();
}
