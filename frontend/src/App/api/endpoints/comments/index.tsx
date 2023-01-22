import axios from "redaxios";

import { IPage, IComment } from "../../models";

const endpoint = process.env.REACT_APP_COMMENTS_URL!;

export function createComment(threadId: number, content: string) {
    return axios.post(endpoint, { content }, { withCredentials: true, params: { threadId } });
}

export function updateCommentById(id: number, content: string) {
    return axios.put(`${endpoint}/${id}`, { content }, { withCredentials: true });
}

export function getCommentsByPage(page: number, threadId: number) {
    return axios.get<IPage<IComment>>(endpoint, { params: { page, threadId } });
}

export function deleteCommentById(id: number) {
    return axios.delete(`${endpoint}/${id}`, { withCredentials: true });
}