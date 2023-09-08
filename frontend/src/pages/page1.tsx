import {Link, NavigateFunction, useLoaderData, useNavigate} from "react-router-dom";
import {ApiBase, Item} from "../api/api.ts";
import * as React from "react";
import { useState } from "react";
import { useRevalidator } from 'react-router-dom';

export default function Page1() {

    // Get data from loader
    const loaderData: Item[] = useLoaderData() as Item[];

    // React Router navigate hook which i will use to change routes
    const navigator: NavigateFunction = useNavigate();

    // we use this hook to re-run the loader to fetch new items
    const revalidator = useRevalidator();

    // HandleSearch function listens for a "Enter" key press event and performs a navigation action.
    const HandleSearch = (e: React.KeyboardEvent<HTMLInputElement>) : void => {
        if (e.key !== "Enter") {
            return;
        }

        navigator("/" + e.currentTarget.value);
    }

    // Show create product modal state
    const [showCreateModel, setShowCreateModel] = useState<boolean>(false);

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


    const CreateItem = async () => {
        const response: Response = await fetch(
            ApiBase + "/items/create",
            {
                method: "post",
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

        alert((response.status === 200) ? "Created the item." : "Failed to create the item.");
        revalidator.revalidate();
    };

    return (
        <>
            <div onClick={e => e.target === e.currentTarget && setShowCreateModel(false)} style={{position: "fixed", display: (showCreateModel) ? "flex" : "none", alignItems: "center", justifyContent: "center", left: 0, top: 0, height: "100%", width: "100%", background: "rgba(0,0,0,0.5)"}}>
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
                        <button onClick={CreateItem} style={{padding: 3}}>
                            Create
                        </button>
                    </div>
                </div>
            </div>

            <div>
                <input placeholder={"search"} onKeyUp={HandleSearch} />
            </div>

            <div style={{marginTop: 50}}>
                {/* Looping through loaderData and creating a div on each iteration */}
                {loaderData.map((item: Item, k: number) => (
                    <div style={{display: "flex", alignItems: "center", gap: 50}} key={k}>
                        {item.ItemCode}

                        <Link to={"/page2/" + item.ItemCode}>
                            View
                        </Link>
                    </div>
                ))}
            </div>

            <button onClick={() => setShowCreateModel(true)} style={{marginTop: 20}}>
                create
            </button>
        </>
    );
}