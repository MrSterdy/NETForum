import axios from "redaxios";

import { IAccount } from "../../models";

const endpoint = process.env.REACT_APP_ACCOUNT_URL!;

export function getAccount() {
    return axios.get<IAccount>(endpoint, { withCredentials: true });
}

export function confirmEmail(userId: number, code: string) {
    return axios.post(`${endpoint}/confirmEmail`, {}, {
        withCredentials: true,
        params: { userId, code }
    });
}

export function changeEmail(code: string) {
    return axios.put(`${endpoint}/email`, {}, {
        withCredentials: true,
        params: { code }
    });
}

export function requestChangeEmail(email: string) {
    return axios.post(`${endpoint}/changeEmail`, { email }, {
        withCredentials: true,
        params: {
            callbackUrl: `${window.location.origin}/account/change-email`
        }
    });
}

export function changeUsername(username: string) {
    return axios.put(`${endpoint}/username`, { username }, { withCredentials: true });
}

export function changePassword(password: string, newPassword: string) {
    return axios.put(`${endpoint}/password`, { password, newPassword }, { withCredentials: true });
}

export function resetPassword(userId: number, code: string, newPassword: string) {
    return axios.put(`${endpoint}/password`, { newPassword }, {
        withCredentials: true,
        params: { userId, code }
    });
}

export function requestResetPassword(email: string) {
    return axios.post(`${endpoint}/resetPassword`, { email }, {
        withCredentials: true,
        params: {
            callbackUrl: `${window.location.origin}/account/reset-password`
        }
    });
}

export function login(username: string, password: string, rememberMe: boolean) {
    return axios.post<IAccount>(`${endpoint}/login`, { username, password, rememberMe }, { withCredentials: true });
}

export function logout() {
    return axios.post(`${endpoint}/logout`, {}, { withCredentials: true });
}

export function signup(email: string, username: string, password: string) {
    return axios.post<IAccount>(`${endpoint}/signup`, { email, username, password }, {
        withCredentials: true,
        params: {
            callbackUrl: `${window.location.origin}/account/confirm-email`
        }
    });
}