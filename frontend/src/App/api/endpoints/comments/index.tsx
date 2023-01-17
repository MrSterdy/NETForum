import axios from "redaxios";

import { IPage, IComment } from "../../models";

interface CommentProps {
    threadId: number,
    content: string
}

export function createComment(props: CommentProps) {
    return axios.post(process.env.REACT_APP_COMMENTS_URL!, props, { withCredentials: true });
}

export function updateCommentById(id: number, props: CommentProps) {
    return axios.put(`${process.env.REACT_APP_COMMENTS_URL}/${id}`, props, { withCredentials: true });
}

export function getCommentsByPage(page: number, threadId: number) {
    return axios.get<IPage<IComment>>(process.env.REACT_APP_COMMENTS_URL!, {
        params: {
            page,
            thread: threadId
        }
    });
}

export function deleteCommentById(id: number) {
    return axios.delete(`${process.env.REACT_APP_COMMENTS_URL}/${id}`, { withCredentials: true });
}