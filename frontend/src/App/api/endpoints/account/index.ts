import axios from "redaxios";

import { IUser } from "../../models";

export async function getAccount() {
    return await axios.get<IUser>(process.env.REACT_APP_ACCOUNT_URL!, { withCredentials: true });
}