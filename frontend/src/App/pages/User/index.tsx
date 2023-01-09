import { useParams } from "react-router-dom";

import useFetch from "../../hooks/useFetch";
import { IUser } from "../../models";
import { Loader, Error } from "../../components";

export default function User() {
    const { data, isLoading, error } = useFetch<IUser>(`${process.env.REACT_APP_USER_ID_URL}/${useParams().id}`);

    if (isLoading)
        return <Loader />;

    if (error)
        return <Error message={ error } />;

    return (
        <section className="main">
            <h1 className="title">{ data!.username }</h1>

            <article className="content">TODO...</article>
        </section>
    );
}