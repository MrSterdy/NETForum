import { useSearchParams } from "react-router-dom";

import { useFetch } from "../../../hooks";

import { confirmEmail } from "../../../api/endpoints/auth";

import { Error, Loader } from "../../../components";

export default function Confirm() {
    const [searchParams, _] = useSearchParams();

    const { isLoading, error } = useFetch(confirmEmail, {
        userId: parseInt(searchParams.get("userId")!),
        code: searchParams.get("code")
    });

    if (isLoading)
        return <Loader />;

    if (error)
        return <Error message="Not found" />;

    return <h1 className="title">Your email address has been confirmed</h1>;
}