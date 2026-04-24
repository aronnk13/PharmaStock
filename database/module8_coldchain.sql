-- MODULE 8: COLD-CHAIN LOGS & MISSING TABLES

-- ColdChainLog (new - required for Cold-Chain module)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ColdChainLog')
BEGIN
    CREATE TABLE ColdChainLog (
        LogID          INT IDENTITY(1,1) PRIMARY KEY,
        LocationID     INT NOT NULL,
        SensorID       NVARCHAR(50) NOT NULL,
        [Timestamp]    DATETIME NOT NULL DEFAULT GETDATE(),
        TemperatureC   DECIMAL(5,2) NOT NULL,
        [Status]       NVARCHAR(20) NOT NULL,   -- 'Normal' or 'Excursion'
        CONSTRAINT FK_CCLog_Location FOREIGN KEY (LocationID) REFERENCES [Location](LocationID)
    );
END

-- PutAwayTask (from spec Module 4.5 - was missing from project)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PutAwayTask')
BEGIN
    CREATE TABLE PutAwayTask (
        TaskID       INT IDENTITY(1,1) PRIMARY KEY,
        GRNItemID    INT NOT NULL,
        TargetBinID  INT NOT NULL,
        Quantity     INT NOT NULL,
        [Status]     NVARCHAR(20) NOT NULL DEFAULT 'Pending',  -- 'Pending' or 'Completed'
        CONSTRAINT FK_PAT_GRNItem FOREIGN KEY (GRNItemID)   REFERENCES GoodsReceiptItem(GoodsReceiptItemID),
        CONSTRAINT FK_PAT_Bin     FOREIGN KEY (TargetBinID) REFERENCES Bin(BinID)
    );
END
