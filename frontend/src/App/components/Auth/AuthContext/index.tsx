import { createContext } from "react";

import { IUser } from "../../../api/models";
import { LoginParams, SignupParams } from "../../../api/endpoints/auth";

interface AuthContextType {
    user?: IUser;

    isLoading: boolean;

    error: number;

    logIn: (params: LoginParams) => void;
    logOut: () => void;

    signUp: (params: SignupParams) => void;
}

const AuthContext = createContext<AuthContextType>({} as AuthContextType);

export default AuthContext;