import {useLoaderData, useParams} from "react-router-dom";
import {ApiBase, Item} from "../api/api.ts";
import * as React from "react";
import {useState} from "react";
export default function Page2() {

    // Get data from loader
    const loaderData: Item = useLoaderData() as Item;

    const {itemCode} = useParams();

    const [showUpdateModel, setShowUpdateModel] = useState<boolean>(false);

    // Modal input States
    const [descriptionState, setDescription] = useState<string>("");
    const [activeState, setActive] = useState<boolean>(false);
    const [customerDescription, setCustomerDescription] = useState<string>("");
    const [salesItem, setSalesItem] = useState<boolean>(false);
    const [stockItem, setStockItem] = useState<boolean>(false);
    const [purchasedItem, setPurchasedItem] = useState<boolean>(false);
    const [barcode, setBarcode] = useState<string>("");
    const [manageItemBy, setManageItemBy] = useState<number>(0);
    const [minimumInventory, setMinimumInventory] = useState<number>(0);
    const [maximumInventory, setMaximumInventory] = useState<number>(0);
    const [remarks, setRemarks] = useState<string>("");
    const [imagePath, setImagePath] = useState<string>("");

    const UpdateItem = async () => {
        const response: Response = await fetch(
            ApiBase + `/items/${itemCode}/update`,
            {
                method: "patch",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    description: descriptionState,
                    active: activeState,
                    salesItem: salesItem,
                    customerDescription: customerDescription,
                    stockItem: stockItem,
                    purchasedItem: purchasedItem,
                    barcode: barcode,
                    manageItemBy: manageItemBy,
                    minimumInventory: minimumInventory,
                    maximumInventory: maximumInventory,
                    remarks: remarks,
                    imagePath: imagePath
                })
            }
        );

        alert((response.status === 200) ? "Updated item." : "Failed to update item.");
    };

    const DeleteItem = async () => {
        const response = await fetch(
            ApiBase + `/items/${itemCode}/delete`,
            {
                method: "delete"
            }
        );

        if (response.status !== 200) {
            return alert("Failed to delete item.");
        }

        alert("Deleted item successfully.");
        window.location.href = "/";
    };

    return (
        <div style={{padding: 10, display: "flex", flexDirection: "column", alignItems: "start"}}>
            <img style={{width: 200, height: 200}} src={loaderData.ImagePath} alt="img"/>

            <div onClick={e => e.target === e.currentTarget && setShowUpdateModel(false)} style={{position: "fixed", display: (showUpdateModel) ? "flex" : "none", alignItems: "center", justifyContent: "center", left: 0, top: 0, height: "100%", width: "100%", background: "rgba(0,0,0,0.5)"}}>
                <div style={{background: "white", display: "grid", padding: 10, width:490, gap: 10}}>
                    <div style={{display: "grid", gridTemplateColumns: "1fr 1fr", gap:3}}>
                        <input onInput={(e: React.KeyboardEvent<HTMLInputElement>) => setDescription(e.currentTarget.value)} style={{flexGrow: 1, padding: 5}} placeholder="Description"/>
                        <select onInput={(e) => setActive(parseInt(e.currentTarget.value) === 1)} style={{padding: 5}}>
                            <option defaultChecked>
                                Active
                            </option>
                            <option value={1}>
                                true
                            </option>
                            <option value={0}>
                                false
                            </option>
                        </select>
                        <input onInput={(e: React.KeyboardEvent<HTMLInputElement>) => setCustomerDescription(e.currentTarget.value)} style={{flexGrow: 1, padding: 5}} placeholder="CustomerDescription"/>
                        <select onInput={(e) => setSalesItem(parseInt(e.currentTarget.value) === 1)} style={{padding: 5}}>
                            <option defaultChecked>
                                SalesItem
                            </option>
                            <option value={1}>
                                true
                            </option>
                            <option value={0}>
                                false
                            </option>
                        </select>
                        <select onInput={(e) => setStockItem(parseInt(e.currentTarget.value) === 1)} style={{padding: 5}}>
                            <option defaultChecked>
                                StockItem
                            </option>
                            <option value={1}>
                                true
                            </option>
                            <option value={0}>
                                false
                            </option>
                        </select>
                        <select onInput={(e) => setPurchasedItem(parseInt(e.currentTarget.value) === 1)} style={{padding: 5}}>
                            <option defaultChecked>
                                PurchasedItem
                            </option>
                            <option value={1}>
                                true
                            </option>
                            <option value={0}>
                                false
                            </option>
                        </select>
                        <input type="number" onInput={(e: React.KeyboardEvent<HTMLInputElement>) => setMinimumInventory(parseInt(e.currentTarget.value))} style={{flexGrow: 1, padding: 5}} placeholder="MinimumInventory"/>
                        <input type="number" onInput={(e: React.KeyboardEvent<HTMLInputElement>) => setMaximumInventory(parseInt(e.currentTarget.value))} style={{flexGrow: 1, padding: 5}} placeholder="MaximumInventory"/>
                        <input type="number" onInput={(e: React.KeyboardEvent<HTMLInputElement>) => setManageItemBy(parseInt(e.currentTarget.value))} style={{flexGrow: 1, padding: 5}} placeholder="ManageItemBy"/>
                        <input onInput={(e: React.KeyboardEvent<HTMLInputElement>) => setRemarks(e.currentTarget.value)} style={{flexGrow: 1, padding: 5}} placeholder="Remarks"/>
                        <input onInput={(e: React.KeyboardEvent<HTMLInputElement>) => setImagePath(e.currentTarget.value)} style={{flexGrow: 1, padding: 5}} placeholder="ImagePath"/>
                        <input onInput={(e: React.KeyboardEvent<HTMLInputElement>) => setBarcode(e.currentTarget.value)} style={{flexGrow: 1, padding: 5}} placeholder="Barcode"/>
                    </div>

                    <div>
                        <button onClick={UpdateItem} style={{padding: 3}}>
                            Update
                        </button>
                    </div>
                </div>
            </div>

            <div style={{display: "flex", gap: 4, marginTop: 10}}>
                <button onClick={() => setShowUpdateModel(true)} style={{padding: 3}}>
                    Update
                </button>
                <button onClick={DeleteItem} style={{padding: 3}}>
                    Delete
                </button>
            </div>
        </div>
    );
}