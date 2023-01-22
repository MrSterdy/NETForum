import { FormEvent, useState } from "react";

import { useAuth } from "../../../hooks";

import ProfilePic from "../../../assets/icons/profile-pic.png";
import { ReactComponent as Edit  } from "../../../assets/icons/pencil.svg";

import * as accountApi from "../../../api/endpoints/account";

import "./index.css";
import "../index.css";

export default function Account() {
    const { account } = useAuth();

    const [isEmailSubmitted, setEmailSubmitted] = useState(false);
    const [isChangingEmail, setChangingEmail] = useState(false);

    const [isChangingUserName, setChangingUserName] = useState(false);

    const [passwordError, setPasswordError] = useState(false);
    const [isChangingPassword, setChangingPassword] = useState(false);
    const [isResettingPassword, setResettingPassword] = useState(false);

    if (isEmailSubmitted)
        return <h1 className="title">Please check your Email</h1>;

    function changeEmail() {
        setChangingEmail(!isChangingEmail);
    }
    function submitNewEmail(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const data = new FormData(event.currentTarget);

        accountApi.requestChangeEmail(data.get("email") as string)
            .then(() => setEmailSubmitted(true));
    }

    function changeUserName() {
        setChangingUserName(!isChangingUserName);
    }
    function submitNewUserName(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const data = new FormData(event.currentTarget);

        accountApi.changeUsername(data.get("username") as string)
            .then(() => window.location.reload());
    }

    function changePassword() {
        setChangingPassword(!isChangingPassword);
    }
    function submitNewPassword(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const data = new FormData(event.currentTarget);

        accountApi.changePassword(data.get("password") as string, data.get("new-password") as string)
            .then(() => window.location.reload())
            .catch(() => setPasswordError(true));
    }
    function resetPassword() {
        accountApi.requestResetPassword(account!.email)
            .then(() => setResettingPassword(true));
    }

    if (isChangingEmail)
        return (
            <section className="submit-section main">
                <h2 className="title">Change Email</h2>

                <form className="content column" onSubmit={submitNewEmail}>
                    <div>
                        <h3 className="title">New Email</h3>

                        <input className="full-width" type="email" name="email" required />
                    </div>

                    <div className="centered row">
                        <button type="submit">Submit</button>
                        <button type="button" onClick={changeEmail}>Cancel</button>
                    </div>
                </form>
            </section>
        );

    if (isChangingUserName)
        return (
            <section className="submit-section main">
                <h2 className="title">Change Username</h2>

                <form className="content column" onSubmit={submitNewUserName}>
                    <div>
                        <h3 className="title">New Username</h3>

                        <input className="full-width" type="text" name="username" minLength={4} maxLength={16} pattern="[a-zA-Z0-9-._@+]+" required />
                    </div>

                    <div className="centered row">
                        <button type="submit">Submit</button>
                        <button type="button" onClick={changeUserName}>Cancel</button>
                    </div>
                </form>
            </section>
        );

    if (isResettingPassword)
        return <h1 className="title">Please check your email</h1>;

    if (isChangingPassword)
        return (
            <section className="submit-section main">
                <h2 className="title">Change Password</h2>

                <form className="content column" onSubmit={submitNewPassword}>
                    <div>
                        <h3 className="title">Password</h3>

                        <input className="full-width" type="password" name="password" required />
                    </div>

                    <div>
                        <h3 className="title">New Password</h3>

                        <input className="full-width" type="password" name="new-password" minLength={4} maxLength={16} required />
                    </div>

                    {passwordError && <span className="centered error">Old password is incorrect</span>}

                    <div className="centered row">
                        <button type="submit">Submit</button>
                        <button type="button" onClick={changePassword}>Cancel</button>
                    </div>

                    <span className="clickable centered" onClick={resetPassword}>Forgot password?</span>
                </form>
            </section>
        );

    return (
        <section className="user-profile main">
            <section className="user-header content row">
                <div className="center">
                    <img src={ProfilePic} className="user-avatar" alt=""/>
                </div>

                <section className="user-description column">
                    <div className="column">
                        <h3 className="title">Email address:</h3>

                        <div className="user-item row">
                            <h4 className="description">{account!.email}</h4>
                            <Edit className="clickable icon" onClick={changeEmail} />
                        </div>
                    </div>

                    <div className="column">
                        <h3 className="title">Username:</h3>

                        <div className="user-item row">
                            <h4 className="description">{account!.userName}</h4>
                            <Edit className="clickable icon" onClick={changeUserName} />
                        </div>
                    </div>

                    <div className="column">
                        <h3 className="title">Password:</h3>

                        <div className="user-item row">
                            <h4 className="description">****************</h4>
                            <Edit className="clickable icon" onClick={changePassword} />
                        </div>
                    </div>
                </section>
            </section>
        </section>
    );
}