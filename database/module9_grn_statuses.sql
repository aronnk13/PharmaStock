-- MODULE 9: GRN QC workflow — seed missing lookup values
-- Run this once against your DB before using the complete-qc endpoint.

-- GoodsReceiptStatus: add QC workflow statuses if missing
IF NOT EXISTS (SELECT 1 FROM GoodsReceiptStatus WHERE [Status] = 'PendingQC')
    INSERT INTO GoodsReceiptStatus ([Status]) VALUES ('PendingQC');

IF NOT EXISTS (SELECT 1 FROM GoodsReceiptStatus WHERE [Status] = 'Completed')
    INSERT INTO GoodsReceiptStatus ([Status]) VALUES ('Completed');

IF NOT EXISTS (SELECT 1 FROM GoodsReceiptStatus WHERE [Status] = 'PartiallyAccepted')
    INSERT INTO GoodsReceiptStatus ([Status]) VALUES ('PartiallyAccepted');

IF NOT EXISTS (SELECT 1 FROM GoodsReceiptStatus WHERE [Status] = 'FullyRejected')
    INSERT INTO GoodsReceiptStatus ([Status]) VALUES ('FullyRejected');

-- PurchaseOrderStatus: add receiving statuses if missing
IF NOT EXISTS (SELECT 1 FROM PurchaseOrderStatus WHERE [Status] = 'PartiallyReceived')
    INSERT INTO PurchaseOrderStatus ([Status]) VALUES ('PartiallyReceived');

IF NOT EXISTS (SELECT 1 FROM PurchaseOrderStatus WHERE [Status] = 'Closed')
    INSERT INTO PurchaseOrderStatus ([Status]) VALUES ('Closed');

-- InventoryLotStatus: add Available and Quarantined if missing
IF NOT EXISTS (SELECT 1 FROM InventoryLotStatus WHERE [Status] = 'Available')
    INSERT INTO InventoryLotStatus ([Status]) VALUES ('Available');

IF NOT EXISTS (SELECT 1 FROM InventoryLotStatus WHERE [Status] = 'Quarantined')
    INSERT INTO InventoryLotStatus ([Status]) VALUES ('Quarantined');

-- StockTransitionType: add Receipt if missing
IF NOT EXISTS (SELECT 1 FROM StockTransitionType WHERE TransitionType = 'Receipt')
    INSERT INTO StockTransitionType (TransitionType) VALUES ('Receipt');

-- Verify
SELECT 'GoodsReceiptStatus' AS TableName, [Status] FROM GoodsReceiptStatus
UNION ALL
SELECT 'PurchaseOrderStatus', [Status] FROM PurchaseOrderStatus
UNION ALL
SELECT 'InventoryLotStatus', [Status] FROM InventoryLotStatus
UNION ALL
SELECT 'StockTransitionType', TransitionType FROM StockTransitionType;
