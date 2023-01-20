import { useSearchParams } from "react-router-dom";

import { useFetch } from "../../../hooks";

import { confirmChangeEmail } from "../../../api/endpoints/account";

import { Error, Loader } from "../../../components";

export default function ConfirmNewEmail() {
    const [params, _] = useSearchParams();

    const { isLoading, error } = useFetch(confirmChangeEmail, params.get("code"), params.get("email"));

    if (isLoading)
        return <Loader />;

    if (error)
        return <Error message="An error occurred" />;

    return <h1 className="title">Your new email has been confirmed</h1>;
}