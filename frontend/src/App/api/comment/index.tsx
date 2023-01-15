import axios from "redaxios";

interface CommentProps {
    threadId: number,
    content: string
}

export default function createComment(props: CommentProps) {
    return axios.post(process.env.REACT_APP_COMMENTS_URL!, props, { withCredentials: true });
}