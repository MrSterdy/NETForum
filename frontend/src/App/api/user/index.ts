import axios from "redaxios";

import { IUser } from "../models";

export async function getCurrentUser() {
    return await axios.get<IUser>(process.env.REACT_APP_CURRENT_USER_URL!, { withCredentials: true });
}

export async function getUserById(id: number) {
    return await axios.get<IUser>(`${process.env.REACT_APP_USER_ID_URL}/${id}`);
}