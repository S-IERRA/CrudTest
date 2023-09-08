import ReactDOM from 'react-dom/client';
import {createBrowserRouter, RouterProvider} from "react-router-dom";
import Page1 from "./pages/page1.tsx";
import Page2 from "./pages/page2.tsx";
import Loader1 from "./loaders/loader1.ts";
import Loader2 from "./loaders/loader2.ts";
import "./assets/style.css";

// Loaders: a loader runs when the path matches and is used to fetch or validate things before the page renders
// Elements: react hooks that renders when "path" is matched or loader returns
// Path: a route that can contain params (which are prefixed with ":", optional params end with "?")

const router = createBrowserRouter([
    {
        path: "/:searchQuery?",
        element: <Page1 />,
        loader: Loader1
    },
    {
        "path": "/page2/:itemCode",
        element: <Page2 />,
        loader: Loader2
    }
]);


ReactDOM.createRoot(document.getElementById('root')!).render(
    <RouterProvider router={router} />
);
