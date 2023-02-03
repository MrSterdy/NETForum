import axios from "redaxios";

import { IPage, IUser } from "../../models";

const endpoint = process.env.REACT_APP_USERS_URL!;

export function getUserById(id: number) {
    return axios.get<IUser>(`${endpoint}/${id}`);
}

export function searchUsers(page: number, username: string) {
    return axios.get<IPage<IUser>>(`${endpoint}/search`, { params: { page, username } });
}

export function banById(id: number) {
    return axios.post(`${endpoint}/ban/${id}`, {}, { withCredentials: true });
}