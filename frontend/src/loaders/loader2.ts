import {ApiBase, Item} from "../api/api.ts";

// This loader takes the param "itemCode" and fetches item based on it, if its invalid it will redirect to page 1
export default async function Loader1({params} : any) : Promise<Item> {
    if (!params.itemCode) {
        window.location.href = "/";
    }

    const response: Response = await fetch(ApiBase + "/items/fetch/" + params.itemCode);

    if (response.status !== 200) {
        window.location.href = "/";
    }

    return await response.json();
}

