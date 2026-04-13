using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class BinStorageClass
{
    public int BinStorageClassId { get; set; }

    public string StorageClass { get; set; } = null!;

    public virtual ICollection<Bin> Bins { get; set; } = new List<Bin>();
}
