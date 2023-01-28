import axios from "redaxios";

import { IUser } from "../../models";

const endpoint = process.env.REACT_APP_USERS_URL!;

export function getUserById(id: number) {
    return axios.get<IUser>(`${endpoint}/${id}`);
}

export async function banById(id: number) {
    return axios.post(`${endpoint}/ban/${id}`, {}, { withCredentials: true });
}