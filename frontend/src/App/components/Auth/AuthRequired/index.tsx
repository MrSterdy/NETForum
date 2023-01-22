import { ReactNode}  from "react";
import { Navigate } from "react-router-dom";

import { useAuth } from "../../../hooks";

export default function AuthRequired({ children }: { children: ReactNode }) {
    const { account } = useAuth();

    if (account === undefined)
        return <Navigate to="/account/login" />;

    return <>{children}</>;
}