import {ApiBase, Item} from "../api/api.ts";

// This loader takes an optional param "searchQuery" which if present will search instead of a regular fetch
// returns the json from the request on success and an empty list on failure

export default async function Loader1({params} : any) : Promise<Item[]> {
    const response: Response = await fetch(
        ApiBase + "/items/" + (params.searchQuery ? "search/" + params.searchQuery : "")
    );

    return (response.status === 200)
        ? await response.json()
        : [];
}

