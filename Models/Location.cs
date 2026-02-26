using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class Location
{
    public string LocationId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? ParentLocationId { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Bin> Bins { get; set; } = new List<Bin>();

    public virtual ICollection<ColdChainLog> ColdChainLogs { get; set; } = new List<ColdChainLog>();

    public virtual ICollection<DispenseRef> DispenseRefs { get; set; } = new List<DispenseRef>();

    public virtual ICollection<InventoryBalance> InventoryBalances { get; set; } = new List<InventoryBalance>();

    public virtual ICollection<Location> InverseParentLocation { get; set; } = new List<Location>();

    public virtual Location? ParentLocation { get; set; }

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

    public virtual ICollection<ReplenishmentRequest> ReplenishmentRequests { get; set; } = new List<ReplenishmentRequest>();

    public virtual ICollection<ReplenishmentRule> ReplenishmentRules { get; set; } = new List<ReplenishmentRule>();

    public virtual ICollection<StockAdjustment> StockAdjustments { get; set; } = new List<StockAdjustment>();

    public virtual ICollection<StockCount> StockCounts { get; set; } = new List<StockCount>();

    public virtual ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();

    public virtual ICollection<TransferOrder> TransferOrderFromLocations { get; set; } = new List<TransferOrder>();

    public virtual ICollection<TransferOrder> TransferOrderToLocations { get; set; } = new List<TransferOrder>();
}
