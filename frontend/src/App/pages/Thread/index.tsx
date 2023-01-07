import { Link, useParams } from "react-router-dom";

import useFetch from "../../hooks/useFetch";
import { IThread } from "../../models";
import { Error, Loader, Title } from "../../components";

export default function Thread() {
    const { data, isLoading, error } =
        useFetch<IThread>(`${process.env.REACT_APP_THREAD_ID_URL}/${useParams().id}`);

    if (isLoading)
        return <Loader />;

    if (error)
        return <Error message={ error } />

    return (
        <section className="main">
            <div>
                <Title title={ data!.title } />

                <h3 className="description">
                    <Link to={ `user/${data!.userId}` }>{ data!.user!.username }</Link>
                </h3>
            </div>

            <article className="content">{ data!.content }</article>
        </section>
    );
}