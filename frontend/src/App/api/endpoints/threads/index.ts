import axios from "redaxios";

import { IPage, IThread } from "../../models";

const endpoint = process.env.REACT_APP_THREADS_URL!;

export function getThreadById(id: number) {
    return axios.get<IThread>(`${endpoint}/${id}`);
}

export function getThreadsByPage(page: number) {
    return axios.get<IPage<IThread>>(endpoint, { params: { page } });
}

export function getThreadsByUserId(userId: number, page: number) {
    return axios.get<IPage<IThread>>(endpoint, { params: { userId, page } });
}

export function search(title: string, page: number) {
    return axios.get<IPage<IThread>>(`${endpoint}/search`, { params: { title, page } });
}

export function createThread(title: string, content: string) {
    return axios.post(endpoint, { title, content }, { withCredentials: true });
}

export function updateThreadById(id: number, title: string, content: string) {
    return axios.put(`${endpoint}/${id}`, { title, content }, { withCredentials: true });
}

export function deleteThread(id: number) {
    return axios.delete(`${endpoint}/${id}`, { withCredentials: true });
}