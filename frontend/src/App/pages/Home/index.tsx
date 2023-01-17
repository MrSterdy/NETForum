import { useEffect, useState } from "react";
import { Link } from "react-router-dom";

import { Error, Loader } from "../../components";

import { IPage, IThread } from "../../api/models";

import { getThreadsByPage } from "../../api/endpoints/threads";

import { useAuth } from "../../hooks";

export default function Home() {
    const [page, setPage] = useState<IPage<IThread>>({} as IPage<IThread>);
    const [pageNumber, setPageNumber] = useState(1);
    const [isLoading, setLoading] = useState(true);
    const [error, setError] = useState("");

    const { user } = useAuth();

    useEffect(() => {
        getThreadsByPage(pageNumber)
            .then(res => setPage(p => ({
                items: p.items ? p.items.concat(res.data.items) : res.data.items,
                isLast: res.data.isLast
            })))
            .catch(err => setError((err as Error).message))
            .finally(() => setLoading(false));
    }, [pageNumber]);

    const loadMore = () => setPageNumber(pageNumber + 1);
    
    if (isLoading) 
        return <Loader />;
    
    if (error)
        return <Error message={ error } />;
    
    return (
        <section className="main threads">
            <div>
                <h1 className="title">Recent threads</h1>

                { user?.confirmed &&
                    <h3 className="description">
                        <Link to="/thread/create">Create new thread</Link>
                    </h3>
                }
            </div>

            <ul className="content column thread-list">
                { page.items.map(thread => (
                    <li key={ thread.id }>
                        <h2 className="title">
                            <Link to={ `thread/${thread.id}` }>{ thread.title }</Link>
                        </h2>
                        <h3 className="description">
                            <Link to={ `user/${thread.user.id}` }>{ thread.user.userName }</Link>
                        </h3>
                    </li>
                )) }
            </ul>

            { !page.isLast && <button type="button" onClick={ loadMore } className="centered">Load more</button> }
        </section>
    );
}