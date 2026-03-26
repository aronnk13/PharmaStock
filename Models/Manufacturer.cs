using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class Manufacturer
{
    public int ManufacturerId { get; set; }

    public string ManufacturerName { get; set; } = null!;

    public virtual ICollection<InventoryLot> InventoryLots { get; set; } = new List<InventoryLot>();
}
