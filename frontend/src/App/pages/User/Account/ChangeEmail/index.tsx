import { useSearchParams } from "react-router-dom";

import { useFetch } from "../../../../hooks";

import { changeEmail } from "../../../../api/endpoints/account";

import { Loader } from "../../../../components";

export default function ChangeEmail() {
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    const [params, _] = useSearchParams();

    const { isLoading, error } = useFetch(changeEmail, params.get("code"));

    if (isLoading)
        return <Loader />;

    if (error)
        return <h1 className="error title">An error occurred. Please try again later</h1>;

    return <h1 className="title">Your new email has been confirmed</h1>;
}