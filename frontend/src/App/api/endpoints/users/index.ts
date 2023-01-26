import axios from "redaxios";

import { IUser } from "../../models";

const endpoint = process.env.REACT_APP_USERS_URL!;

export function getUserById(id: number) {
    return axios.get<IUser>(`${endpoint}/${id}`);
}

export async function blockById(id: number) {
    return axios.post(`${endpoint}/block/${id}`, {}, { withCredentials: true });
}