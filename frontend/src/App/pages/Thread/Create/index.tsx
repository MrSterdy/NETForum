import React, { FormEvent, useState } from "react";
import { Tag } from "react-tag-input";
import { Navigate } from "react-router-dom";

import { createThread } from "../../../api/endpoints/threads";

import { Loader, TagInput } from "../../../components";

import "./index.css";

export default function ThreadCreate() {
    const [error, setError] = useState(false);
    const [isSent, setSent] = useState(false);
    const [isLoading, setLoading] = useState(false);

    const [searchTags, setSearchTags] = useState<Tag[]>([]);

    if (isLoading)
        return <Loader />;

    if (isSent)
        return <Navigate to="/" />;

    function submitForm(event: FormEvent<HTMLFormElement>) {
        setLoading(true);

        event.preventDefault();

        const data = new FormData(event.currentTarget);

        createThread(
            data.get("title") as string,
            data.get("content") as string,
            searchTags.map(t => parseInt(t.id))
        )
            .then(() => setSent(true))
            .catch(() => setError(true))
            .finally(() => setLoading(false));
    }

    return (
        <form className="thread-create main" onSubmit={submitForm}>
            <div>
                <h2 className="title">Title:</h2>

                <input className="full-width" type="text" name="title" minLength={4} maxLength={127} required />
            </div>

            <div>
                <h2 className="title">Content:</h2>

                <textarea className="full-width" name="content" minLength={4} maxLength={32767} required></textarea>
            </div>

            <div className="center row">
                <h3 className="title">Tags:</h3>

                <TagInput tags={searchTags} setTags={setSearchTags} />
            </div>

            {error && <span className="centered error">An error occurred. Please try again later</span>}

            <button type="submit" className="centered">Create new thread</button>
        </form>
    );
}