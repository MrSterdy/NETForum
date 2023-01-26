import { ReactNode}  from "react";
import { Navigate } from "react-router-dom";

import { useAuth } from "../../../hooks";

import Loader from "../../Loader";

export default function AuthRequired({ children }: { children: ReactNode }) {
    const { account, isLoading } = useAuth();

    if (account === undefined)
        return <Navigate to="/account/login" />;

    return <>{isLoading ? <Loader /> : children}</>;
}