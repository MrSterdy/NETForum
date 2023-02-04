import React, { FormEvent, useState } from "react";
import { Navigate, useNavigate, useParams } from "react-router-dom";

import { deleteTagById, getTagById, updateTagById } from "../../../api/endpoints/tags";

import { Loader } from "../../../components";

import { useAuth, useFetch } from "../../../hooks";

export default function TagEdit() {
    const { account } = useAuth();

    const id = parseInt(useParams().id!);

    const {
        data: tag,
        isLoading: isTagLoading,
        error: tagError
    } = useFetch(getTagById, id);

    const navigate = useNavigate();

    const [isLoading, setLoading] = useState(false);
    const [error, setError] = useState(false);

    if (!account!.admin)
        return <Navigate to="/" />;

    if (isTagLoading || isLoading)
        return <Loader />;

    if (!!tagError)
        return <h1 className="error title">Tag Not Found</h1>;

    function submitForm(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();

        setLoading(true);

        const data = new FormData(event.currentTarget);

        updateTagById(id, data.get("name") as string)
            .then(() => navigate("/"))
            .catch(() => setError(true))
            .finally(() => setLoading(false));
    }

    function deleteTag() {
        setLoading(true);

        deleteTagById(id)
            .then(() => navigate("/"))
            .finally(() => setLoading(false));
    }

    return (
        <section className="submit-section main">
            <h1 className="title">Edit tag</h1>

            <form className="column content" onSubmit={submitForm}>
                <div>
                    <h3 className="title">Tag name</h3>

                    <input className="full-width" type="text" name="name" defaultValue={tag.name} minLength={2} maxLength={16} required />
                </div>

                {error && <span className="centered error">This name is already taken</span>}

                <div className="centered row">
                    <button type="submit">Submit</button>
                    <button className="full-width" type="button" onClick={deleteTag}>Delete</button>
                </div>
            </form>
        </section>
    );
}