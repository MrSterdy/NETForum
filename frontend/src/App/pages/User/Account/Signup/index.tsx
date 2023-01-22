import { FormEvent } from "react";
import { Link, Navigate } from "react-router-dom";

import { Error, Loader } from "../../../../components";

import { useAuth } from "../../../../hooks";

import "../index.css";

export default function Signup() {
    const { account, signUp, isLoading, error } = useAuth();

    if (account !== undefined)
        return account.confirmed ?
            <Navigate to="/" /> :
            <h1 className="title">Please confirm your email address</h1>;

    if (isLoading)
        return <Loader />;

    function submitForm(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const data = new FormData(event.currentTarget);

        signUp(data.get("email") as string, data.get("username") as string, data.get("password") as string);
    }

    return (
        <section className="submit-section main">
            <h1 className="title">Sign up</h1>

            <form className="column content" onSubmit={submitForm}>
                <div>
                    <h3 className="title">Email</h3>

                    <input className="full-width" type="email" name="email" required />
                </div>

                <div>
                    <h3 className="title">Username</h3>

                    <input className="full-width" type="text" name="username" minLength={4} maxLength={16} pattern="[a-zA-Z0-9-._@+]+" required />
                </div>

                <div>
                    <h3 className="title">Password</h3>

                    <input className="full-width" type="password" name="password" minLength={4} maxLength={16} required />
                </div>

                {!!error && <Error message="User with that email or username already exists" />}

                <button type="submit">Continue</button>

                <Link to="/account/login" className="centered">Log in</Link>
            </form>
        </section>
    );
}