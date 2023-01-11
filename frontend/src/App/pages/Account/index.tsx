import { Navigate } from "react-router-dom";

import useAuth from "../../hooks/useAuth";

export default function Account() {
    const { user } = useAuth();

    return <Navigate to={ user?.confirmed ? `/user/${user.id}` : `/login` } />
}