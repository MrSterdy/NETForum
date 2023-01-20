import axios from "redaxios";

import { IUser } from "../../models";

export async function getAccount() {
    return await axios.get<IUser>(process.env.REACT_APP_ACCOUNT_URL!, { withCredentials: true });
}

export async function changeEmail(email: string) {
    return await axios.post(
        `${process.env.REACT_APP_ACCOUNT_URL}/change/email?clientUrl=${window.location.origin}/account/confirmNewEmail`,
        { email },
        { withCredentials: true }
    );
}

export async function confirmChangeEmail(code: string, email: string) {
    return await axios.patch(`${process.env.REACT_APP_ACCOUNT_URL}/email?code=${encodeURIComponent(code)}`, { email }, {
        withCredentials: true
    });
}

export async function changePassword(password: string, newPassword: string) {
    return await axios.patch(`${process.env.REACT_APP_ACCOUNT_URL}/password`, { password, newPassword }, {
        withCredentials: true
    });
}

export async function changeUsername(username: string) {
    return await axios.patch(`${process.env.REACT_APP_ACCOUNT_URL}/username`, { username }, {
        withCredentials: true
    });
}