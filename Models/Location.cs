using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PharmaStock.Models;

public partial class Location
{
    public int LocationId { get; set; }

    public string Name { get; set; } = null!;

    public int LocationTypeId { get; set; }

    public int? ParentLocationId { get; set; }

    public bool StatusId { get; set; }

    public virtual ICollection<Bin> Bins { get; set; } = new List<Bin>();

    public virtual ICollection<DispenseRef> DispenseRefs { get; set; } = new List<DispenseRef>();

    public virtual ICollection<InventoryBalance> InventoryBalances { get; set; } = new List<InventoryBalance>();

    public virtual ICollection<Location> InverseParentLocation { get; set; } = new List<Location>();

    public virtual LocationType LocationType { get; set; } = null!;

    public virtual Location? ParentLocation { get; set; }

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

    public virtual ICollection<ReplenishmentRequest> ReplenishmentRequests { get; set; } = new List<ReplenishmentRequest>();

    public virtual ICollection<ReplenishmentRule> ReplenishmentRules { get; set; } = new List<ReplenishmentRule>();

    public virtual ICollection<StockAdjustment> StockAdjustments { get; set; } = new List<StockAdjustment>();

    public virtual ICollection<StockCount> StockCounts { get; set; } = new List<StockCount>();

    public virtual ICollection<StockTransition> StockTransitions { get; set; } = new List<StockTransition>();

    public virtual ICollection<TransferOrder> TransferOrderFromLocations { get; set; } = new List<TransferOrder>();

    public virtual ICollection<TransferOrder> TransferOrderToLocations { get; set; } = new List<TransferOrder>();



}
