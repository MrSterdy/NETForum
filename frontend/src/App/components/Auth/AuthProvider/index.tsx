import React, {
    ReactNode,
    useEffect,
    useMemo,
    useState
} from "react";
import { useLocation } from "react-router-dom";
import { Response } from "redaxios";

import AuthContext from "../AuthContext";

import { IUser } from "../../../api/models";

import { LoginParams, SignupParams } from "../../../api/endpoints/auth";
import * as accountApi from "../../../api/endpoints/account";
import * as authApi from "../../../api/endpoints/auth";

export default function AuthProvider({ children }: { children: ReactNode }): JSX.Element {
    const [user, setUser] = useState<IUser>();
    const [error, setError] = useState(0);
    const [isLoading, setLoading] = useState(true);

    const location = useLocation();

    useEffect(() => setError(0), [location.pathname]);

    useEffect(() => {
        accountApi.getAccount()
            .then(res => setUser(res.data))
            .catch(() => {})
            .finally(() => setLoading(false))
    }, []);

    function logIn(params: LoginParams) {
        setLoading(true);

        authApi.logIn(params)
            .then(res => setUser(res.data))
            .catch(res => setError((res as Response<unknown>).status))
            .finally(() => setLoading(false));
    }

    function logOut() {
        return authApi.logOut().then(() => setUser(undefined));
    }

    function signUp(params: SignupParams) {
        setLoading(true);

        authApi.signUp(params)
            .then(() => setUser({
                userName: params.username,
                email: params.email,
                confirmed: false
            }))
            .catch(res => setError((res as Response<unknown>).status))
            .finally(() => setLoading(false));
    }

    const memo = useMemo(
        () => ({
            user,
            isLoading,
            error,
            logIn,
            signUp,
            logOut
        }),
        [user, isLoading, error]
    );

    return (
        <AuthContext.Provider value={memo}>
            {!isLoading && children}
        </AuthContext.Provider>
    );
}