import { FormEvent, useState } from "react";
import { useSearchParams } from "react-router-dom";

import { resetPassword } from "../../../../api/endpoints/account";

import { Loader } from "../../../../components";

import "../index.css"

export default function ResetPassword() {
    const [error, setError] = useState(false);
    const [isLoading, setLoading] = useState(false);
    const [isSuccessful, setSuccessful] = useState(false);

    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    const [params, _] = useSearchParams();

    if (isLoading)
        return <Loader />;

    function submitForm(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();

        setLoading(true);

        const data = new FormData(event.currentTarget);

        resetPassword(parseInt(params.get("userId")!), params.get("code")!, data.get("new-password") as string)
            .then(() => setSuccessful(true))
            .catch(() => setError(true))
            .finally(() => setLoading(false));
    }

    if (isSuccessful)
        return <h1 className="title">Your password has been changed</h1>;

    return (
        <section className="submit-section main">
            <h1 className="title">Reset Password</h1>

            <form className="column content" onSubmit={submitForm}>
                <div>
                    <h3 className="title">New Password</h3>

                    <input className="full-width" type="password" name="new-password" minLength={4} maxLength={16} required />
                </div>

                {error && <span className="centered error">An error occurred. Please try again later</span>}

                <button type="submit">Continue</button>
            </form>
        </section>
    );
}