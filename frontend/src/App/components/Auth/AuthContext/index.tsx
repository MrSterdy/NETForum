import { createContext } from "react";

import { IAccount } from "../../../api/models";

interface AuthContextType {
    account?: IAccount;

    isLoading: boolean;

    error: number;

    logIn: (username: string, password: string, rememberMe: boolean) => void;
    logOut: () => void;

    signUp: (email: string, username: string, password: string) => void;
}

const AuthContext = createContext<AuthContextType>({} as AuthContextType);

export default AuthContext;