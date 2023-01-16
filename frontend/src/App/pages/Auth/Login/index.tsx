import { FormEvent } from "react";
import { Link, Navigate } from "react-router-dom";

import { Error, Loader } from "../../../components";

import { useAuth } from "../../../hooks";

import "../index.css";

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
            password: data.get("password") as string,
            rememberMe: data.has("rememberMe")
        });
    }

    return (
        <section className="auth main">
            <h1 className="title">Log in</h1>

            <form className="column content" onSubmit={ submitForm }>
                <div>
                    <h3 className="title">Username</h3>

                    <input type="text" name="username" />
                </div>

                <div>
                    <h3 className="title">Password</h3>

                    <input type="password" name="password" />
                </div>

                <div className="center row">
                    <input type="checkbox" name="rememberMe" />

                    <span>Remember me</span>
                </div>

                { !!error && <Error message="Invalid username or password" /> }

                <button type="submit">Continue</button>

                <Link to="/signup" className="centered">Sign up</Link>
            </form>
        </section>
    );
}