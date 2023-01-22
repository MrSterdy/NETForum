import { FormEvent, useState } from "react";
import { Link, Navigate } from "react-router-dom";

import { Error, Loader } from "../../../../components";

import { useAuth } from "../../../../hooks";

import { requestResetPassword } from "../../../../api/endpoints/account";

import "../index.css";

export default function Login() {
    const { account, logIn, isLoading, error } = useAuth();

    const [isResetSuccessful, setResetSuccessful] = useState(false);
    const [resetPasswordError, setResetPasswordError] = useState(false);
    const [isResettingPassword, setResettingPassword] = useState(false);

    if (account?.confirmed)
        return <Navigate to="/" />;

    if (isLoading)
        return <Loader />;

    function submitLoginForm(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const data = new FormData(event.currentTarget);

        logIn(data.get("username") as string, data.get("password") as string, data.has("rememberMe"));
    }

    function submitResetPasswordForm(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const data = new FormData(event.currentTarget);

        requestResetPassword(data.get("email") as string)
            .then(() => setResetSuccessful(true))
            .catch(() => setResetPasswordError(true));
    }

    function resetPassword() {
        setResettingPassword(!isResettingPassword);
    }

    if (isResetSuccessful)
        return <h1 className="title">Please check your email</h1>;

    if (isResettingPassword)
        return (
            <section className="submit-section main">
                <h1 className="title">Reset password</h1>

                <form className="column content" onSubmit={submitResetPasswordForm}>
                    <div>
                        <h3 className="title">Email</h3>

                        <input className="full-width" type="email" name="email" required />
                    </div>

                    {resetPasswordError && <Error message="User with that email doesn't exist" />}

                    <div className="centered row">
                        <button type="submit">Submit</button>
                        <button type="button" onClick={resetPassword}>Cancel</button>
                    </div>
                </form>
            </section>
        );

    return (
        <section className="submit-section main">
            <h1 className="title">Log in</h1>

            <form className="column content" onSubmit={submitLoginForm}>
                <div>
                    <h3 className="title">Username</h3>

                    <input className="full-width" type="text" name="username" required />
                </div>

                <div>
                    <h3 className="title">Password</h3>

                    <input className="full-width" type="password" name="password" required />
                </div>

                <div className="full-width row">
                    <input type="checkbox" name="rememberMe" />

                    <span>Remember me</span>
                </div>

                { !!error && <Error message="Invalid username or password" /> }

                <button type="submit">Continue</button>

                <Link to="/account/signup" className="centered">Sign up</Link>

                <span className="centered clickable" onClick={resetPassword}>Forgot Password?</span>
            </form>
        </section>
    );
}