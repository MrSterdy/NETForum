import { FormEvent, useState } from "react";
import { Navigate, useNavigate } from "react-router-dom";

import { createTag } from "../../../api/endpoints/tags";

import { Loader } from "../../../components";

import { useAuth } from "../../../hooks";

export default function TagCreate() {
    const navigate = useNavigate();

    const { account } = useAuth();

    const [isLoading, setLoading] = useState(false);
    const [error, setError] = useState(false);

    if (!account!.admin)
        return <Navigate to="/" />;

    if (isLoading)
        return <Loader />;

    function submitForm(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();

        setLoading(true);

        const data = new FormData(event.currentTarget);

        createTag(data.get("name") as string)
            .then(() => navigate("/"))
            .catch(() => setError(true))
            .finally(() => setLoading(false));
    }

    return (
        <section className="submit-section main">
            <h1 className="title">Create tag</h1>

            <form className="column content" onSubmit={submitForm}>
                <div>
                    <h3 className="title">Tag name</h3>

                    <input className="full-width" type="text" name="name" minLength={2} maxLength={16} required />
                </div>

                {error && <span className="centered error">This name is already taken</span>}

                <button type="submit">Create</button>
            </form>
        </section>
    );
}