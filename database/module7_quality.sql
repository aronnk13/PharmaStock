-- MODULE 7: QUALITY CONTROL & COMPLIANCE 

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'StockCountStatus')
BEGIN
    CREATE TABLE StockCountStatus (
        StockCountStatusID INT IDENTITY(1,1) PRIMARY KEY,
        [Status] VARCHAR(50) NOT NULL UNIQUE 
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'CountCycle')
BEGIN
    CREATE TABLE CountCycle (
        CountCycleID INT IDENTITY(1,1) PRIMARY KEY,
        Cycle VARCHAR(50) NOT NULL UNIQUE 
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'RecallAction')
BEGIN
    CREATE TABLE RecallAction (
        RecallActionID INT IDENTITY(1,1) PRIMARY KEY,
        [Action] VARCHAR(50) NOT NULL UNIQUE
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'QuarantineStatus')
BEGIN
    CREATE TABLE QuarantineStatus (
        QuarantineStatusID INT IDENTITY(1,1) PRIMARY KEY,
        [Status] VARCHAR(50) NOT NULL UNIQUE 
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ReportScope')
BEGIN
    CREATE TABLE ReportScope (
        ReportScopeID INT IDENTITY(1,1) PRIMARY KEY,
        Scope VARCHAR(50) NOT NULL UNIQUE 
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'StockCount')
BEGIN
    CREATE TABLE StockCount (
        StockCountID INT IDENTITY(1,1) PRIMARY KEY,
        LocationID INT NOT NULL,
        Cycle INT NOT NULL,
        ScheduledDate DATE NOT NULL,
        [Status] INT NOT NULL,
        CONSTRAINT FK_SC_Loc FOREIGN KEY (LocationID) REFERENCES [Location](LocationID),
        CONSTRAINT FK_SC_Cycle FOREIGN KEY (Cycle) REFERENCES CountCycle(CountCycleID),
        CONSTRAINT FK_SC_Status FOREIGN KEY ([Status]) REFERENCES StockCountStatus(StockCountStatusID)
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'StockCountItem')
BEGIN
    CREATE TABLE StockCountItem (
        StockCountItemID INT IDENTITY(1,1) PRIMARY KEY,
        CountID INT NOT NULL,
        BinID INT NOT NULL,
        ItemID INT NOT NULL,
        InventoryLotID INT NOT NULL,
        SystemQty INT NOT NULL,
        CountedQty INT NOT NULL,
        Variance INT NOT NULL,
        ReasonCode INT NOT NULL,
        CONSTRAINT FK_SCI_Count FOREIGN KEY (CountID) REFERENCES StockCount(StockCountID),
        CONSTRAINT FK_SCI_Bin FOREIGN KEY (BinID) REFERENCES Bin(BinID),
        CONSTRAINT FK_SCI_Item FOREIGN KEY (ItemID) REFERENCES Item(ItemID),
        CONSTRAINT FK_SCI_Lot FOREIGN KEY (InventoryLotID) REFERENCES InventoryLot(InventoryLotID),
        CONSTRAINT FK_SCI_Reason FOREIGN KEY (ReasonCode) REFERENCES Reason(ReasonID)
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'QuarantaineAction')
BEGIN
    CREATE TABLE QuarantaineAction (
        QuarantaineActionID INT IDENTITY(1,1) PRIMARY KEY,
        InventoryLotID INT NOT NULL,
        QuarantineDate DATETIME NOT NULL DEFAULT GETDATE(),
        Reason NVARCHAR(MAX) NOT NULL,
        ReleasedDate DATETIME,
        [Status] INT NOT NULL,
        CONSTRAINT FK_QA_Lot FOREIGN KEY (InventoryLotID) REFERENCES InventoryLot(InventoryLotID),
        CONSTRAINT FK_QA_Status FOREIGN KEY ([Status]) REFERENCES QuarantineStatus(QuarantineStatusID)
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'RecallNotice')
BEGIN
    CREATE TABLE RecallNotice (
        RecallNoticeID INT IDENTITY(1,1) PRIMARY KEY,
        DrugID INT NOT NULL,
        NoticeDate DATE NOT NULL DEFAULT GETDATE(),
        Reason NVARCHAR(MAX),
        [Action] INT NOT NULL,
        [Status] BIT NOT NULL DEFAULT 1,
        CONSTRAINT FK_RN_Drug FOREIGN KEY (DrugID) REFERENCES Drug(DrugID),
        CONSTRAINT FK_RN_Action FOREIGN KEY ([Action]) REFERENCES RecallAction(RecallActionID)
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'InventoryReport')
BEGIN
    CREATE TABLE InventoryReport (
        InventoryReportID INT IDENTITY(1,1) PRIMARY KEY,
        Scope INT NOT NULL,
        Metrics BIT NOT NULL, 
        GeneratedDate DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_IR_Scope FOREIGN KEY (Scope) REFERENCES ReportScope(ReportScopeID)
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Notification')
BEGIN
    CREATE TABLE Notification (
        NotificationID INT IDENTITY(1,1) PRIMARY KEY,
        UserID INT NOT NULL,
        [Message] NVARCHAR(MAX) NOT NULL,
        Category INT NOT NULL, 
        [Status] INT NOT NULL, 
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Notif_User FOREIGN KEY (UserID) REFERENCES [User](UserID),
        CONSTRAINT FK_Notif_Cat FOREIGN KEY (Category) REFERENCES NotificationCategory(NotificationCategoryID),
        CONSTRAINT FK_Notif_Status FOREIGN KEY ([Status]) REFERENCES NotificationStatus(NotificationStatusID)
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'StockAdjustment')
BEGIN
    CREATE TABLE StockAdjustment (
        StockAdjustmentID INT IDENTITY(1,1) PRIMARY KEY,
        LocationID INT NOT NULL,
        ItemID INT NOT NULL,
        InventoryLotID INT NOT NULL,
        QuantityDelta INT NOT NULL,
        ReasonCode INT NOT NULL,
        ApprovedBy INT NOT NULL,
        PostedDate DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Adj_Loc FOREIGN KEY (LocationID) REFERENCES [Location](LocationID),
        CONSTRAINT FK_Adj_Item FOREIGN KEY (ItemID) REFERENCES Item(ItemID),
        CONSTRAINT FK_Adj_Lot FOREIGN KEY (InventoryLotID) REFERENCES InventoryLot(InventoryLotID),
        CONSTRAINT FK_Adj_Reason FOREIGN KEY (ReasonCode) REFERENCES Reason(ReasonID),
        CONSTRAINT FK_Adj_User FOREIGN KEY (ApprovedBy) REFERENCES [User](UserID)
    );
END
