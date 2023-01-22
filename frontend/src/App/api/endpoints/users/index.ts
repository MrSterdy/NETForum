import axios from "redaxios";

import { IUser } from "../../models";

const endpoint = process.env.REACT_APP_USERS_URL!;

export async function getUserById(id: number) {
    return await axios.get<IUser>(`${endpoint}/${id}`);
}