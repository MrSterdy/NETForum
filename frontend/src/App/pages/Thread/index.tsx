import { Link, useParams } from "react-router-dom";

import { IThread } from "../../api/models";
import { Error, Loader } from "../../components";
import { getThreadById } from "../../api/thread";
import useFetch from "../../hooks/useFetch";

export default function Thread() {
    const { data, isLoading, error } = useFetch<IThread>(getThreadById, parseInt(useParams().id!));

    if (isLoading)
        return <Loader />;

    if (error)
        return <Error message="Thread not found" />

    return (
        <section className="main">
            <div>
                <h1 className="title">{ data.title }</h1>

                <h3 className="description">
                    <Link to={ `user/${data.user.id}` }>{ data.user.userName }</Link>
                </h3>
            </div>

            <article className="content">{ data.content }</article>
        </section>
    );
}