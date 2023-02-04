import axios from "redaxios";

import { IPage, IThread } from "../../models";

const endpoint = process.env.REACT_APP_THREADS_URL!;

export function getThreadById(id: number) {
    return axios.get<IThread>(`${endpoint}/${id}`);
}

export function getThreads(page: number, userId?: number, title?: string, tagIds?: number[]) {
    const params: Record<string, any> = { page };

    if (userId)
        params.userId = userId;
    if (title)
        params.title = title;
    if (tagIds && tagIds.length !== 0)
        params.tagIds = tagIds;

    return axios.get<IPage<IThread>>(endpoint, { params });
}

export function createThread(title: string, content: string, tagIds: number[]) {
    return axios.post(endpoint, { title, content, tagIds }, { withCredentials: true });
}

export function updateThreadById(id: number, title: string, content: string, tagIds: number[]) {
    return axios.put(`${endpoint}/${id}`, { title, content, tagIds }, { withCredentials: true });
}

export function deleteThread(id: number) {
    return axios.delete(`${endpoint}/${id}`, { withCredentials: true });
}