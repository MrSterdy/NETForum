import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

import { Error, Loader } from "../../../components";

import "./index.css";

export default function Login() {
    const navigate = useNavigate();

    const [isInAuth, setInAuth] = useState(false);
    const [error, setError] = useState(false);

    if (isInAuth)
        return <Loader />;

    function submitForm(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        setInAuth(true);

        fetch(process.env.REACT_APP_AUTH_LOGIN_URL!, {
            method: "POST",
            credentials: "include",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(Object.fromEntries(new FormData(event.currentTarget)))
        })
            .then(response => response.ok ? navigate("/") : setError(true))
            .finally(() => setInAuth(false));
    }

    return (
        <section className="auth main">
            <h1 className="title">Login</h1>

            <form className="content" onSubmit={ submitForm }>
                <div>
                    <h3 className="title">Username</h3>

                    <input type="text" name="username" />
                </div>

                <div>
                    <h3 className="title">Password</h3>

                    <input type="password" name="password" />
                </div>

                { error && <Error message="Invalid username or password" /> }

                <button type="submit">Continue</button>
            </form>
        </section>
    );
}