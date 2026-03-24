--Module 6: Stock Operations & Movements

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'StockTransitionType')
BEGIN
    CREATE TABLE StockTransitionType (StockTransitionTypeID INT PRIMARY KEY, TransitionType VARCHAR(50) NOT NULL);
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ReplenishmentStatus')
BEGIN
    CREATE TABLE ReplenishmentStatus (ReplenishmentStatusID INT IDENTITY(1,1) PRIMARY KEY, [Status] VARCHAR(50) NOT NULL UNIQUE);
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TransferOrderStatus')
BEGIN
    CREATE TABLE TransferOrderStatus (TransferOrderStatusID INT IDENTITY(1,1) PRIMARY KEY, [Status] VARCHAR(50) NOT NULL UNIQUE);
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TransferOrder')
BEGIN
    CREATE TABLE TransferOrder (
        TransferOrderID INT IDENTITY(1,1) PRIMARY KEY,
        FromLocationID INT NOT NULL,
        ToLocationID INT NOT NULL,
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
        [Status] INT NOT NULL,
        CONSTRAINT FK_TO_FromLoc FOREIGN KEY (FromLocationID) REFERENCES [Location](LocationID),
        CONSTRAINT FK_TO_ToLoc FOREIGN KEY (ToLocationID) REFERENCES [Location](LocationID),
        CONSTRAINT FK_TO_Status FOREIGN KEY ([Status]) REFERENCES TransferOrderStatus(TransferOrderStatusID)
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'TransferItem')
BEGIN
    CREATE TABLE TransferItem (
        TransferItemID INT IDENTITY(1,1) PRIMARY KEY,
        TransferOrderID INT NOT NULL,
        ItemID INT NOT NULL,
        InventoryLotID INT NOT NULL,
        Quantity INT NOT NULL,
        CONSTRAINT CHK_TItem_Qty CHECK (Quantity >= 0),
        CONSTRAINT FK_TItem_Order FOREIGN KEY (TransferOrderID) REFERENCES TransferOrder(TransferOrderID),
        CONSTRAINT FK_TItem_Item FOREIGN KEY (ItemID) REFERENCES Item(ItemID),
        CONSTRAINT FK_TItem_Lot FOREIGN KEY (InventoryLotID) REFERENCES InventoryLot(InventoryLotID)
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'DestinationType')
BEGIN
    CREATE TABLE DestinationType (DestinationTypeID INT IDENTITY(1,1) PRIMARY KEY, [Type] VARCHAR(50) NOT NULL);
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'DispenseRef')
BEGIN
    CREATE TABLE DispenseRef (
        DispenseRefID INT IDENTITY(1,1) PRIMARY KEY,
        LocationID INT NOT NULL,
        ItemID INT NOT NULL,
        InventoryLotID INT NOT NULL,
        Quantity INT NOT NULL,
        DispenseDate DATETIME NOT NULL DEFAULT GETDATE(),
        [Status] BIT NOT NULL DEFAULT 1,
        Destination INT NOT NULL,
        CONSTRAINT FK_Disp_Loc FOREIGN KEY (LocationID) REFERENCES [Location](LocationID),
        CONSTRAINT FK_Disp_Item FOREIGN KEY (ItemID) REFERENCES Item(ItemID),
        CONSTRAINT FK_Disp_Lot FOREIGN KEY (InventoryLotID) REFERENCES InventoryLot(InventoryLotID),
        CONSTRAINT FK_Disp_Dest FOREIGN KEY (Destination) REFERENCES DestinationType(DestinationTypeID)
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ReplenishmentRule')
BEGIN
    CREATE TABLE ReplenishmentRule (
        ReplenishmentRuleID INT IDENTITY(1,1) PRIMARY KEY,
        LocationID INT NOT NULL,
        ItemID INT NOT NULL,
        MinLevel INT NOT NULL,
        MaxLevel INT NOT NULL,
        ParLevel INT NOT NULL,
        ReviewCycle BIT NOT NULL DEFAULT 1,
        CONSTRAINT FK_Repl_Loc FOREIGN KEY (LocationID) REFERENCES [Location](LocationID),
        CONSTRAINT FK_Repl_Item FOREIGN KEY (ItemID) REFERENCES Item(ItemID)
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ReplenishmentRequest')
BEGIN
    CREATE TABLE ReplenishmentRequest (
        ReplenishmentRequestID INT IDENTITY(1,1) PRIMARY KEY,
        LocationID INT NOT NULL,
        ItemID INT NOT NULL,
        SuggestedQty INT NOT NULL,
        CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
        RuleID INT NOT NULL,
        [Status] INT NOT NULL,
        CONSTRAINT FK_RReq_Loc FOREIGN KEY (LocationID) REFERENCES [Location](LocationID),
        CONSTRAINT FK_RReq_Item FOREIGN KEY (ItemID) REFERENCES Item(ItemID),
        CONSTRAINT FK_RReq_Rule FOREIGN KEY (RuleID) REFERENCES ReplenishmentRule(ReplenishmentRuleID),
        CONSTRAINT FK_RReq_Status FOREIGN KEY ([Status]) REFERENCES ReplenishmentStatus(ReplenishmentStatusID)
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'StockTransition')
BEGIN
    CREATE TABLE StockTransition (
        StockTransitionID INT IDENTITY(1,1) PRIMARY KEY,
        LocationID INT NOT NULL,
        BinID INT NOT NULL,
        ItemID INT NOT NULL,
        InventoryLotID INT NOT NULL,
        StockTransitionType INT NOT NULL,
        Quantity INT NOT NULL, 
        StockTransitionTypeDate DATETIME NOT NULL DEFAULT GETDATE(),
        ReferenceID VARCHAR(50), 
        Notes NVARCHAR(MAX),
        PerformedBy INT NOT NULL,
        CONSTRAINT FK_ST_Loc FOREIGN KEY (LocationID) REFERENCES [Location](LocationID),
        CONSTRAINT FK_ST_Bin FOREIGN KEY (BinID) REFERENCES Bin(BinID),
        CONSTRAINT FK_ST_Item FOREIGN KEY (ItemID) REFERENCES Item(ItemID),
        CONSTRAINT FK_ST_Lot FOREIGN KEY (InventoryLotID) REFERENCES InventoryLot(InventoryLotID),
        CONSTRAINT FK_ST_Type FOREIGN KEY (StockTransitionType) REFERENCES StockTransitionType(StockTransitionTypeID),
        CONSTRAINT FK_ST_User FOREIGN KEY (PerformedBy) REFERENCES [User](UserID)
    );
END