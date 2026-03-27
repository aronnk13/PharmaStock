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

    public virtual DbSet<Audit> Audits { get; set; }

    public virtual DbSet<Bin> Bins { get; set; }

    public virtual DbSet<BinStorageClass> BinStorageClasses { get; set; }

    public virtual DbSet<ControlClass> ControlClasses { get; set; }

    public virtual DbSet<CountCycle> CountCycles { get; set; }

    public virtual DbSet<DestinationType> DestinationTypes { get; set; }

    public virtual DbSet<DispenseRef> DispenseRefs { get; set; }

    public virtual DbSet<Drug> Drugs { get; set; }

    public virtual DbSet<DrugForm> DrugForms { get; set; }

    public virtual DbSet<DrugStorageClass> DrugStorageClasses { get; set; }

    public virtual DbSet<ExpiryWatch> ExpiryWatches { get; set; }

    public virtual DbSet<GoodsReceiptItem> GoodsReceiptItems { get; set; }

    public virtual DbSet<GoodsReceiptStatus> GoodsReceiptStatuses { get; set; }

    public virtual DbSet<GoodsReciept> GoodsReciepts { get; set; }

    public virtual DbSet<InventoryBalance> InventoryBalances { get; set; }

    public virtual DbSet<InventoryLot> InventoryLots { get; set; }

    public virtual DbSet<InventoryLotStatus> InventoryLotStatuses { get; set; }

    public virtual DbSet<InventoryReport> InventoryReports { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<LocationType> LocationTypes { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<NotificationCategory> NotificationCategories { get; set; }

    public virtual DbSet<NotificationStatus> NotificationStatuses { get; set; }

    public virtual DbSet<PurchaseItem> PurchaseItems { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<PurchaseOrderStatus> PurchaseOrderStatuses { get; set; }

    public virtual DbSet<QuarantaineAction> QuarantaineActions { get; set; }

    public virtual DbSet<QuarantineStatus> QuarantineStatuses { get; set; }

    public virtual DbSet<Reason> Reasons { get; set; }

    public virtual DbSet<RecallAction> RecallActions { get; set; }

    public virtual DbSet<RecallNotice> RecallNotices { get; set; }

    public virtual DbSet<ReplenishmentRequest> ReplenishmentRequests { get; set; }

    public virtual DbSet<ReplenishmentRule> ReplenishmentRules { get; set; }

    public virtual DbSet<ReplenishmentStatus> ReplenishmentStatuses { get; set; }

    public virtual DbSet<ReportScope> ReportScopes { get; set; }

    public virtual DbSet<StockAdjustment> StockAdjustments { get; set; }

    public virtual DbSet<StockCount> StockCounts { get; set; }

    public virtual DbSet<StockCountItem> StockCountItems { get; set; }

    public virtual DbSet<StockCountStatus> StockCountStatuses { get; set; }

    public virtual DbSet<StockTransition> StockTransitions { get; set; }

    public virtual DbSet<StockTransitionType> StockTransitionTypes { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TransferItem> TransferItems { get; set; }

    public virtual DbSet<TransferOrder> TransferOrders { get; set; }

    public virtual DbSet<TransferOrderStatus> TransferOrderStatuses { get; set; }

    public virtual DbSet<UoM> UoMs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<Vendor> Vendors { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=LTIN718770\\SQLEXPRESS;Initial Catalog=PHARMASTOCK;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True;Integrated Security=True");

    //  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     => optionsBuilder.UseSqlServer("PharmaDbConnection");
    
    // ToDo: Protect the connection string.


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Audit>(entity =>
        {
            entity.HasKey(e => e.AuditId).HasName("PK__Audit__A17F23B85A3C6410");

            entity.ToTable("Audit");

            entity.Property(e => e.AuditId).HasColumnName("AuditID");
            entity.Property(e => e.Action)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Resource)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Audits)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Audit_User");
        });

        modelBuilder.Entity<Bin>(entity =>
        {
            entity.HasKey(e => e.BinId).HasName("PK__Bin__4BFF5A4E63AE8F51");

            entity.ToTable("Bin");

            entity.Property(e => e.BinId).HasColumnName("BinID");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.StatusId)
                .HasDefaultValue(true)
                .HasColumnName("StatusID");

            entity.HasOne(d => d.BinStorageClassNavigation).WithMany(p => p.Bins)
                .HasForeignKey(d => d.BinStorageClass)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bin_Storage");

            entity.HasOne(d => d.Location).WithMany(p => p.Bins)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bin_Location");
        });

        modelBuilder.Entity<BinStorageClass>(entity =>
        {
            entity.HasKey(e => e.BinStorageClassId).HasName("PK__BinStora__E00857A4B27B455A");

            entity.ToTable("BinStorageClass");

            entity.Property(e => e.StorageClass)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ControlClass>(entity =>
        {
            entity.HasKey(e => e.ControlClassId).HasName("PK__ControlC__BA1ADFA69DC8305B");

            entity.ToTable("ControlClass");

            entity.HasIndex(e => e.Class, "UQ__ControlC__E81871BCCB6A29C9").IsUnique();

            entity.Property(e => e.ControlClassId).HasColumnName("ControlClassID");
            entity.Property(e => e.Class)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CountCycle>(entity =>
        {
            entity.HasKey(e => e.CountCycleId).HasName("PK__CountCyc__B2CF833C1B819EDA");

            entity.ToTable("CountCycle");

            entity.HasIndex(e => e.Cycle, "UQ__CountCyc__288654CB513314FD").IsUnique();

            entity.Property(e => e.CountCycleId).HasColumnName("CountCycleID");
            entity.Property(e => e.Cycle)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DestinationType>(entity =>
        {
            entity.HasKey(e => e.DestinationTypeId).HasName("PK__Destinat__22578B65FD10A4BB");

            entity.ToTable("DestinationType");

            entity.Property(e => e.DestinationTypeId).HasColumnName("DestinationTypeID");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DispenseRef>(entity =>
        {
            entity.HasKey(e => e.DispenseRefId).HasName("PK__Dispense__F85513AEE4A7FD3F");

            entity.ToTable("DispenseRef");

            entity.Property(e => e.DispenseRefId).HasColumnName("DispenseRefID");
            entity.Property(e => e.DispenseDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InventoryLotId).HasColumnName("InventoryLotID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.Status).HasDefaultValue(true);

            entity.HasOne(d => d.DestinationNavigation).WithMany(p => p.DispenseRefs)
                .HasForeignKey(d => d.Destination)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Disp_Dest");

            entity.HasOne(d => d.InventoryLot).WithMany(p => p.DispenseRefs)
                .HasForeignKey(d => d.InventoryLotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Disp_Lot");

            entity.HasOne(d => d.Item).WithMany(p => p.DispenseRefs)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Disp_Item");

            entity.HasOne(d => d.Location).WithMany(p => p.DispenseRefs)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Disp_Loc");
        });

        modelBuilder.Entity<Drug>(entity =>
        {
            entity.HasKey(e => e.DrugId).HasName("PK__Drug__908D66F6B76B16E9");

            entity.ToTable("Drug");

            entity.Property(e => e.DrugId).HasColumnName("DrugID");
            entity.Property(e => e.Atccode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ATCCode");
            entity.Property(e => e.BrandName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.GenericName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.Strength)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.ControlClassNavigation).WithMany(p => p.Drugs)
                .HasForeignKey(d => d.ControlClass)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Drug_Control");

            entity.HasOne(d => d.FormNavigation).WithMany(p => p.Drugs)
                .HasForeignKey(d => d.Form)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Drug_Form");

            entity.HasOne(d => d.StorageClassNavigation).WithMany(p => p.Drugs)
                .HasForeignKey(d => d.StorageClass)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Drug_Storage");
        });

        modelBuilder.Entity<DrugForm>(entity =>
        {
            entity.HasKey(e => e.DrugFormId).HasName("PK__DrugForm__913BB843CC9FDBD3");

            entity.ToTable("DrugForm");

            entity.HasIndex(e => e.Form, "UQ__DrugForm__386CF3FD5B5B1E91").IsUnique();

            entity.Property(e => e.DrugFormId).HasColumnName("DrugFormID");
            entity.Property(e => e.Form)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DrugStorageClass>(entity =>
        {
            entity.HasKey(e => e.DrugStorageClassId).HasName("PK__DrugStor__DD84E9B053F0E29C");

            entity.ToTable("DrugStorageClass");

            entity.HasIndex(e => e.Class, "UQ__DrugStor__E81871BC40FD23CD").IsUnique();

            entity.Property(e => e.DrugStorageClassId).HasColumnName("DrugStorageClassID");
            entity.Property(e => e.Class)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ExpiryWatch>(entity =>
        {
            entity.HasKey(e => e.ExpiryWatchId).HasName("PK__ExpiryWa__B465D391AC0154E5");

            entity.ToTable("ExpiryWatch");

            entity.Property(e => e.ExpiryWatchId).HasColumnName("ExpiryWatchID");
            entity.Property(e => e.InventoryLotId).HasColumnName("InventoryLotID");
            entity.Property(e => e.Status).HasDefaultValue(true);

            entity.HasOne(d => d.InventoryLot).WithMany(p => p.ExpiryWatches)
                .HasForeignKey(d => d.InventoryLotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expiry_Lot");
        });

        modelBuilder.Entity<GoodsReceiptItem>(entity =>
        {
            entity.HasKey(e => e.GoodsReceiptItemId).HasName("PK__GoodsRec__D732C28BDEA649D6");

            entity.ToTable("GoodsReceiptItem");

            entity.Property(e => e.GoodsReceiptItemId).HasColumnName("GoodsReceiptItemID");
            entity.Property(e => e.GoodsReceiptId).HasColumnName("GoodsReceiptID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.PurchaseOrderItemId).HasColumnName("PurchaseOrderItemID");
            entity.Property(e => e.Reason)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.HasOne(d => d.GoodsReceipt).WithMany(p => p.GoodsReceiptItems)
                .HasForeignKey(d => d.GoodsReceiptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GRI_GR");

            entity.HasOne(d => d.Item).WithMany(p => p.GoodsReceiptItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GRI_Item");

            entity.HasOne(d => d.PurchaseOrderItem).WithMany(p => p.GoodsReceiptItems)
                .HasForeignKey(d => d.PurchaseOrderItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GRI_PI");
        });

        modelBuilder.Entity<GoodsReceiptStatus>(entity =>
        {
            entity.HasKey(e => e.GoodsReceiptStatusId).HasName("PK__GoodsRec__B25EB86699078223");

            entity.ToTable("GoodsReceiptStatus");

            entity.Property(e => e.GoodsReceiptStatusId).HasColumnName("GoodsReceiptStatusID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<GoodsReciept>(entity =>
        {
            entity.HasKey(e => e.GoodsRecieptId).HasName("PK__GoodsRec__B1C169970FA17650");

            entity.ToTable("GoodsReciept");

            entity.Property(e => e.PurchaseOrderId).HasColumnName("PurchaseOrderID");
            entity.Property(e => e.ReceivedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.GoodsReciepts)
                .HasForeignKey(d => d.PurchaseOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GR_PO");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.GoodsReciepts)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GR_Status");
        });

        modelBuilder.Entity<InventoryBalance>(entity =>
        {
            entity.HasKey(e => e.InventoryBalanceId).HasName("PK__Inventor__FBB7928E1E39DC6C");

            entity.ToTable("InventoryBalance");

            entity.Property(e => e.InventoryBalanceId).HasColumnName("InventoryBalanceID");
            entity.Property(e => e.BinId).HasColumnName("BinID");
            entity.Property(e => e.InventoryLotId).HasColumnName("InventoryLotID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.LocationId).HasColumnName("LocationID");

            entity.HasOne(d => d.Bin).WithMany(p => p.InventoryBalances)
                .HasForeignKey(d => d.BinId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bal_Bin");

            entity.HasOne(d => d.InventoryLot).WithMany(p => p.InventoryBalances)
                .HasForeignKey(d => d.InventoryLotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bal_Lot");

            entity.HasOne(d => d.Item).WithMany(p => p.InventoryBalances)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bal_Item");

            entity.HasOne(d => d.Location).WithMany(p => p.InventoryBalances)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bal_Location");
        });

        modelBuilder.Entity<InventoryLot>(entity =>
        {
            entity.HasKey(e => e.InventoryLotId).HasName("PK__Inventor__9B63ED64880C3807");

            entity.ToTable("InventoryLot");

            entity.Property(e => e.InventoryLotId).HasColumnName("InventoryLotID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.ManufacturerId).HasColumnName("ManufacturerID");

            entity.HasOne(d => d.Item).WithMany(p => p.InventoryLots)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lot_Item");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.InventoryLots)
                .HasForeignKey(d => d.ManufacturerId)
                .HasConstraintName("FK_Lot_Manufacturer");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.InventoryLots)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lot_Status");
        });

        modelBuilder.Entity<InventoryLotStatus>(entity =>
        {
            entity.HasKey(e => e.InventoryLotStatusId).HasName("PK__Inventor__FC4BC9855C833D0B");

            entity.ToTable("InventoryLotStatus");

            entity.Property(e => e.InventoryLotStatusId).HasColumnName("InventoryLotStatusID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<InventoryReport>(entity =>
        {
            entity.HasKey(e => e.InventoryReportId).HasName("PK__Inventor__66568F5D3A178055");

            entity.ToTable("InventoryReport");

            entity.Property(e => e.InventoryReportId).HasColumnName("InventoryReportID");
            entity.Property(e => e.GeneratedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.ScopeNavigation).WithMany(p => p.InventoryReports)
                .HasForeignKey(d => d.Scope)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IRep_Scope");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__Item__727E83EB872C4014");

            entity.ToTable("Item");

            entity.HasIndex(e => e.Barcode, "UQ__Item__177800D3ABA58A81").IsUnique();

            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.Barcode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ConversionToEach)
                .HasDefaultValue(1.0000m)
                .HasColumnType("decimal(10, 4)");
            entity.Property(e => e.DrugId).HasColumnName("DrugID");
            entity.Property(e => e.Status).HasDefaultValue(true);

            entity.HasOne(d => d.Drug).WithMany(p => p.Items)
                .HasForeignKey(d => d.DrugId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Item_Drug");

            entity.HasOne(d => d.UoMNavigation).WithMany(p => p.Items)
                .HasForeignKey(d => d.UoM)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Item_UoM");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PK__Location__E7FEA47790033C34");

            entity.ToTable("Location");

            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.LocationTypeId).HasColumnName("LocationTypeID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ParentLocationId).HasColumnName("ParentLocationID");
            entity.Property(e => e.StatusId)
                .HasDefaultValue(true)
                .HasColumnName("StatusID");

            entity.HasOne(d => d.LocationType).WithMany(p => p.Locations)
                .HasForeignKey(d => d.LocationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Location_Type");

            entity.HasOne(d => d.ParentLocation).WithMany(p => p.InverseParentLocation)
                .HasForeignKey(d => d.ParentLocationId)
                .HasConstraintName("FK_Location_Parent");
        });

        modelBuilder.Entity<LocationType>(entity =>
        {
            entity.HasKey(e => e.LocationTypeId).HasName("PK__Location__737D32D921D4DA80");

            entity.ToTable("LocationType");

            entity.Property(e => e.LocationTypeId).HasColumnName("LocationTypeID");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.ManufacturerId).HasName("PK__Manufact__357E5CA1EAC1A2C9");

            entity.ToTable("Manufacturer");

            entity.HasIndex(e => e.ManufacturerName, "UQ__Manufact__3B9CDE2EBD20D788").IsUnique();

            entity.Property(e => e.ManufacturerId).HasColumnName("ManufacturerID");
            entity.Property(e => e.ManufacturerName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E32E9F88E8A");

            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.Category)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Noti_Cat");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Noti_Status");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Noti_User");
        });

        modelBuilder.Entity<NotificationCategory>(entity =>
        {
            entity.HasKey(e => e.NotificationCategoryId).HasName("PK__Notifica__C7B5518BA7D09E95");

            entity.ToTable("NotificationCategory");

            entity.Property(e => e.NotificationCategoryId).HasColumnName("NotificationCategoryID");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NotificationStatus>(entity =>
        {
            entity.HasKey(e => e.NotificationStatusId).HasName("PK__Notifica__6564229346816A1F");

            entity.ToTable("NotificationStatus");

            entity.Property(e => e.NotificationStatusId).HasColumnName("NotificationStatusID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PurchaseItem>(entity =>
        {
            entity.HasKey(e => e.PurchaseItemId).HasName("PK__Purchase__B48BB6A7C8E895FD");

            entity.ToTable("PurchaseItem");

            entity.Property(e => e.PurchaseItemId).HasColumnName("PurchaseItemID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.PurchaseOrderId).HasColumnName("PurchaseOrderID");
            entity.Property(e => e.TaxPct).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Item).WithMany(p => p.PurchaseItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PI_Item");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.PurchaseItems)
                .HasForeignKey(d => d.PurchaseOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PI_Order");
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.PurchaseOrderId).HasName("PK__Purchase__036BACA487CC0D6E");

            entity.ToTable("PurchaseOrder");

            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.PurchaseOrderStatusId).HasColumnName("PurchaseOrderStatusID");
            entity.Property(e => e.VendorId).HasColumnName("VendorID");

            entity.HasOne(d => d.Location).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PO_Location");

            entity.HasOne(d => d.PurchaseOrderStatus).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.PurchaseOrderStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PO_Status");

            entity.HasOne(d => d.Vendor).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.VendorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PO_Vendor");
        });

        modelBuilder.Entity<PurchaseOrderStatus>(entity =>
        {
            entity.HasKey(e => e.PurchaseOrderStatusId).HasName("PK__Purchase__32256B4B08F6A53C");

            entity.ToTable("PurchaseOrderStatus");

            entity.Property(e => e.PurchaseOrderStatusId).HasColumnName("PurchaseOrderStatusID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<QuarantaineAction>(entity =>
        {
            entity.HasKey(e => e.QuarantaineActionId).HasName("PK__Quaranta__A3C54BCFE81D1161");

            entity.ToTable("QuarantaineAction");

            entity.Property(e => e.QuarantaineActionId).HasColumnName("QuarantaineActionID");
            entity.Property(e => e.InventoryLotId).HasColumnName("InventoryLotID");
            entity.Property(e => e.QuarantineDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReleasedDate).HasColumnType("datetime");

            entity.HasOne(d => d.InventoryLot).WithMany(p => p.QuarantaineActions)
                .HasForeignKey(d => d.InventoryLotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QAct_Lot");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.QuarantaineActions)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QAct_Status");
        });

        modelBuilder.Entity<QuarantineStatus>(entity =>
        {
            entity.HasKey(e => e.QuarantineStatusId).HasName("PK__Quaranti__B3FDD81C67F33580");

            entity.ToTable("QuarantineStatus");

            entity.HasIndex(e => e.Status, "UQ__Quaranti__3A15923F5B416EBB").IsUnique();

            entity.Property(e => e.QuarantineStatusId).HasColumnName("QuarantineStatusID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Reason>(entity =>
        {
            entity.HasKey(e => e.ReasonId).HasName("PK__Reason__A4F8C0C723E12CD8");

            entity.ToTable("Reason");

            entity.Property(e => e.ReasonId).HasColumnName("ReasonID");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RecallAction>(entity =>
        {
            entity.HasKey(e => e.RecallActionId).HasName("PK__RecallAc__00E8DEB67B36D58F");

            entity.ToTable("RecallAction");

            entity.HasIndex(e => e.Action, "UQ__RecallAc__68BAC84C7356C065").IsUnique();

            entity.Property(e => e.RecallActionId).HasColumnName("RecallActionID");
            entity.Property(e => e.Action)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RecallNotice>(entity =>
        {
            entity.HasKey(e => e.RecallNoticeId).HasName("PK__RecallNo__9E9450E0B9E55AEE");

            entity.ToTable("RecallNotice");

            entity.Property(e => e.RecallNoticeId).HasColumnName("RecallNoticeID");
            entity.Property(e => e.DrugId).HasColumnName("DrugID");
            entity.Property(e => e.NoticeDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue(true);

            entity.HasOne(d => d.ActionNavigation).WithMany(p => p.RecallNotices)
                .HasForeignKey(d => d.Action)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rec_Action");

            entity.HasOne(d => d.Drug).WithMany(p => p.RecallNotices)
                .HasForeignKey(d => d.DrugId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Rec_Drug");
        });

        modelBuilder.Entity<ReplenishmentRequest>(entity =>
        {
            entity.HasKey(e => e.ReplenishmentRequestId).HasName("PK__Replenis__0BC6312FE9BEC0D4");

            entity.ToTable("ReplenishmentRequest");

            entity.Property(e => e.ReplenishmentRequestId).HasColumnName("ReplenishmentRequestID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.RuleId).HasColumnName("RuleID");

            entity.HasOne(d => d.Item).WithMany(p => p.ReplenishmentRequests)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RReq_Item");

            entity.HasOne(d => d.Location).WithMany(p => p.ReplenishmentRequests)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RReq_Loc");

            entity.HasOne(d => d.Rule).WithMany(p => p.ReplenishmentRequests)
                .HasForeignKey(d => d.RuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RReq_Rule");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.ReplenishmentRequests)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RReq_Status");
        });

        modelBuilder.Entity<ReplenishmentRule>(entity =>
        {
            entity.HasKey(e => e.ReplenishmentRuleId).HasName("PK__Replenis__E708E81F619D6FAE");

            entity.ToTable("ReplenishmentRule");

            entity.Property(e => e.ReplenishmentRuleId).HasColumnName("ReplenishmentRuleID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.ReviewCycle).HasDefaultValue(true);

            entity.HasOne(d => d.Item).WithMany(p => p.ReplenishmentRules)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Repl_Item");

            entity.HasOne(d => d.Location).WithMany(p => p.ReplenishmentRules)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Repl_Loc");
        });

        modelBuilder.Entity<ReplenishmentStatus>(entity =>
        {
            entity.HasKey(e => e.ReplenishmentStatusId).HasName("PK__Replenis__BB9872623408A29A");

            entity.ToTable("ReplenishmentStatus");

            entity.HasIndex(e => e.Status, "UQ__Replenis__3A15923FF42FBE4D").IsUnique();

            entity.Property(e => e.ReplenishmentStatusId).HasColumnName("ReplenishmentStatusID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ReportScope>(entity =>
        {
            entity.HasKey(e => e.ReportScopeId).HasName("PK__ReportSc__C98142E622F45AD4");

            entity.ToTable("ReportScope");

            entity.HasIndex(e => e.Scope, "UQ__ReportSc__E028DBCCCE8A46FD").IsUnique();

            entity.Property(e => e.ReportScopeId).HasColumnName("ReportScopeID");
            entity.Property(e => e.Scope)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StockAdjustment>(entity =>
        {
            entity.HasKey(e => e.StockAdjustmentId).HasName("PK__StockAdj__0A9711D3F0A9F956");

            entity.ToTable("StockAdjustment");

            entity.Property(e => e.StockAdjustmentId).HasColumnName("StockAdjustmentID");
            entity.Property(e => e.InventoryLotId).HasColumnName("InventoryLotID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.PostedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.StockAdjustments)
                .HasForeignKey(d => d.ApprovedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SAdj_User");

            entity.HasOne(d => d.InventoryLot).WithMany(p => p.StockAdjustments)
                .HasForeignKey(d => d.InventoryLotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SAdj_Lot");

            entity.HasOne(d => d.Item).WithMany(p => p.StockAdjustments)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SAdj_Item");

            entity.HasOne(d => d.Location).WithMany(p => p.StockAdjustments)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SAdj_Loc");

            entity.HasOne(d => d.ReasonCodeNavigation).WithMany(p => p.StockAdjustments)
                .HasForeignKey(d => d.ReasonCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SAdj_Reason");
        });

        modelBuilder.Entity<StockCount>(entity =>
        {
            entity.HasKey(e => e.StockCountId).HasName("PK__StockCou__489231A3A53AC47F");

            entity.ToTable("StockCount");

            entity.Property(e => e.StockCountId).HasColumnName("StockCountID");
            entity.Property(e => e.LocationId).HasColumnName("LocationID");

            entity.HasOne(d => d.CycleNavigation).WithMany(p => p.StockCounts)
                .HasForeignKey(d => d.Cycle)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SCount_Cycle");

            entity.HasOne(d => d.Location).WithMany(p => p.StockCounts)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SCount_Loc");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.StockCounts)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SCount_Status");
        });

        modelBuilder.Entity<StockCountItem>(entity =>
        {
            entity.HasKey(e => e.StockCountItemId).HasName("PK__StockCou__1D4AC5E9C7804FE0");

            entity.ToTable("StockCountItem");

            entity.Property(e => e.StockCountItemId).HasColumnName("StockCountItemID");
            entity.Property(e => e.BinId).HasColumnName("BinID");
            entity.Property(e => e.CountId).HasColumnName("CountID");
            entity.Property(e => e.InventoryLotId).HasColumnName("InventoryLotID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");

            entity.HasOne(d => d.Bin).WithMany(p => p.StockCountItems)
                .HasForeignKey(d => d.BinId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SCItem_Bin");

            entity.HasOne(d => d.Count).WithMany(p => p.StockCountItems)
                .HasForeignKey(d => d.CountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SCItem_Count");

            entity.HasOne(d => d.InventoryLot).WithMany(p => p.StockCountItems)
                .HasForeignKey(d => d.InventoryLotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SCItem_Lot");

            entity.HasOne(d => d.Item).WithMany(p => p.StockCountItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SCItem_Item");

            entity.HasOne(d => d.ReasonCodeNavigation).WithMany(p => p.StockCountItems)
                .HasForeignKey(d => d.ReasonCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SCItem_Reason");
        });

        modelBuilder.Entity<StockCountStatus>(entity =>
        {
            entity.HasKey(e => e.StockCountStatusId).HasName("PK__StockCou__F1C65D4DD10B8F75");

            entity.ToTable("StockCountStatus");

            entity.HasIndex(e => e.Status, "UQ__StockCou__3A15923FC633A56E").IsUnique();

            entity.Property(e => e.StockCountStatusId).HasColumnName("StockCountStatusID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StockTransition>(entity =>
        {
            entity.HasKey(e => e.StockTransitionId).HasName("PK__StockTra__B0415204B845A2F7");

            entity.ToTable("StockTransition");

            entity.Property(e => e.StockTransitionId).HasColumnName("StockTransitionID");
            entity.Property(e => e.BinId).HasColumnName("BinID");
            entity.Property(e => e.InventoryLotId).HasColumnName("InventoryLotID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.ReferenceId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ReferenceID");
            entity.Property(e => e.StockTransitionTypeDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Bin).WithMany(p => p.StockTransitions)
                .HasForeignKey(d => d.BinId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ST_Bin");

            entity.HasOne(d => d.InventoryLot).WithMany(p => p.StockTransitions)
                .HasForeignKey(d => d.InventoryLotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ST_Lot");

            entity.HasOne(d => d.Item).WithMany(p => p.StockTransitions)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ST_Item");

            entity.HasOne(d => d.Location).WithMany(p => p.StockTransitions)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ST_Loc");

            entity.HasOne(d => d.PerformedByNavigation).WithMany(p => p.StockTransitions)
                .HasForeignKey(d => d.PerformedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ST_User");

            entity.HasOne(d => d.StockTransitionTypeNavigation).WithMany(p => p.StockTransitions)
                .HasForeignKey(d => d.StockTransitionType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ST_Type");
        });

        modelBuilder.Entity<StockTransitionType>(entity =>
        {
            entity.HasKey(e => e.StockTransitionTypeId).HasName("PK__StockTra__AEF2064BC14A3126");

            entity.ToTable("StockTransitionType");

            entity.Property(e => e.StockTransitionTypeId)
                .ValueGeneratedNever()
                .HasColumnName("StockTransitionTypeID");
            entity.Property(e => e.TransitionType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__Task__7C6949D1679ED75A");

            entity.ToTable("Task");

            entity.Property(e => e.TaskId).HasColumnName("TaskID");
            entity.Property(e => e.GoodsReceiptItemId).HasColumnName("GoodsReceiptItemID");
            entity.Property(e => e.TargetBinId).HasColumnName("TargetBinID");

            entity.HasOne(d => d.GoodsReceiptItem).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.GoodsReceiptItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Task_GRI");

            entity.HasOne(d => d.TargetBin).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.TargetBinId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Task_Bin");
        });

        modelBuilder.Entity<TransferItem>(entity =>
        {
            entity.HasKey(e => e.TransferItemId).HasName("PK__Transfer__AAC7775DBEAB5858");

            entity.ToTable("TransferItem");

            entity.Property(e => e.TransferItemId).HasColumnName("TransferItemID");
            entity.Property(e => e.InventoryLotId).HasColumnName("InventoryLotID");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.TransferOrderId).HasColumnName("TransferOrderID");

            entity.HasOne(d => d.InventoryLot).WithMany(p => p.TransferItems)
                .HasForeignKey(d => d.InventoryLotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TItem_Lot");

            entity.HasOne(d => d.Item).WithMany(p => p.TransferItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TItem_Item");

            entity.HasOne(d => d.TransferOrder).WithMany(p => p.TransferItems)
                .HasForeignKey(d => d.TransferOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TItem_Order");
        });

        modelBuilder.Entity<TransferOrder>(entity =>
        {
            entity.HasKey(e => e.TransferOrderId).HasName("PK__Transfer__4AEC45EEDD0A1002");

            entity.ToTable("TransferOrder");

            entity.Property(e => e.TransferOrderId).HasColumnName("TransferOrderID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FromLocationId).HasColumnName("FromLocationID");
            entity.Property(e => e.ToLocationId).HasColumnName("ToLocationID");

            entity.HasOne(d => d.FromLocation).WithMany(p => p.TransferOrderFromLocations)
                .HasForeignKey(d => d.FromLocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TO_FromLoc");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.TransferOrders)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TO_Status");

            entity.HasOne(d => d.ToLocation).WithMany(p => p.TransferOrderToLocations)
                .HasForeignKey(d => d.ToLocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TO_ToLoc");
        });

        modelBuilder.Entity<TransferOrderStatus>(entity =>
        {
            entity.HasKey(e => e.TransferOrderStatusId).HasName("PK__Transfer__3C8A7429736C3A44");

            entity.ToTable("TransferOrderStatus");

            entity.HasIndex(e => e.Status, "UQ__Transfer__3A15923F7B58D038").IsUnique();

            entity.Property(e => e.TransferOrderStatusId).HasColumnName("TransferOrderStatusID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UoM>(entity =>
        {
            entity.HasKey(e => e.UoMid).HasName("PK__UoM__8286E93C59BA2D37");

            entity.ToTable("UoM");

            entity.HasIndex(e => e.Code, "UQ__UoM__A25C5AA7684346C0").IsUnique();

            entity.Property(e => e.UoMid).HasColumnName("UoMID");
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCACA9FE520C");

            entity.ToTable("User");

            entity.HasIndex(e => e.Username, "UQ__User__536C85E4091755AB").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__User__A9D105342711B7B7").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.StatusId)
                .HasDefaultValue(true)
                .HasColumnName("StatusID");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__UserRole__8AFACE3A25A33A52");

            entity.ToTable("UserRole");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Vendor>(entity =>
        {
            entity.HasKey(e => e.VendorId).HasName("PK__Vendor__FC8618D326A300FD");

            entity.ToTable("Vendor");

            entity.Property(e => e.VendorId).HasColumnName("VendorID");
            entity.Property(e => e.ContactInfo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.StatusId)
                .HasDefaultValue(true)
                .HasColumnName("StatusID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
