import { useSearchParams } from "react-router-dom";

import { useFetch } from "../../../../hooks";

import { confirmEmail } from "../../../../api/endpoints/account";

import { Loader } from "../../../../components";

export default function ConfirmEmail() {
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    const [searchParams, _] = useSearchParams();

    const { isLoading, error } = useFetch(confirmEmail, parseInt(searchParams.get("userId")!), searchParams.get("code"));

    if (isLoading)
        return <Loader />;

    if (error)
        return <h1 className="error title">An error occurred. Please try again later</h1>;

    return <h1 className="title">Your email address has been confirmed</h1>;
}