using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PharmaStock.Models;

public partial class PharmaStockContext : DbContext
{
    public PharmaStockContext()
    {
    }

    public PharmaStockContext(DbContextOptions<PharmaStockContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Bin> Bins { get; set; }

    public virtual DbSet<ColdChainLog> ColdChainLogs { get; set; }

    public virtual DbSet<DispenseRef> DispenseRefs { get; set; }

    public virtual DbSet<Drug> Drugs { get; set; }

    public virtual DbSet<ExpiryWatch> ExpiryWatches { get; set; }

    public virtual DbSet<GoodsReceipt> GoodsReceipts { get; set; }

    public virtual DbSet<Grnitem> Grnitems { get; set; }

    public virtual DbSet<InventoryBalance> InventoryBalances { get; set; }

    public virtual DbSet<InventoryLot> InventoryLots { get; set; }

    public virtual DbSet<InventoryReport> InventoryReports { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Poitem> Poitems { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<PutAwayTask> PutAwayTasks { get; set; }

    public virtual DbSet<QuarantineAction> QuarantineActions { get; set; }

    public virtual DbSet<RecallNotice> RecallNotices { get; set; }

    public virtual DbSet<ReplenishmentRequest> ReplenishmentRequests { get; set; }

    public virtual DbSet<ReplenishmentRule> ReplenishmentRules { get; set; }

    public virtual DbSet<StockAdjustment> StockAdjustments { get; set; }

    public virtual DbSet<StockCount> StockCounts { get; set; }

    public virtual DbSet<StockCountItem> StockCountItems { get; set; }

    public virtual DbSet<StockTransaction> StockTransactions { get; set; }

    public virtual DbSet<TransferItem> TransferItems { get; set; }

    public virtual DbSet<TransferOrder> TransferOrders { get; set; }

    public virtual DbSet<UoM> UoMs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vendor> Vendors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LTIN717944\\SQLEXPRESS;initial catalog=pharmastock;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true;Integrated Security=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.AuditId).HasName("PK__AuditLog__A17F23B874F4A00E");

            entity.ToTable("AuditLog");

            entity.Property(e => e.AuditId)
                .HasMaxLength(50)
                .HasColumnName("AuditID");
            entity.Property(e => e.Action).HasMaxLength(100);
            entity.Property(e => e.Resource).HasMaxLength(100);
            entity.Property(e => e.Timestamp).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__AuditLog__UserID__5535A963");
        });

        modelBuilder.Entity<Bin>(entity =>
        {
            entity.HasKey(e => e.BinId).HasName("PK__Bin__4BFF5A4ECBCE84BB");

            entity.ToTable("Bin");

            entity.Property(e => e.BinId)
                .HasMaxLength(50)
                .HasColumnName("BinID");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.LocationId)
                .HasMaxLength(50)
                .HasColumnName("LocationID");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.StorageClass).HasMaxLength(50);

            entity.HasOne(d => d.Location).WithMany(p => p.Bins)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Bin__LocationID__6477ECF3");
        });

        modelBuilder.Entity<ColdChainLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__ColdChai__5E5499A84264CEB4");

            entity.ToTable("ColdChainLog");

            entity.Property(e => e.LogId)
                .HasMaxLength(50)
                .HasColumnName("LogID");
            entity.Property(e => e.LocationId)
                .HasMaxLength(50)
                .HasColumnName("LocationID");
            entity.Property(e => e.SensorId)
                .HasMaxLength(50)
                .HasColumnName("SensorID");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.TemperatureC).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Timestamp).HasColumnType("datetime");

            entity.HasOne(d => d.Location).WithMany(p => p.ColdChainLogs)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__ColdChain__Locat__09A971A2");
        });

        modelBuilder.Entity<DispenseRef>(entity =>
        {
            entity.HasKey(e => e.DispenseId).HasName("PK__Dispense__26DF670A1EFD74F9");

            entity.ToTable("DispenseRef");

            entity.Property(e => e.DispenseId)
                .HasMaxLength(50)
                .HasColumnName("DispenseID");
            entity.Property(e => e.Destination).HasMaxLength(100);
            entity.Property(e => e.DispenseDate).HasColumnType("datetime");
            entity.Property(e => e.ItemId)
                .HasMaxLength(50)
                .HasColumnName("ItemID");
            entity.Property(e => e.LocationId)
                .HasMaxLength(50)
                .HasColumnName("LocationID");
            entity.Property(e => e.LotId)
                .HasMaxLength(50)
                .HasColumnName("LotID");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Item).WithMany(p => p.DispenseRefs)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__DispenseR__ItemI__2645B050");

            entity.HasOne(d => d.Location).WithMany(p => p.DispenseRefs)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__DispenseR__Locat__25518C17");

            entity.HasOne(d => d.Lot).WithMany(p => p.DispenseRefs)
                .HasForeignKey(d => d.LotId)
                .HasConstraintName("FK__DispenseR__LotID__2739D489");
        });

        modelBuilder.Entity<Drug>(entity =>
        {
            entity.HasKey(e => e.DrugId).HasName("PK__Drug__908D66F6325DAFC4");

            entity.ToTable("Drug");

            entity.Property(e => e.DrugId)
                .HasMaxLength(50)
                .HasColumnName("DrugID");
            entity.Property(e => e.Atccode)
                .HasMaxLength(50)
                .HasColumnName("ATCCode");
            entity.Property(e => e.BrandName).HasMaxLength(200);
            entity.Property(e => e.ControlClass).HasMaxLength(50);
            entity.Property(e => e.Form).HasMaxLength(50);
            entity.Property(e => e.GenericName).HasMaxLength(200);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.StorageClass).HasMaxLength(50);
            entity.Property(e => e.Strength).HasMaxLength(50);
        });

        modelBuilder.Entity<ExpiryWatch>(entity =>
        {
            entity.HasKey(e => e.WatchId).HasName("PK__ExpiryWa__3BA3DA83B4EA5463");

            entity.ToTable("ExpiryWatch");

            entity.Property(e => e.WatchId)
                .HasMaxLength(50)
                .HasColumnName("WatchID");
            entity.Property(e => e.FlagDate).HasColumnType("datetime");
            entity.Property(e => e.LotId)
                .HasMaxLength(50)
                .HasColumnName("LotID");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Lot).WithMany(p => p.ExpiryWatches)
                .HasForeignKey(d => d.LotId)
                .HasConstraintName("FK__ExpiryWat__LotID__787EE5A0");
        });

        modelBuilder.Entity<GoodsReceipt>(entity =>
        {
            entity.HasKey(e => e.Grnid).HasName("PK__GoodsRec__BC0E8C62AEB2E05E");

            entity.ToTable("GoodsReceipt");

            entity.Property(e => e.Grnid)
                .HasMaxLength(50)
                .HasColumnName("GRNID");
            entity.Property(e => e.Poid)
                .HasMaxLength(50)
                .HasColumnName("POID");
            entity.Property(e => e.ReceivedDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Po).WithMany(p => p.GoodsReceipts)
                .HasForeignKey(d => d.Poid)
                .HasConstraintName("FK__GoodsRecei__POID__71D1E811");

            entity.HasOne(d => d.ReceivedByNavigation).WithMany(p => p.GoodsReceipts)
                .HasForeignKey(d => d.ReceivedBy)
                .HasConstraintName("FK__GoodsRece__Recei__72C60C4A");
        });

        modelBuilder.Entity<Grnitem>(entity =>
        {
            entity.HasKey(e => e.GrnitemId).HasName("PK__GRNItem__89F1FEBE47D17B32");

            entity.ToTable("GRNItem");

            entity.Property(e => e.GrnitemId)
                .HasMaxLength(50)
                .HasColumnName("GRNItemID");
            entity.Property(e => e.BatchNumber).HasMaxLength(100);
            entity.Property(e => e.Grnid)
                .HasMaxLength(50)
                .HasColumnName("GRNID");
            entity.Property(e => e.ItemId)
                .HasMaxLength(50)
                .HasColumnName("ItemID");
            entity.Property(e => e.PoitemId)
                .HasMaxLength(50)
                .HasColumnName("POItemID");

            entity.HasOne(d => d.Grn).WithMany(p => p.Grnitems)
                .HasForeignKey(d => d.Grnid)
                .HasConstraintName("FK__GRNItem__GRNID__0C85DE4D");

            entity.HasOne(d => d.Item).WithMany(p => p.Grnitems)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__GRNItem__ItemID__0E6E26BF");

            entity.HasOne(d => d.Poitem).WithMany(p => p.Grnitems)
                .HasForeignKey(d => d.PoitemId)
                .HasConstraintName("FK__GRNItem__POItemI__0D7A0286");
        });

        modelBuilder.Entity<InventoryBalance>(entity =>
        {
            entity.HasKey(e => e.BalanceId).HasName("PK__Inventor__A760D59E0DF02E43");

            entity.ToTable("InventoryBalance");

            entity.Property(e => e.BalanceId)
                .HasMaxLength(50)
                .HasColumnName("BalanceID");
            entity.Property(e => e.BinId)
                .HasMaxLength(50)
                .HasColumnName("BinID");
            entity.Property(e => e.ItemId)
                .HasMaxLength(50)
                .HasColumnName("ItemID");
            entity.Property(e => e.LocationId)
                .HasMaxLength(50)
                .HasColumnName("LocationID");
            entity.Property(e => e.LotId)
                .HasMaxLength(50)
                .HasColumnName("LotID");

            entity.HasOne(d => d.Bin).WithMany(p => p.InventoryBalances)
                .HasForeignKey(d => d.BinId)
                .HasConstraintName("FK__Inventory__BinID__1AD3FDA4");

            entity.HasOne(d => d.Item).WithMany(p => p.InventoryBalances)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__Inventory__ItemI__1BC821DD");

            entity.HasOne(d => d.Location).WithMany(p => p.InventoryBalances)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Inventory__Locat__19DFD96B");

            entity.HasOne(d => d.Lot).WithMany(p => p.InventoryBalances)
                .HasForeignKey(d => d.LotId)
                .HasConstraintName("FK__Inventory__LotID__1CBC4616");
        });

        modelBuilder.Entity<InventoryLot>(entity =>
        {
            entity.HasKey(e => e.LotId).HasName("PK__Inventor__4160EF4DC6592FD7");

            entity.ToTable("InventoryLot");

            entity.Property(e => e.LotId)
                .HasMaxLength(50)
                .HasColumnName("LotID");
            entity.Property(e => e.BatchNumber).HasMaxLength(100);
            entity.Property(e => e.ItemId)
                .HasMaxLength(50)
                .HasColumnName("ItemID");
            entity.Property(e => e.Manufacturer).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Item).WithMany(p => p.InventoryLots)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__Inventory__ItemI__6B24EA82");
        });

        modelBuilder.Entity<InventoryReport>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Inventor__D5BD48E5D5E55A3E");

            entity.ToTable("InventoryReport");

            entity.Property(e => e.ReportId)
                .HasMaxLength(50)
                .HasColumnName("ReportID");
            entity.Property(e => e.GeneratedDate).HasColumnType("datetime");
            entity.Property(e => e.Scope).HasMaxLength(50);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__Item__727E83EB7180D482");

            entity.ToTable("Item");

            entity.Property(e => e.ItemId)
                .HasMaxLength(50)
                .HasColumnName("ItemID");
            entity.Property(e => e.Barcode).HasMaxLength(100);
            entity.Property(e => e.ConversionToEach).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DrugId)
                .HasMaxLength(50)
                .HasColumnName("DrugID");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.UoMid)
                .HasMaxLength(50)
                .HasColumnName("UoMID");

            entity.HasOne(d => d.Drug).WithMany(p => p.Items)
                .HasForeignKey(d => d.DrugId)
                .HasConstraintName("FK__Item__DrugID__60A75C0F");

            entity.HasOne(d => d.UoM).WithMany(p => p.Items)
                .HasForeignKey(d => d.UoMid)
                .HasConstraintName("FK__Item__UoMID__619B8048");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PK__Location__E7FEA4774558E4B4");

            entity.ToTable("Location");

            entity.Property(e => e.LocationId)
                .HasMaxLength(50)
                .HasColumnName("LocationID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.ParentLocationId)
                .HasMaxLength(50)
                .HasColumnName("ParentLocationID");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.ParentLocation).WithMany(p => p.InverseParentLocation)
                .HasForeignKey(d => d.ParentLocationId)
                .HasConstraintName("FK__Location__Parent__5AEE82B9");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E3250C06B00");

            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId)
                .HasMaxLength(50)
                .HasColumnName("NotificationID");
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Notificat__UserI__5812160E");
        });

        modelBuilder.Entity<Poitem>(entity =>
        {
            entity.HasKey(e => e.PoitemId).HasName("PK__POItem__CA5147B0C592F96E");

            entity.ToTable("POItem");

            entity.Property(e => e.PoitemId)
                .HasMaxLength(50)
                .HasColumnName("POItemID");
            entity.Property(e => e.ItemId)
                .HasMaxLength(50)
                .HasColumnName("ItemID");
            entity.Property(e => e.Poid)
                .HasMaxLength(50)
                .HasColumnName("POID");
            entity.Property(e => e.TaxPct).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Item).WithMany(p => p.Poitems)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__POItem__ItemID__6EF57B66");

            entity.HasOne(d => d.Po).WithMany(p => p.Poitems)
                .HasForeignKey(d => d.Poid)
                .HasConstraintName("FK__POItem__POID__6E01572D");
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.Poid).HasName("PK__Purchase__5F02A2F4BABE1514");

            entity.ToTable("PurchaseOrder");

            entity.Property(e => e.Poid)
                .HasMaxLength(50)
                .HasColumnName("POID");
            entity.Property(e => e.ExpectedDate).HasColumnType("datetime");
            entity.Property(e => e.LocationId)
                .HasMaxLength(50)
                .HasColumnName("LocationID");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.VendorId)
                .HasMaxLength(50)
                .HasColumnName("VendorID");

            entity.HasOne(d => d.Location).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__PurchaseO__Locat__68487DD7");

            entity.HasOne(d => d.Vendor).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.VendorId)
                .HasConstraintName("FK__PurchaseO__Vendo__6754599E");
        });

        modelBuilder.Entity<PutAwayTask>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__PutAwayT__7C6949D1F2E88A36");

            entity.ToTable("PutAwayTask");

            entity.Property(e => e.TaskId)
                .HasMaxLength(50)
                .HasColumnName("TaskID");
            entity.Property(e => e.GrnitemId)
                .HasMaxLength(50)
                .HasColumnName("GRNItemID");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.TargetBinId)
                .HasMaxLength(50)
                .HasColumnName("TargetBinID");

            entity.HasOne(d => d.Grnitem).WithMany(p => p.PutAwayTasks)
                .HasForeignKey(d => d.GrnitemId)
                .HasConstraintName("FK__PutAwayTa__GRNIt__160F4887");

            entity.HasOne(d => d.TargetBin).WithMany(p => p.PutAwayTasks)
                .HasForeignKey(d => d.TargetBinId)
                .HasConstraintName("FK__PutAwayTa__Targe__17036CC0");
        });

        modelBuilder.Entity<QuarantineAction>(entity =>
        {
            entity.HasKey(e => e.Qaid).HasName("PK__Quaranti__DFA59380E09B385E");

            entity.ToTable("QuarantineAction");

            entity.Property(e => e.Qaid)
                .HasMaxLength(50)
                .HasColumnName("QAID");
            entity.Property(e => e.LotId)
                .HasMaxLength(50)
                .HasColumnName("LotID");
            entity.Property(e => e.QuarantineDate).HasColumnType("datetime");
            entity.Property(e => e.ReleasedDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Lot).WithMany(p => p.QuarantineActions)
                .HasForeignKey(d => d.LotId)
                .HasConstraintName("FK__Quarantin__LotID__75A278F5");
        });

        modelBuilder.Entity<RecallNotice>(entity =>
        {
            entity.HasKey(e => e.RecallId).HasName("PK__RecallNo__DB2339A3274BA3D3");

            entity.ToTable("RecallNotice");

            entity.Property(e => e.RecallId)
                .HasMaxLength(50)
                .HasColumnName("RecallID");
            entity.Property(e => e.Action).HasMaxLength(50);
            entity.Property(e => e.DrugId)
                .HasMaxLength(50)
                .HasColumnName("DrugID");
            entity.Property(e => e.NoticeDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Drug).WithMany(p => p.RecallNotices)
                .HasForeignKey(d => d.DrugId)
                .HasConstraintName("FK__RecallNot__DrugI__5DCAEF64");
        });

        modelBuilder.Entity<ReplenishmentRequest>(entity =>
        {
            entity.HasKey(e => e.ReqId).HasName("PK__Replenis__28A9A3A2C7E24806");

            entity.ToTable("ReplenishmentRequest");

            entity.Property(e => e.ReqId)
                .HasMaxLength(50)
                .HasColumnName("ReqID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ItemId)
                .HasMaxLength(50)
                .HasColumnName("ItemID");
            entity.Property(e => e.LocationId)
                .HasMaxLength(50)
                .HasColumnName("LocationID");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Item).WithMany(p => p.ReplenishmentRequests)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__Replenish__ItemI__00200768");

            entity.HasOne(d => d.Location).WithMany(p => p.ReplenishmentRequests)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Replenish__Locat__7F2BE32F");
        });

        modelBuilder.Entity<ReplenishmentRule>(entity =>
        {
            entity.HasKey(e => e.RuleId).HasName("PK__Replenis__110458C2CEF4AA3D");

            entity.ToTable("ReplenishmentRule");

            entity.Property(e => e.RuleId)
                .HasMaxLength(50)
                .HasColumnName("RuleID");
            entity.Property(e => e.ItemId)
                .HasMaxLength(50)
                .HasColumnName("ItemID");
            entity.Property(e => e.LocationId)
                .HasMaxLength(50)
                .HasColumnName("LocationID");
            entity.Property(e => e.ReviewCycle).HasMaxLength(50);

            entity.HasOne(d => d.Item).WithMany(p => p.ReplenishmentRules)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__Replenish__ItemI__7C4F7684");

            entity.HasOne(d => d.Location).WithMany(p => p.ReplenishmentRules)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__Replenish__Locat__7B5B524B");
        });

        modelBuilder.Entity<StockAdjustment>(entity =>
        {
            entity.HasKey(e => e.AdjustmentId).HasName("PK__StockAdj__E60DB8B30642F298");

            entity.ToTable("StockAdjustment");

            entity.Property(e => e.AdjustmentId)
                .HasMaxLength(50)
                .HasColumnName("AdjustmentID");
            entity.Property(e => e.ItemId)
                .HasMaxLength(50)
                .HasColumnName("ItemID");
            entity.Property(e => e.LocationId)
                .HasMaxLength(50)
                .HasColumnName("LocationID");
            entity.Property(e => e.LotId)
                .HasMaxLength(50)
                .HasColumnName("LotID");
            entity.Property(e => e.PostedDate).HasColumnType("datetime");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.StockAdjustments)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK__StockAdju__Appro__32AB8735");

            entity.HasOne(d => d.Item).WithMany(p => p.StockAdjustments)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__StockAdju__ItemI__30C33EC3");

            entity.HasOne(d => d.Location).WithMany(p => p.StockAdjustments)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__StockAdju__Locat__2FCF1A8A");

            entity.HasOne(d => d.Lot).WithMany(p => p.StockAdjustments)
                .HasForeignKey(d => d.LotId)
                .HasConstraintName("FK__StockAdju__LotID__31B762FC");
        });

        modelBuilder.Entity<StockCount>(entity =>
        {
            entity.HasKey(e => e.CountId).HasName("PK__StockCou__06678C9CA72D3AD3");

            entity.ToTable("StockCount");

            entity.Property(e => e.CountId)
                .HasMaxLength(50)
                .HasColumnName("CountID");
            entity.Property(e => e.Cycle).HasMaxLength(50);
            entity.Property(e => e.LocationId)
                .HasMaxLength(50)
                .HasColumnName("LocationID");
            entity.Property(e => e.ScheduledDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Location).WithMany(p => p.StockCounts)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__StockCoun__Locat__06CD04F7");
        });

        modelBuilder.Entity<StockCountItem>(entity =>
        {
            entity.HasKey(e => e.CountItemId).HasName("PK__StockCou__585EF8C5632A91F1");

            entity.ToTable("StockCountItem");

            entity.Property(e => e.CountItemId)
                .HasMaxLength(50)
                .HasColumnName("CountItemID");
            entity.Property(e => e.BinId)
                .HasMaxLength(50)
                .HasColumnName("BinID");
            entity.Property(e => e.CountId)
                .HasMaxLength(50)
                .HasColumnName("CountID");
            entity.Property(e => e.ItemId)
                .HasMaxLength(50)
                .HasColumnName("ItemID");
            entity.Property(e => e.LotId)
                .HasMaxLength(50)
                .HasColumnName("LotID");
            entity.Property(e => e.ReasonCode).HasMaxLength(100);

            entity.HasOne(d => d.Bin).WithMany(p => p.StockCountItems)
                .HasForeignKey(d => d.BinId)
                .HasConstraintName("FK__StockCoun__BinID__2B0A656D");

            entity.HasOne(d => d.Count).WithMany(p => p.StockCountItems)
                .HasForeignKey(d => d.CountId)
                .HasConstraintName("FK__StockCoun__Count__2A164134");

            entity.HasOne(d => d.Item).WithMany(p => p.StockCountItems)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__StockCoun__ItemI__2BFE89A6");

            entity.HasOne(d => d.Lot).WithMany(p => p.StockCountItems)
                .HasForeignKey(d => d.LotId)
                .HasConstraintName("FK__StockCoun__LotID__2CF2ADDF");
        });

        modelBuilder.Entity<StockTransaction>(entity =>
        {
            entity.HasKey(e => e.TxnId).HasName("PK__StockTra__C19608744D4DCD1E");

            entity.ToTable("StockTransaction");

            entity.Property(e => e.TxnId)
                .HasMaxLength(50)
                .HasColumnName("TxnID");
            entity.Property(e => e.BinId)
                .HasMaxLength(50)
                .HasColumnName("BinID");
            entity.Property(e => e.ItemId)
                .HasMaxLength(50)
                .HasColumnName("ItemID");
            entity.Property(e => e.LocationId)
                .HasMaxLength(50)
                .HasColumnName("LocationID");
            entity.Property(e => e.LotId)
                .HasMaxLength(50)
                .HasColumnName("LotID");
            entity.Property(e => e.ReferenceId)
                .HasMaxLength(100)
                .HasColumnName("ReferenceID");
            entity.Property(e => e.TxnDate).HasColumnType("datetime");
            entity.Property(e => e.TxnType).HasMaxLength(50);

            entity.HasOne(d => d.Bin).WithMany(p => p.StockTransactions)
                .HasForeignKey(d => d.BinId)
                .HasConstraintName("FK__StockTran__BinID__208CD6FA");

            entity.HasOne(d => d.Item).WithMany(p => p.StockTransactions)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__StockTran__ItemI__2180FB33");

            entity.HasOne(d => d.Location).WithMany(p => p.StockTransactions)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__StockTran__Locat__1F98B2C1");

            entity.HasOne(d => d.Lot).WithMany(p => p.StockTransactions)
                .HasForeignKey(d => d.LotId)
                .HasConstraintName("FK__StockTran__LotID__22751F6C");
        });

        modelBuilder.Entity<TransferItem>(entity =>
        {
            entity.HasKey(e => e.ToitemId).HasName("PK__Transfer__FC6491FC5A347811");

            entity.ToTable("TransferItem");

            entity.Property(e => e.ToitemId)
                .HasMaxLength(50)
                .HasColumnName("TOItemID");
            entity.Property(e => e.ItemId)
                .HasMaxLength(50)
                .HasColumnName("ItemID");
            entity.Property(e => e.LotId)
                .HasMaxLength(50)
                .HasColumnName("LotID");
            entity.Property(e => e.Toid)
                .HasMaxLength(50)
                .HasColumnName("TOID");

            entity.HasOne(d => d.Item).WithMany(p => p.TransferItems)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__TransferI__ItemI__123EB7A3");

            entity.HasOne(d => d.Lot).WithMany(p => p.TransferItems)
                .HasForeignKey(d => d.LotId)
                .HasConstraintName("FK__TransferI__LotID__1332DBDC");

            entity.HasOne(d => d.To).WithMany(p => p.TransferItems)
                .HasForeignKey(d => d.Toid)
                .HasConstraintName("FK__TransferIt__TOID__114A936A");
        });

        modelBuilder.Entity<TransferOrder>(entity =>
        {
            entity.HasKey(e => e.Toid).HasName("PK__Transfer__A7B0727140244157");

            entity.ToTable("TransferOrder");

            entity.Property(e => e.Toid)
                .HasMaxLength(50)
                .HasColumnName("TOID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.FromLocationId)
                .HasMaxLength(50)
                .HasColumnName("FromLocationID");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.ToLocationId)
                .HasMaxLength(50)
                .HasColumnName("ToLocationID");

            entity.HasOne(d => d.FromLocation).WithMany(p => p.TransferOrderFromLocations)
                .HasForeignKey(d => d.FromLocationId)
                .HasConstraintName("FK__TransferO__FromL__02FC7413");

            entity.HasOne(d => d.ToLocation).WithMany(p => p.TransferOrderToLocations)
                .HasForeignKey(d => d.ToLocationId)
                .HasConstraintName("FK__TransferO__ToLoc__03F0984C");
        });

        modelBuilder.Entity<UoM>(entity =>
        {
            entity.HasKey(e => e.UoMid).HasName("PK__UoM__8286E93C91BD29AE");

            entity.ToTable("UoM");

            entity.Property(e => e.UoMid)
                .HasMaxLength(50)
                .HasColumnName("UoMID");
            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.Description).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCAC0821F459");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__A9D10534132BE290").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(10);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(20);
        });

        modelBuilder.Entity<Vendor>(entity =>
        {
            entity.HasKey(e => e.VendorId).HasName("PK__Vendor__FC8618D3A9A08E87");

            entity.ToTable("Vendor");

            entity.Property(e => e.VendorId)
                .HasMaxLength(50)
                .HasColumnName("VendorID");
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Status).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
