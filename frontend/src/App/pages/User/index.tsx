import { useParams } from "react-router-dom";

import { IUser } from "../../api/models";
import { Loader, Error } from "../../components";
import { getUserById } from "../../api/user";
import useFetch from "../../hooks/useFetch";

export default function User() {
    const { data, isLoading, error } = useFetch<IUser>(getUserById, parseInt(useParams().id!));

    if (isLoading)
        return <Loader />;

    if (error)
        return <Error message="User not found" />;

    return (
        <section className="main">
            <h1 className="title">{ data.userName }</h1>

            <article className="content">TODO...</article>
        </section>
    );
}