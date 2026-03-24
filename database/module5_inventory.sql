-- MODULE 5: INVENTORY & LOT MANAGEMENT 

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'InventoryLotStatus')
BEGIN
    CREATE TABLE InventoryLotStatus (InventoryLotStatusID INT IDENTITY(1,1) PRIMARY KEY, [Status] VARCHAR(50) NOT NULL);
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'InventoryLot')
BEGIN
    CREATE TABLE InventoryLot (
        InventoryLotID INT IDENTITY(1,1) PRIMARY KEY,
        ItemID INT NOT NULL,
        BatchNumber INT NOT NULL,
        ExpiryDate DATE NOT NULL,
        ManufacturerID INT,
        [Status] INT NOT NULL,
        CONSTRAINT FK_Lot_Item FOREIGN KEY (ItemID) REFERENCES Item(ItemID),
        CONSTRAINT FK_Lot_Manufacturer FOREIGN KEY (ManufacturerID) REFERENCES Manufacturer(ManufacturerID),
        CONSTRAINT FK_Lot_Status FOREIGN KEY ([Status]) REFERENCES InventoryLotStatus(InventoryLotStatusID)
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'InventoryBalance')
BEGIN
    CREATE TABLE InventoryBalance (
        InventoryBalanceID INT IDENTITY(1,1) PRIMARY KEY,
        LocationID INT NOT NULL,
        BinID INT NOT NULL,
        ItemID INT NOT NULL,
        InventoryLotID INT NOT NULL,
        QuantityOnHand INT NOT NULL DEFAULT 0,
        ReservedQty INT NOT NULL DEFAULT 0,
        CONSTRAINT CHK_QuantityOnHand CHECK (QuantityOnHand >= 0),
        CONSTRAINT CHK_ReservedQty CHECK (ReservedQty >= 0),
        CONSTRAINT FK_Bal_Location FOREIGN KEY (LocationID) REFERENCES [Location](LocationID),
        CONSTRAINT FK_Bal_Bin FOREIGN KEY (BinID) REFERENCES Bin(BinID),
        CONSTRAINT FK_Bal_Item FOREIGN KEY (ItemID) REFERENCES Item(ItemID),
        CONSTRAINT FK_Bal_Lot FOREIGN KEY (InventoryLotID) REFERENCES InventoryLot(InventoryLotID)
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ExpiryWatch')
BEGIN
    CREATE TABLE ExpiryWatch (
        ExpiryWatchID INT IDENTITY(1,1) PRIMARY KEY,
        InventoryLotID INT NOT NULL,
        DaysToExpire INT NOT NULL,
        FlagDate DATE NOT NULL,
        [Status] BIT NOT NULL DEFAULT 1,
        CONSTRAINT FK_Expiry_Lot FOREIGN KEY (InventoryLotID) REFERENCES InventoryLot(InventoryLotID)
    );
END