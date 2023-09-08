export const ApiBase: string = "https://localhost:44317/api";

export interface Item {
    Description: string,
    Active: boolean,
    CustomerDescription: string,
    SalesItem: boolean,
    StockItem: boolean,
    PurchasedItem: boolean,
    Barcode: string,
    ManageItemBy: number,
    MinimumInventory: number,
    MaximumInventory: number,
    Remarks: string,
    ImagePath: string,
    ItemCode: number
}