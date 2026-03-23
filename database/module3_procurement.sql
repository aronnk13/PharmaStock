-- MODULE 3: PROCUREMENT & VENDOR MANAGEMENT 

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Manufacturer')
BEGIN
    CREATE TABLE Manufacturer (
        ManufacturerID INT IDENTITY(1,1) PRIMARY KEY,
        ManufacturerName VARCHAR(100) NOT NULL UNIQUE
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PurchaseOrderStatus')
BEGIN
    CREATE TABLE PurchaseOrderStatus (
        PurchaseOrderStatusID INT IDENTITY(1,1) PRIMARY KEY,
        [Status] VARCHAR(50) NOT NULL
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Vendor')
BEGIN
    CREATE TABLE Vendor (
        VendorID INT IDENTITY(1,1) PRIMARY KEY,
        Name VARCHAR(100) NOT NULL,
        ContactInfo VARCHAR(255),
        Rating INT,
        StatusID BIT DEFAULT 1,
        Email VARCHAR(100),
        Phone VARCHAR(20)
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PurchaseOrder')
BEGIN
    CREATE TABLE PurchaseOrder (
        PurchaseOrderId INT IDENTITY(1,1) PRIMARY KEY,
        VendorID INT NOT NULL,
        LocationID INT NOT NULL,
        OrderDate DATE NOT NULL,
        ExpectedDate DATE NOT NULL,
        PurchaseOrderStatusID INT NOT NULL,
        CONSTRAINT FK_PO_Vendor FOREIGN KEY (VendorID) REFERENCES Vendor(VendorID),
        CONSTRAINT FK_PO_Location FOREIGN KEY (LocationID) REFERENCES [Location](LocationID),
        CONSTRAINT FK_PO_Status FOREIGN KEY (PurchaseOrderStatusID) REFERENCES PurchaseOrderStatus(PurchaseOrderStatusID)
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PurchaseItem')
BEGIN
    CREATE TABLE PurchaseItem (
        PurchaseItemID INT IDENTITY(1,1) PRIMARY KEY,
        PurchaseOrderID INT NOT NULL,
        ItemID INT NOT NULL,
        OrderedQty INT NOT NULL,
        UnitPrice DECIMAL(10,2) NOT NULL,
        TaxPct DECIMAL(5,2) NOT NULL,
        CONSTRAINT FK_PI_Order FOREIGN KEY (PurchaseOrderID) REFERENCES PurchaseOrder(PurchaseOrderId),
        CONSTRAINT FK_PI_Item FOREIGN KEY (ItemID) REFERENCES Item(ItemID)
    );
END
