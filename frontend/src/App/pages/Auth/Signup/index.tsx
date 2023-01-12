import { FormEvent } from "react";
import { Navigate } from "react-router-dom";

import { Error, Loader } from "../../../components";

import "../index.css";

import useAuth from "../../../hooks/useAuth";

export default function Signup() {
    const { user, signUp, isLoading, error } = useAuth();

    if (user)
        return user.confirmed ?
            <Navigate to="/" /> :
            <h1 className="title">Please confirm your email address</h1>;

    if (isLoading)
        return <Loader />;

    function submitForm(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const data = new FormData(event.currentTarget);

        signUp({
            email: data.get("email") as string,
            username: data.get("username") as string,
            password: data.get("password") as string
        });
    }

    return (
        <>
            <section className="auth main">
                <h1 className="title">Sign up</h1>

                <form className="column content" onSubmit={ submitForm }>
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

                    { !!error && <Error message="User with that email or username already exists" /> }

                    <button type="submit">Continue</button>
                </form>
            </section>
        </>
    );
}