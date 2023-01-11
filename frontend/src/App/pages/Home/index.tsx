import { useEffect, useState } from "react";
import { Link } from "react-router-dom";

import { Error, Loader } from "../../components";
import { IPage, IThread } from "../../api/models";
import { getThreadsByPage } from "../../api/thread";

import "./index.css";

export default function Home() {
    const [page, setPage] = useState<IPage<IThread>>();
    const [pageNumber, setPageNumber] = useState(1);
    const [isLoading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        getThreadsByPage(pageNumber)
            .then(res => setPage(p => ({
                items: p ? p.items.concat(res.data.items) : res.data.items,
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
            <h1 className="title">Recent threads</h1>
            
            <ul className="content thread-list">
                { page!.items.map(thread => (
                    <li key={ thread.id }>
                        <h3 className="title">
                            <Link to={ `thread/${thread.id}` }>{ thread.title }</Link>
                        </h3>
                        <h4 className="description">
                            <Link to={ `user/${thread.userId}` }>{ thread.user!.username }</Link>
                        </h4>
                    </li>
                )) }
            </ul>

            { !page!.isLast && <button type="button" onClick={ loadMore }>Load more</button> }
        </section>
    );
}