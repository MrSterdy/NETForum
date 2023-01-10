import React, { useState } from "react";

import { Error, Loader } from "../../../components";

import "../index.css";

export default function Register() {
    const [isInProcess, setInProcess] = useState(false);
    const [error, setError] = useState(0);
    const [isRegistered, setRegistered] = useState(false);

    if (isInProcess)
        return <Loader />;

    function submitForm(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        setInProcess(true);

        fetch(process.env.REACT_APP_AUTH_REGISTER_URL!, {
            method: "POST",
            credentials: "include",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(Object.fromEntries(new FormData(event.currentTarget)))
        })
            .then(response => response.ok ? setRegistered(true) : setError(response.status))
            .finally(() => setInProcess(false));
    }

    return (
        <>
            { isRegistered &&
                <h1 className="title">Please verify your email address</h1>
            }

            { !isRegistered &&
                <section className="auth main">
                    <h1 className="title">Register</h1>

                    <form className="content" onSubmit={ submitForm }>
                        <div>
                            <h3 className="title">Email</h3>

                            <input type="email" name="email" required />
                        </div>

                        <div>
                            <h3 className="title">Username</h3>

                            <input type="text" name="username" minLength={4} maxLength={16} pattern="[a-zA-Z0-9-._@+]+" required />
                        </div>

                        <div>
                            <h3 className="title">Password</h3>

                            <input type="password" name="password" minLength={4} maxLength={16} required />
                        </div>

                        { !!error && <Error message={ error === 409 ? "User with that email or username already exists" : "Invalid form" } /> }

                        <button type="submit">Continue</button>
                    </form>
                </section>
            }
        </>
    );
}