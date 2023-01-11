import axios from "redaxios";

import { IUser } from "../models";

export interface LoginParams {
    username: string,
    password: string
}

export async function login(params: LoginParams) {
    return await axios.post<IUser>(process.env.REACT_APP_AUTH_LOGIN_URL!, params, { withCredentials: true });
}

export async function logout() {
    await axios.post(process.env.REACT_APP_AUTH_LOGOUT_URL!);
}

export interface RegisterParams extends LoginParams {
    email: string,
    password: string
}

export async function register(params: RegisterParams) {
    return await axios.post<IUser>(process.env.REACT_APP_AUTH_REGISTER_URL!, params);
}