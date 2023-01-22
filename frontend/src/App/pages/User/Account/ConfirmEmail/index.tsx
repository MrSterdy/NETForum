import { useSearchParams } from "react-router-dom";

import { useFetch } from "../../../../hooks";

import { confirmEmail } from "../../../../api/endpoints/account";

import { Error, Loader } from "../../../../components";

export default function ConfirmEmail() {
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    const [searchParams, _] = useSearchParams();

    const { isLoading, error } = useFetch(confirmEmail, parseInt(searchParams.get("userId")!), searchParams.get("code"));

    if (isLoading)
        return <Loader />;

    if (error)
        return <Error message="Not found" />;

    return <h1 className="title">Your email address has been confirmed</h1>;
}