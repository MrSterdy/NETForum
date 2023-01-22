import React, {
    ReactNode,
    useEffect,
    useMemo,
    useState
} from "react";
import { useLocation } from "react-router-dom";
import { Response } from "redaxios";

import AuthContext from "../AuthContext";

import { IAccount } from "../../../api/models";

import * as accountApi from "../../../api/endpoints/account";

export default function AuthProvider({ children }: { children: ReactNode }): JSX.Element {
    const [account, setAccount] = useState<IAccount>();
    const [error, setError] = useState(0);
    const [isLoading, setLoading] = useState(true);

    const location = useLocation();

    useEffect(() => setError(0), [location.pathname]);

    useEffect(() => {
        accountApi.getAccount()
            .then(res => setAccount(res.data))
            .catch(() => {})
            .finally(() => setLoading(false))
    }, []);

    function logIn(username: string, password: string, rememberMe: boolean) {
        setLoading(true);

        accountApi.login(username, password, rememberMe)
            .then(res => setAccount(res.data))
            .catch(res => setError((res as Response<unknown>).status))
            .finally(() => setLoading(false));
    }

    function logOut() {
        return accountApi.logout().then(() => setAccount(undefined));
    }

    function signUp(email: string, username: string, password: string) {
        setLoading(true);

        accountApi.signup(email, username, password)
            .then(res => setAccount(res.data))
            .catch(res => setError((res as Response<unknown>).status))
            .finally(() => setLoading(false));
    }

    const memo = useMemo(
        () => ({
            account,
            isLoading,
            error,
            logIn,
            signUp,
            logOut
        }),
        [account, isLoading, error]
    );

    return (
        <AuthContext.Provider value={memo}>
            {!isLoading && children}
        </AuthContext.Provider>
    );
}