-- MODULE 4: GOODS RECEIPT & PUT-AWAY 

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'GoodsReceiptStatus')
BEGIN
    CREATE TABLE GoodsReceiptStatus (
        GoodsReceiptStatusID INT IDENTITY(1,1) PRIMARY KEY,
        [Status] VARCHAR(50) NOT NULL
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'GoodsReciept')
BEGIN
    CREATE TABLE GoodsReciept (
        GoodsRecieptId INT IDENTITY(1,1) PRIMARY KEY,
        PurchaseOrderID INT NOT NULL,
        ReceivedDate DATETIME NOT NULL DEFAULT GETDATE(),
        [Status] INT NOT NULL,
        CONSTRAINT FK_GR_PO FOREIGN KEY (PurchaseOrderID) REFERENCES PurchaseOrder(PurchaseOrderId),
        CONSTRAINT FK_GR_Status FOREIGN KEY ([Status]) REFERENCES GoodsReceiptStatus(GoodsReceiptStatusID)
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'GoodsReceiptItem')
BEGIN
    CREATE TABLE GoodsReceiptItem (
        GoodsReceiptItemID INT IDENTITY(1,1) PRIMARY KEY,
        GoodsReceiptID INT NOT NULL,
        PurchaseOrderItemID INT NOT NULL,
        ItemID INT NOT NULL,
        BatchNumber