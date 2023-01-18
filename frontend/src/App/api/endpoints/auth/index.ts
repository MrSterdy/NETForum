import axios from "redaxios";

import { IUser } from "../../models";

export interface LoginParams {
    username: string,
    password: string,
    rememberMe: boolean
}

export async function logIn(params: LoginParams) {
    return await axios.post<IUser>(process.env.REACT_APP_AUTH_LOGIN_URL!, params, { withCredentials: true });
}

export async function logOut() {
    await axios.post(process.env.REACT_APP_AUTH_LOGOUT_URL!);
}

export interface SignupParams {
    email: string,
    username: string,
    password: string,

    clientUrl: string
}

export async function signUp(params: SignupParams) {
    return await axios.post<IUser>(process.env.REACT_APP_AUTH_SIGNUP_URL!, params);
}

interface ConfirmEmailParams {
    userId: number,

    code: string
}

export async function confirmEmail(params: ConfirmEmailParams) {
    return await axios.get(process.env.REACT_APP_AUTH_CONFIRM_URL!, { params });
}