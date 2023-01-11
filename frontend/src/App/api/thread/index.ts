import axios from "redaxios";

import { IPage, IThread } from "../models";

export async function getThreadById(id: number) {
    return axios.get<IThread>(`${process.env.REACT_APP_THREAD_ID_URL}/${id}`);
}

export async function getThreadsByPage(page: number) {
    return axios.get<IPage<IThread>>(`${process.env.REACT_APP_THREAD_PAGE_URL}/${page}`);
}