-- MODULE 2: CATALOG & INFRASTRUCTURE 

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'DrugForm')
BEGIN
    CREATE TABLE DrugForm (
        DrugFormID INT IDENTITY(1,1) PRIMARY KEY,
        Form VARCHAR(50) NOT NULL UNIQUE
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'DrugStorageClass')
BEGIN
    CREATE TABLE DrugStorageClass (
        DrugStorageClassID INT IDENTITY(1,1) PRIMARY KEY,
        Class VARCHAR(50) NOT NULL UNIQUE
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ControlClass')
BEGIN
    CREATE TABLE ControlClass (
        ControlClassID INT IDENTITY(1,1) PRIMARY KEY,
        Class VARCHAR(50) NOT NULL UNIQUE
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'UoM')
BEGIN
    CREATE TABLE UoM (
        UoMID INT IDENTITY(1,1) PRIMARY KEY,
        Code VARCHAR(10) NOT NULL UNIQUE,
        Description VARCHAR(100) NOT NULL
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Drug')
BEGIN
    CREATE TABLE Drug (
        DrugID INT IDENTITY(1,1) PRIMARY KEY,
        GenericName VARCHAR(255) NOT NULL,
        BrandName VARCHAR(255),
        Strength VARCHAR(50) NOT NULL,
        Form INT NOT NULL,
        ATCCode VARCHAR(20),
        ControlClass INT NOT NULL,
        StorageClass INT NOT NULL,
        [Status] BIT NOT NULL DEFAULT 1,
        CONSTRAINT FK_Drug_Form FOREIGN KEY (Form) REFERENCES DrugForm(DrugFormID),
        CONSTRAINT FK_Drug_Control FOREIGN KEY (ControlClass) REFERENCES ControlClass(ControlClassID),
        CONSTRAINT FK_Drug_Storage FOREIGN KEY (StorageClass) REFERENCES DrugStorageClass(DrugStorageClassID)
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Item')
BEGIN
    CREATE TABLE Item (
        ItemID INT IDENTITY(1,1) PRIMARY KEY,
        DrugID INT NOT NULL,
        PackSize INT,
        UoM INT NOT NULL,
        ConversionToEach DECIMAL(10,4) NOT NULL DEFAULT 1.0000,
        Barcode VARCHAR(100) UNIQUE,
        [Status] BIT NOT NULL DEFAULT 1,
        CONSTRAINT FK_Item_Drug FOREIGN KEY (DrugID) REFERENCES Drug(DrugID),
        CONSTRAINT FK_Item_UoM FOREIGN KEY (UoM) REFERENCES UoM(UoMID)
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'LocationType')
BEGIN
    CREATE TABLE LocationType (
        LocationTypeID INT IDENTITY(1,1) PRIMARY KEY,
        [Type] VARCHAR(50) NOT NULL
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Location')
BEGIN
    CREATE TABLE [Location] (
        LocationID INT IDENTITY(1,1) PRIMARY KEY,
        Name VARCHAR(100) NOT NULL,
        LocationTypeID INT NOT NULL,
        ParentLocationID INT NULL, 
        StatusID BIT NOT NULL DEFAULT 1,
        CONSTRAINT FK_Location_Type FOREIGN KEY (LocationTypeID) REFERENCES LocationType(LocationTypeID),
        CONSTRAINT FK_Location_Parent FOREIGN KEY (ParentLocationID) REFERENCES [Location](LocationID)
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'BinStorageClass')
BEGIN
    CREATE TABLE BinStorageClass (
        BinStorageClassId INT IDENTITY(1,1) PRIMARY KEY,
        StorageClass VARCHAR(50) NOT NULL
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Bin')
BEGIN
    CREATE TABLE Bin (
        BinID INT IDENTITY(1,1) PRIMARY KEY,
        LocationID INT NOT NULL,
        Code VARCHAR(50) NOT NULL,
        BinStorageClass INT NOT NULL,
        IsQuarantine BIT NOT NULL DEFAULT 0,
        MaxCapacity INT NOT NULL,
        StatusID BIT NOT NULL DEFAULT 1,
        CONSTRAINT FK_Bin_Location FOREIGN KEY (LocationID) REFERENCES [Location](LocationID),
        CONSTRAINT FK_Bin_Storage FOREIGN KEY (BinStorageClass) REFERENCES BinStorageClass(BinStorageClassId)
    );
END
