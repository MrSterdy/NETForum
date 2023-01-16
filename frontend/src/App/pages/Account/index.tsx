import { Navigate } from "react-router-dom";

import { useAuth } from "../../hooks";

export default function Account() {
    const { user } = useAuth();

    return <Navigate to={`/user/${user!.id}`} />
}