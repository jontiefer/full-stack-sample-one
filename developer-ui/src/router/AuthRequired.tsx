import { Navigate, Outlet, useLocation } from "react-router-dom";
import { useAppStore } from "../stores/appStore";

export default function RequireAuth() {
    const {authStore: {isLoggedIn}} = useAppStore();
    const location = useLocation();

    if (!isLoggedIn) {
        return <Navigate to='/' state={{from: location}} />
    }

    return <Outlet />
}