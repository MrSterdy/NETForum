import axios from "redaxios";

import { IUser } from "../models";

export async function getUserById(id: number) {
    return await axios.get<IUser>(`${process.env.REACT_APP_USERS_URL}/${id}`);
}