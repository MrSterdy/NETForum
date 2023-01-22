import { useSearchParams } from "react-router-dom";

import { useFetch } from "../../../../hooks";

import { changeEmail } from "../../../../api/endpoints/account";

import { Error, Loader } from "../../../../components";

export default function ConfirmNewEmail() {
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    const [params, _] = useSearchParams();

    const { isLoading, error } = useFetch(changeEmail, params.get("code"));

    if (isLoading)
        return <Loader />;

    if (error)
        return <Error message="An error occurred" />;

    return <h1 className="title">Your new email has been confirmed</h1>;
}