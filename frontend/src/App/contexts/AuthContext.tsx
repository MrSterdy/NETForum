import React, {
    createContext,
    ReactNode,
    useEffect,
    useMemo,
    useState
} from "react";
import { useLocation } from "react-router-dom";
import { Response } from "redaxios";

import { IUser } from "../api/models";
import { LoginParams, RegisterParams } from "../api/auth";
import * as userApi from "../api/user";
import * as authApi from "../api/auth";

interface AuthContextType {
    user?: IUser;

    isLoading: boolean;

    error: number;

    login: (params: LoginParams) => void;
    logout: () => void;

    register: (params: RegisterParams) => void;
}

export const AuthContext = createContext<AuthContextType>({} as AuthContextType);

export function AuthProvider({ children }: { children: ReactNode }): JSX.Element {
    const [user, setUser] = useState<IUser>();
    const [error, setError] = useState(0);
    const [isLoading, setLoading] = useState(false);
    const [isLoadingInitial, setLoadingInitial] = useState(true);

    const location = useLocation();

    useEffect(() => setError(0), [location.pathname]);

    useEffect(() => {
        userApi.getCurrentUser()
            .then(res => setUser(res.data))
            .catch(() => {})
            .finally(() => setLoadingInitial(false));
    }, []);

    function login(params: LoginParams) {
        setLoading(true);

        authApi.login(params)
            .then(res => setUser(res.data))
            .catch(res => setError((res as Response<unknown>).status))
            .finally(() => setLoading(false));
    }

    function logout() {
        return authApi.logout().then(() => setUser(undefined));
    }

    function register(params: RegisterParams) {
        setLoading(true);

        authApi.register(params)
            .then(res => setUser(res.data))
            .catch(res => setError((res as Response<unknown>).status))
            .finally(() => setLoading(false));
    }

    const memo = useMemo(
        () => ({
            user,
            isLoading,
            error,
            login,
            register,
            logout
        }),
        [user, isLoading, error]
    );

    return (
        <AuthContext.Provider value={ memo }>
            { !isLoadingInitial && children }
        </AuthContext.Provider>
    );
}