USE Crud; 

CREATE TABLE ItemMaster
(
    ItemCode NVARCHAR(25) NOT NULL PRIMARY KEY,
    Description NVARCHAR(300) NOT NULL,
    Active BIT NOT NULL,
    CustomerDescription NVARCHAR(300),
    SalesItem BIT NOT NULL,
    StockItem BIT NOT NULL,
    PurchasedItem BIT NOT NULL,
    Barcode NVARCHAR(100) NOT NULL,
    ManageItemBy INT NOT NULL,
    MinimumInventory DECIMAL(18, 2) NOT NULL,
    MaximumInventory DECIMAL(18, 2) NOT NULL,
    Remarks NVARCHAR(MAX),
    ImagePath NVARCHAR(MAX) NOT NULL
);

CREATE UNIQUE INDEX IX_ItemCode ON ItemMaster (ItemCode);