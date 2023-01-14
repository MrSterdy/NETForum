import { ReactElement, ReactNode } from "react";
import { Navigate } from "react-router-dom";

import useAuth from "../../hooks/useAuth";

export default function AuthenticatedRoute({ children }: { children: ReactNode }) {
    const { user } = useAuth();

    return user?.confirmed ? (children as ReactElement) : <Navigate to="/login" />;
}