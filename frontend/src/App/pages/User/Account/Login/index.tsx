import { FormEvent, useState } from "react";
import { Link, Navigate } from "react-router-dom";

import { Loader } from "../../../../components";

import { useAuth } from "../../../../hooks";

import { requestResetPassword } from "../../../../api/endpoints/account";

import "../index.css";

export default function Login() {
    const { account, logIn, isLoading, error } = useAuth();

    const [isResetSuccessful, setResetSuccessful] = useState(false);
    const [resetPassword, setResetPassword] = useState(false);
    const [resetPasswordError, setResetPasswordError] = useState(false);
    const [isResettingPassword, setResettingPassword] = useState(false);

    if (account?.emailConfirmed)
        return <Navigate to="/" />;

    if (isLoading || isResettingPassword)
        return <Loader />;

    function submitLoginForm(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const data = new FormData(event.currentTarget);

        logIn(data.get("username") as string, data.get("password") as string, data.has("rememberMe"));
    }

    function submitResetPasswordForm(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();

        setResettingPassword(true);

        const data = new FormData(event.currentTarget);

        requestResetPassword(data.get("email") as string)
            .then(() => setResetSuccessful(true))
            .catch(() => setResetPasswordError(true))
            .finally(() => setResettingPassword(false));
    }

    function startResettingPassword() {
        setResetPasswordError(false);

        setResetPassword(!resetPassword);
    }

    if (isResetSuccessful)
        return <h1 className="title">Please check your email</h1>;

    if (resetPassword)
        return (
            <section className="submit-section main">
                <h1 className="title">Reset password</h1>

                <form className="column content" onSubmit={submitResetPasswordForm}>
                    <div>
                        <h3 className="title">Email</h3>

                        <input className="full-width" type="email" name="email" required />
                    </div>

                    {resetPasswordError && <span className="centered error">User with this email doesn't exist</span>}

                    <div className="centered row">
                        <button type="submit">Submit</button>
                        <button type="button" onClick={startResettingPassword}>Cancel</button>
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

                {!!error && <span className="centered error">Invalid username or password</span>}

                <button type="submit">Continue</button>

                <Link to="/account/signup" className="centered">Sign up</Link>

                <span className="centered clickable" onClick={startResettingPassword}>Forgot Password?</span>
            </form>
        </section>
    );
}