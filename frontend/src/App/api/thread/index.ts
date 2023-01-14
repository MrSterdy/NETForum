import axios from "redaxios";

import { IPage, IThread } from "../models";

export async function getThreadById(id: number) {
    return axios.get<IThread>(`${process.env.REACT_APP_THREADS_URL}/${id}`);
}

export async function getThreadsByPage(page: number) {
    return axios.get<IPage<IThread>>(`${process.env.REACT_APP_THREADS_URL}?page=${page}`);
}

interface ThreadParams {
    title: string,
    content: string
}

export async function createThread(params: ThreadParams) {
    return axios.post(process.env.REACT_APP_THREADS_URL!, params, { withCredentials: true });
}