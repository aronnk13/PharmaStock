-- =============================================================
-- PharmaStock  —  Seed & Sample Data
-- Run this script ONCE in SQL Server Management Studio
-- against your PharmaStock database.
-- =============================================================

USE PharmaStockDb;   -- <-- change this to your actual DB name if different
GO

-- =============================================================
-- 1. USER ROLES  (IAM)
-- =============================================================
-- Make sure every role exists (safe to run even if already there)

IF NOT EXISTS (SELECT 1 FROM UserRole WHERE RoleType = 'Admin')
    INSERT INTO UserRole (RoleType) VALUES ('Admin');

IF NOT EXISTS (SELECT 1 FROM UserRole WHERE RoleType = 'ProcurementOfficer')
    INSERT INTO UserRole (RoleType) VALUES ('ProcurementOfficer');

IF NOT EXISTS (SELECT 1 FROM UserRole WHERE RoleType = 'InventoryController')
    INSERT INTO UserRole (RoleType) VALUES ('InventoryController');

IF NOT EXISTS (SELECT 1 FROM UserRole WHERE RoleType = 'QualityComplianceOfficer')
    INSERT INTO UserRole (RoleType) VALUES ('QualityComplianceOfficer');

IF NOT EXISTS (SELECT 1 FROM UserRole WHERE RoleType = 'Pharmacist')
    INSERT INTO UserRole (RoleType) VALUES ('Pharmacist');

GO

-- =============================================================
-- 2. USERS  (plain-text passwords — demo only)
-- =============================================================

IF NOT EXISTS (SELECT 1 FROM [User] WHERE Username = 'admin')
    INSERT INTO [User] (Username, RoleID, Email, Phone, CreatedBy, UpdatedBy, PasswordHash)
    VALUES ('admin',
            (SELECT RoleID FROM UserRole WHERE RoleType = 'Admin'),
            'admin@pharmastock.com', '0000000000', 'system', 'system', 'admin123');

IF NOT EXISTS (SELECT 1 FROM [User] WHERE Username = 'procurement1')
    INSERT INTO [User] (Username, RoleID, Email, Phone, CreatedBy, UpdatedBy, PasswordHash)
    VALUES ('procurement1',
            (SELECT RoleID FROM UserRole WHERE RoleType = 'ProcurementOfficer'),
            'procurement@pharmastock.com', '0111111111', 'system', 'system', 'proc123');

IF NOT EXISTS (SELECT 1 FROM [User] WHERE Username = 'ic1')
    INSERT INTO [User] (Username, RoleID, Email, Phone, CreatedBy, UpdatedBy, PasswordHash)
    VALUES ('ic1',
            (SELECT RoleID FROM UserRole WHERE RoleType = 'InventoryController'),
            'ic@pharmastock.com', '0222222222', 'system', 'system', 'ic123');

IF NOT EXISTS (SELECT 1 FROM [User] WHERE Username = 'qco1')
    INSERT INTO [User] (Username, RoleID, Email, Phone, CreatedBy, UpdatedBy, PasswordHash)
    VALUES ('qco1',
            (SELECT RoleID FROM UserRole WHERE RoleType = 'QualityComplianceOfficer'),
            'qco@pharmastock.com', '0333333333', 'system', 'system', 'qco123');

IF NOT EXISTS (SELECT 1 FROM [User] WHERE Username = 'pharmacist1')
    INSERT INTO [User] (Username, RoleID, Email, Phone, CreatedBy, UpdatedBy, PasswordHash)
    VALUES ('pharmacist1',
            (SELECT RoleID FROM UserRole WHERE RoleType = 'Pharmacist'),
            'pharmacist@pharmastock.com', '0444444444', 'system', 'system', 'pharma123');

GO

-- =============================================================
-- 3. QUARANTINE STATUS  (lookup — required for quarantine to work)
-- =============================================================

IF NOT EXISTS (SELECT 1 FROM QuarantineStatus WHERE [Status] = 'Active')
    INSERT INTO QuarantineStatus ([Status]) VALUES ('Active');        -- ID 1

IF NOT EXISTS (SELECT 1 FROM QuarantineStatus WHERE [Status] = 'Released')
    INSERT INTO QuarantineStatus ([Status]) VALUES ('Released');      -- ID 2

IF NOT EXISTS (SELECT 1 FROM QuarantineStatus WHERE [Status] = 'Disposed')
    INSERT INTO QuarantineStatus ([Status]) VALUES ('Disposed');      -- ID 3

GO

-- =============================================================
-- 4. RECALL ACTION  (lookup — required for recall notices to work)
-- =============================================================

IF NOT EXISTS (SELECT 1 FROM RecallAction WHERE [Action] = 'Return to Supplier')
    INSERT INTO RecallAction ([Action]) VALUES ('Return to Supplier');  -- ID 1

IF NOT EXISTS (SELECT 1 FROM RecallAction WHERE [Action] = 'Quarantine & Investigate')
    INSERT INTO RecallAction ([Action]) VALUES ('Quarantine & Investigate'); -- ID 2

IF NOT EXISTS (SELECT 1 FROM RecallAction WHERE [Action] = 'Dispose')
    INSERT INTO RecallAction ([Action]) VALUES ('Dispose');              -- ID 3

GO

-- =============================================================
-- 5. DESTINATION TYPE  (lookup — required for dispense to work)
-- =============================================================

IF NOT EXISTS (SELECT 1 FROM DestinationType WHERE [Type] = 'Ward')
    INSERT INTO DestinationType ([Type]) VALUES ('Ward');              -- ID 1

IF NOT EXISTS (SELECT 1 FROM DestinationType WHERE [Type] = 'Outpatient')
    INSERT INTO DestinationType ([Type]) VALUES ('Outpatient');        -- ID 2

IF NOT EXISTS (SELECT 1 FROM DestinationType WHERE [Type] = 'Emergency')
    INSERT INTO DestinationType ([Type]) VALUES ('Emergency');         -- ID 3

IF NOT EXISTS (SELECT 1 FROM DestinationType WHERE [Type] = 'ICU')
    INSERT INTO DestinationType ([Type]) VALUES ('ICU');               -- ID 4

IF NOT EXISTS (SELECT 1 FROM DestinationType WHERE [Type] = 'Operating Theatre')
    INSERT INTO DestinationType ([Type]) VALUES ('Operating Theatre'); -- ID 5

GO

-- =============================================================
-- 6. SAMPLE QUARANTINE ACTIONS
-- (uses your first 3 inventory lots — adjust IDs if needed)
-- =============================================================

-- First verify some inventory lots exist:
-- SELECT TOP 5 InventoryLotID, BatchNumber FROM InventoryLot;

IF EXISTS (SELECT 1 FROM InventoryLot WHERE InventoryLotID = 1)
   AND NOT EXISTS (SELECT 1 FROM QuarantaineAction WHERE InventoryLotID = 1 AND Reason = 'Suspected contamination — batch discoloration observed')
BEGIN
    INSERT INTO QuarantaineAction (InventoryLotID, QuarantineDate, Reason, ReleasedDate, [Status])
    VALUES
        (1, DATEADD(DAY, -10, GETDATE()), 'Suspected contamination — batch discoloration observed', NULL, 1),
        (1, DATEADD(DAY, -25, GETDATE()), 'Temperature excursion during storage (exceeded 8°C)', DATEADD(DAY, -20, GETDATE()), 2);
END

IF EXISTS (SELECT 1 FROM InventoryLot WHERE InventoryLotID = 2)
   AND NOT EXISTS (SELECT 1 FROM QuarantaineAction WHERE InventoryLotID = 2 AND Reason = 'Supplier recall alert received')
BEGIN
    INSERT INTO QuarantaineAction (InventoryLotID, QuarantineDate, Reason, ReleasedDate, [Status])
    VALUES
        (2, DATEADD(DAY, -5, GETDATE()),  'Supplier recall alert received — pending investigation', NULL, 1),
        (2, DATEADD(DAY, -40, GETDATE()), 'Packaging integrity failure — seal broken on arrival', DATEADD(DAY, -35, GETDATE()), 3);
END

GO

-- =============================================================
-- 7. SAMPLE RECALL NOTICES
-- (uses your first 2 drugs — adjust DrugID if needed)
-- =============================================================

IF EXISTS (SELECT 1 FROM Drug WHERE DrugID = 1)
   AND NOT EXISTS (SELECT 1 FROM RecallNotice WHERE DrugID = 1)
BEGIN
    INSERT INTO RecallNotice (DrugID, NoticeDate, Reason, [Action], [Status])
    VALUES
        (1, DATEADD(DAY, -8, GETDATE()),
         'Manufacturer voluntary recall — sub-potent API level found in batch series 4XX',
         (SELECT TOP 1 RecallActionID FROM RecallAction WHERE [Action] = 'Return to Supplier'),
         1),
        (1, DATEADD(DAY, -45, GETDATE()),
         'Health authority alert — label error on dosage instructions',
         (SELECT TOP 1 RecallActionID FROM RecallAction WHERE [Action] = 'Quarantine & Investigate'),
         0);
END

IF EXISTS (SELECT 1 FROM Drug WHERE DrugID = 2)
   AND NOT EXISTS (SELECT 1 FROM RecallNotice WHERE DrugID = 2)
BEGIN
    INSERT INTO RecallNotice (DrugID, NoticeDate, Reason, [Action], [Status])
    VALUES
        (2, DATEADD(DAY, -3, GETDATE()),
         'Potential microbial contamination detected in stability testing',
         (SELECT TOP 1 RecallActionID FROM RecallAction WHERE [Action] = 'Dispose'),
         1);
END

GO

-- =============================================================
-- 8. REASON CODES  (lookup — required for Stock Adjustments)
-- =============================================================

IF NOT EXISTS (SELECT 1 FROM Reason WHERE Description = 'Damaged Stock')
    INSERT INTO Reason (Description) VALUES ('Damaged Stock');        -- ID 1

IF NOT EXISTS (SELECT 1 FROM Reason WHERE Description = 'Expired Stock')
    INSERT INTO Reason (Description) VALUES ('Expired Stock');        -- ID 2

IF NOT EXISTS (SELECT 1 FROM Reason WHERE Description = 'Stock Count Variance')
    INSERT INTO Reason (Description) VALUES ('Stock Count Variance'); -- ID 3

IF NOT EXISTS (SELECT 1 FROM Reason WHERE Description = 'Supplier Short-Delivery')
    INSERT INTO Reason (Description) VALUES ('Supplier Short-Delivery'); -- ID 4

IF NOT EXISTS (SELECT 1 FROM Reason WHERE Description = 'Data Entry Correction')
    INSERT INTO Reason (Description) VALUES ('Data Entry Correction'); -- ID 5

GO

-- =============================================================
-- 10. SAMPLE DISPENSE RECORDS
-- (uses Location 1, and your first 2 items/lots — adjust if needed)
-- =============================================================

IF EXISTS (SELECT 1 FROM [Location] WHERE LocationID = 1)
   AND EXISTS (SELECT 1 FROM Item WHERE ItemID = 1)
   AND EXISTS (SELECT 1 FROM InventoryLot WHERE InventoryLotID = 1)
   AND NOT EXISTS (SELECT 1 FROM DispenseRef WHERE LocationID = 1 AND ItemID = 1 AND InventoryLotID = 1)
BEGIN
    INSERT INTO DispenseRef (LocationID, ItemID, InventoryLotID, Quantity, DispenseDate, [Status], Destination)
    VALUES
        (1, 1, 1, 20, DATEADD(HOUR, -2,  GETDATE()), 1,
         (SELECT TOP 1 DestinationTypeID FROM DestinationType WHERE [Type] = 'Ward')),
        (1, 1, 1, 10, DATEADD(DAY,  -1,  GETDATE()), 1,
         (SELECT TOP 1 DestinationTypeID FROM DestinationType WHERE [Type] = 'Emergency')),
        (1, 1, 1,  5, DATEADD(DAY,  -3,  GETDATE()), 1,
         (SELECT TOP 1 DestinationTypeID FROM DestinationType WHERE [Type] = 'ICU'));
END

IF EXISTS (SELECT 1 FROM [Location] WHERE LocationID = 1)
   AND EXISTS (SELECT 1 FROM Item WHERE ItemID = 2)
   AND EXISTS (SELECT 1 FROM InventoryLot WHERE InventoryLotID = 2)
   AND NOT EXISTS (SELECT 1 FROM DispenseRef WHERE LocationID = 1 AND ItemID = 2 AND InventoryLotID = 2)
BEGIN
    INSERT INTO DispenseRef (LocationID, ItemID, InventoryLotID, Quantity, DispenseDate, [Status], Destination)
    VALUES
        (1, 2, 2, 30, DATEADD(HOUR, -6,  GETDATE()), 1,
         (SELECT TOP 1 DestinationTypeID FROM DestinationType WHERE [Type] = 'Outpatient')),
        (1, 2, 2, 15, DATEADD(DAY,  -2,  GETDATE()), 1,
         (SELECT TOP 1 DestinationTypeID FROM DestinationType WHERE [Type] = 'Ward'));
END

GO

-- =============================================================
-- 9. EXPIRY WATCH ENTRIES  (so the Expiry Watch page has data)
-- Links to your existing inventory lots
-- =============================================================

IF EXISTS (SELECT 1 FROM InventoryLot WHERE InventoryLotID = 1)
   AND NOT EXISTS (SELECT 1 FROM ExpiryWatch WHERE InventoryLotID = 1)
BEGIN
    INSERT INTO ExpiryWatch (InventoryLotID, DaysToExpire, FlagDate, [Status])
    SELECT
        il.InventoryLotID,
        DATEDIFF(DAY, CAST(GETDATE() AS DATE), il.ExpiryDate) AS DaysToExpire,
        CAST(GETDATE() AS DATE),
        1
    FROM InventoryLot il
    WHERE il.InventoryLotID = 1;
END

IF EXISTS (SELECT 1 FROM InventoryLot WHERE InventoryLotID = 2)
   AND NOT EXISTS (SELECT 1 FROM ExpiryWatch WHERE InventoryLotID = 2)
BEGIN
    INSERT INTO ExpiryWatch (InventoryLotID, DaysToExpire, FlagDate, [Status])
    SELECT
        il.InventoryLotID,
        DATEDIFF(DAY, CAST(GETDATE() AS DATE), il.ExpiryDate) AS DaysToExpire,
        CAST(GETDATE() AS DATE),
        1
    FROM InventoryLot il
    WHERE il.InventoryLotID = 2;
END

GO

-- =============================================================
-- 10. STOCK ADJUSTMENT SAMPLE DATA
-- =============================================================

IF EXISTS (SELECT 1 FROM [Location] WHERE LocationID = 1)
   AND EXISTS (SELECT 1 FROM Item WHERE ItemID = 1)
   AND EXISTS (SELECT 1 FROM InventoryLot WHERE InventoryLotID = 1)
   AND EXISTS (SELECT 1 FROM Reason WHERE ReasonID = 1)
   AND EXISTS (SELECT 1 FROM [User] WHERE UserID = 1)
   AND NOT EXISTS (SELECT 1 FROM StockAdjustment WHERE LocationID = 1 AND ItemID = 1 AND QuantityDelta = -5)
BEGIN
    INSERT INTO StockAdjustment (LocationID, ItemID, InventoryLotID, QuantityDelta, ReasonCode, ApprovedBy, PostedDate)
    VALUES
        (1, 1, 1, -5,
         (SELECT TOP 1 ReasonID FROM Reason),
         (SELECT TOP 1 UserID FROM [User]),
         DATEADD(DAY, -7, GETDATE())),
        (1, 1, 1,  10,
         (SELECT TOP 1 ReasonID FROM Reason),
         (SELECT TOP 1 UserID FROM [User]),
         DATEADD(DAY, -2, GETDATE()));
END

GO

-- =============================================================
-- VERIFY — run these SELECTs to confirm data was inserted:
-- =============================================================
-- SELECT * FROM QuarantineStatus;
-- SELECT * FROM RecallAction;
-- SELECT * FROM DestinationType;
-- SELECT COUNT(*) AS QuarantineCount FROM QuarantaineAction;
-- SELECT COUNT(*) AS RecallCount     FROM RecallNotice;
-- SELECT COUNT(*) AS DispenseCount   FROM DispenseRef;
-- SELECT COUNT(*) AS ExpiryCount     FROM ExpiryWatch;
-- SELECT COUNT(*) AS AdjustCount     FROM StockAdjustment;
-- =============================================================
