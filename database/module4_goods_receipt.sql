--MODULE 4 Goods Receipt & Put-away
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
        BatchNumber INT NOT NULL,
        ExpiryDate DATE NOT NULL,
        ReceivedQty INT NOT NULL,
        AcceptedQty INT NOT NULL,
        RejectedQty INT NOT NULL,
        Reason VARCHAR(250), 
        CONSTRAINT FK_GRI_GR FOREIGN KEY (GoodsReceiptID) REFERENCES GoodsReciept(GoodsRecieptId),
        CONSTRAINT FK_GRI_PI FOREIGN KEY (PurchaseOrderItemID) REFERENCES PurchaseItem(PurchaseItemID),
        CONSTRAINT FK_GRI_Item FOREIGN KEY (ItemID) REFERENCES Item(ItemID)
    );
END

-- Run these lines in your local PharmaStock DB
--1.
ALTER TABLE GoodsReciept
ADD ReceivedBy INT NULL;
--2.
UPDATE GoodsReciept
SET ReceivedBy = 1;
--3.
ALTER TABLE GoodsReciept
ADD CONSTRAINT FK_GR_ReceivedBy FOREIGN KEY (ReceivedBy) REFERENCES [User](UserID);

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Task')
BEGIN
    CREATE TABLE Task (
        TaskID INT IDENTITY(1,1) PRIMARY KEY,
        GoodsReceiptItemID INT NOT NULL,
        TargetBinID INT NOT NULL,
        Quantity INT NOT NULL,
        [Status] BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_Task_GRI FOREIGN KEY (GoodsReceiptItemID) REFERENCES GoodsReceiptItem(GoodsReceiptItemID),
        CONSTRAINT FK_Task_Bin FOREIGN KEY (TargetBinID) REFERENCES Bin(BinID)
    );
END
