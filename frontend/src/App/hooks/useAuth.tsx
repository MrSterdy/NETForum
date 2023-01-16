import { useContext } from "react";

import { AuthContext } from "../components";

export default function useAuth() {
    return useContext(AuthContext);
}