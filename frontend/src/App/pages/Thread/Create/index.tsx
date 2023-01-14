import { FormEvent, useState } from "react";
import { Navigate } from "react-router-dom";

import { createThread } from "../../../api/thread";

import "./index.css";
import { Loader } from "../../../components";

export default function ThreadCreate() {
    const [isSent, setSent] = useState(false);
    const [isLoading, setLoading] = useState(false);

    if (isLoading)
        return <Loader />;

    if (isSent)
        return <Navigate to="/" />

    function submitForm(event: FormEvent<HTMLFormElement>) {
        setLoading(true);

        event.preventDefault();

        const data = new FormData(event.currentTarget);

        createThread({
            title: data.get("title") as string,
            content: data.get("content") as string
        })
            .then(() => setSent(true))
            .finally(() => setLoading(false));
    }

    return (
        <form className="thread-create main" onSubmit={ submitForm }>
            <input type="text" name="title" minLength={4} maxLength={127} required />

            <textarea name="content" minLength={4} maxLength={32767} required></textarea>

            <button type="submit" className="centered">Create new thread</button>
        </form>
    );
}