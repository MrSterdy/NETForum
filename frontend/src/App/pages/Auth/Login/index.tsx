import { FormEvent } from "react";
import {Link, Navigate } from "react-router-dom";

import { Error, Loader } from "../../../components";

import "../index.css";

import useAuth from "../../../hooks/useAuth";

export default function Login() {
    const { user, logIn, isLoading, error } = useAuth();

    if (user?.confirmed)
        return <Navigate to="/" />;

    if (isLoading)
        return <Loader />;

    function submitForm(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const data = new FormData(event.currentTarget);

        logIn({
            username: data.get("username") as string,
            password: data.get("password") as string
        });
    }

    return (
        <section className="auth main">
            <h1 className="title">Log in</h1>

            <form className="content" onSubmit={ submitForm }>
                <div>
                    <h3 className="title">Username</h3>

                    <input type="text" name="username" />
                </div>

                <div>
                    <h3 className="title">Password</h3>

                    <input type="password" name="password" />
                </div>

                { !!error && <Error message="Invalid username or password" /> }

                <Link to="/signup">Sign up</Link>

                <button type="submit">Continue</button>
            </form>
        </section>
    );
}