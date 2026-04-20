-- =============================================
-- STEP 1: LOOKUP TABLES  (run this first)
-- These are REQUIRED for everything to work
-- =============================================

-- Quarantine Status
INSERT INTO QuarantineStatus ([Status]) VALUES ('Active');
INSERT INTO QuarantineStatus ([Status]) VALUES ('Released');
INSERT INTO QuarantineStatus ([Status]) VALUES ('Disposed');

-- Recall Action
INSERT INTO RecallAction ([Action]) VALUES ('Return to Supplier');
INSERT INTO RecallAction ([Action]) VALUES ('Quarantine & Investigate');
INSERT INTO RecallAction ([Action]) VALUES ('Dispose');

-- Destination Type (fills the dispense dropdown)
INSERT INTO DestinationType ([Type]) VALUES ('Ward');
INSERT INTO DestinationType ([Type]) VALUES ('Outpatient');
INSERT INTO DestinationType ([Type]) VALUES ('Emergency');
INSERT INTO DestinationType ([Type]) VALUES ('ICU');
INSERT INTO DestinationType ([Type]) VALUES ('Operating Theatre');

-- Reason codes (needed for stock adjustments)
INSERT INTO Reason (Description) VALUES ('Damaged Stock');
INSERT INTO Reason (Description) VALUES ('Expired Stock');
INSERT INTO Reason (Description) VALUES ('Stock Count Variance');
INSERT INTO Reason (Description) VALUES ('Data Entry Correction');


-- =============================================
-- STEP 2: CHECK YOUR INVENTORY LOT IDs
-- Run this SELECT to see what IDs you have
-- =============================================

SELECT TOP 5
    il.InventoryLotID,
    il.ItemID,
    il.BatchNumber,
    il.ExpiryDate,
    i.ItemID,
    d.GenericName
FROM InventoryLot il
LEFT JOIN Item i ON i.ItemID = il.ItemID
LEFT JOIN Drug d ON d.DrugID = i.DrugID;


-- =============================================
-- STEP 3: SAMPLE DATA using your actual IDs
-- After running STEP 2, replace the numbers
-- below with real InventoryLotID and ItemID
-- values from your database.
-- =============================================

-- ---- QUARANTINE ACTIONS ----
-- Replace 1, 2 with real InventoryLotIDs from your DB

INSERT INTO QuarantaineAction (InventoryLotID, QuarantineDate, Reason, ReleasedDate, [Status])
VALUES (1, DATEADD(DAY,-5, GETDATE()), 'Suspected contamination — batch discoloration', NULL, 1);

INSERT INTO QuarantaineAction (InventoryLotID, QuarantineDate, Reason, ReleasedDate, [Status])
VALUES (1, DATEADD(DAY,-20,GETDATE()), 'Temperature excursion during storage', DATEADD(DAY,-15,GETDATE()), 2);

INSERT INTO QuarantaineAction (InventoryLotID, QuarantineDate, Reason, ReleasedDate, [Status])
VALUES (2, DATEADD(DAY,-3, GETDATE()), 'Supplier recall alert — pending investigation', NULL, 1);

INSERT INTO QuarantaineAction (InventoryLotID, QuarantineDate, Reason, ReleasedDate, [Status])
VALUES (2, DATEADD(DAY,-30,GETDATE()), 'Packaging seal broken on arrival', DATEADD(DAY,-28,GETDATE()), 3);


-- ---- RECALL NOTICES ----
-- Replace DrugID 1, 2 with real DrugIDs from your DB
-- Run:  SELECT TOP 5 DrugID, GenericName FROM Drug;

INSERT INTO RecallNotice (DrugID, NoticeDate, Reason, [Action], [Status])
VALUES (1, CAST(DATEADD(DAY,-6,GETDATE()) AS DATE),
        'Manufacturer voluntary recall — sub-potent API level found', 1, 1);

INSERT INTO RecallNotice (DrugID, NoticeDate, Reason, [Action], [Status])
VALUES (1, CAST(DATEADD(DAY,-40,GETDATE()) AS DATE),
        'Health authority alert — dosage label error', 2, 0);

INSERT INTO RecallNotice (DrugID, NoticeDate, Reason, [Action], [Status])
VALUES (2, CAST(DATEADD(DAY,-2,GETDATE()) AS DATE),
        'Potential microbial contamination in stability testing', 3, 1);


-- ---- DISPENSE RECORDS ----
-- Replace LocationID=1, ItemID=1/2, InventoryLotID=1/2
-- with real values from your DB

INSERT INTO DispenseRef (LocationID, ItemID, InventoryLotID, Quantity, DispenseDate, [Status], Destination)
VALUES (1, 1, 1, 20, DATEADD(HOUR,-2, GETDATE()),  1, 1);

INSERT INTO DispenseRef (LocationID, ItemID, InventoryLotID, Quantity, DispenseDate, [Status], Destination)
VALUES (1, 1, 1, 10, DATEADD(DAY, -1, GETDATE()),  1, 3);

INSERT INTO DispenseRef (LocationID, ItemID, InventoryLotID, Quantity, DispenseDate, [Status], Destination)
VALUES (1, 2, 2,  5, DATEADD(DAY, -2, GETDATE()),  1, 4);

INSERT INTO DispenseRef (LocationID, ItemID, InventoryLotID, Quantity, DispenseDate, [Status], Destination)
VALUES (1, 2, 2, 30, DATEADD(HOUR,-6, GETDATE()),  1, 2);


-- ---- EXPIRY WATCH ----
-- Creates ExpiryWatch entries for lot 1 and 2

INSERT INTO ExpiryWatch (InventoryLotID, DaysToExpire, FlagDate, [Status])
SELECT InventoryLotID,
       DATEDIFF(DAY, CAST(GETDATE() AS DATE), ExpiryDate),
       CAST(GETDATE() AS DATE),
       1
FROM InventoryLot
WHERE InventoryLotID IN (1, 2);


-- ---- STOCK ADJUSTMENTS ----
-- Replace UserID=1 with a real UserID from your DB
-- Run:  SELECT TOP 5 UserID, Username FROM [User];

INSERT INTO StockAdjustment (LocationID, ItemID, InventoryLotID, QuantityDelta, ReasonCode, ApprovedBy, PostedDate)
VALUES (1, 1, 1, -5, 1, 1, DATEADD(DAY,-7, GETDATE()));

INSERT INTO StockAdjustment (LocationID, ItemID, InventoryLotID, QuantityDelta, ReasonCode, ApprovedBy, PostedDate)
VALUES (1, 1, 1,  10, 3, 1, DATEADD(DAY,-2, GETDATE()));

INSERT INTO StockAdjustment (LocationID, ItemID, InventoryLotID, QuantityDelta, ReasonCode, ApprovedBy, PostedDate)
VALUES (1, 2, 2, -3, 2, 1, DATEADD(DAY,-1, GETDATE()));


-- =============================================
-- STEP 4: VERIFY — run these to confirm
-- =============================================
SELECT * FROM QuarantineStatus;
SELECT * FROM RecallAction;
SELECT * FROM DestinationType;
SELECT * FROM Reason;
SELECT COUNT(*) AS Quarantines    FROM QuarantaineAction;
SELECT COUNT(*) AS Recalls        FROM RecallNotice;
SELECT COUNT(*) AS Dispenses      FROM DispenseRef;
SELECT COUNT(*) AS ExpiryWatches  FROM ExpiryWatch;
SELECT COUNT(*) AS Adjustments    FROM StockAdjustment;
