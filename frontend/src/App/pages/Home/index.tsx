import { useEffect, useState } from "react";
import { Link } from "react-router-dom";

import { Loader } from "../../components";

import { IPage, IThread } from "../../api/models";

import { getThreadsByPage } from "../../api/endpoints/threads";

import { useAuth } from "../../hooks";
import dayjs from "dayjs";

export default function Home() {
    const [page, setPage] = useState<IPage<IThread>>({} as IPage<IThread>);
    const [pageNumber, setPageNumber] = useState(1);

    const [isPageLoading, setPageLoading] = useState(true);

    const [error, setError] = useState(false);

    const { account } = useAuth();

    useEffect(() => {
        setPageLoading(true);

        getThreadsByPage(pageNumber)
            .then(res => setPage(p => ({
                items: p.items ? p.items.concat(res.data.items) : res.data.items,
                isLast: res.data.isLast
            })))
            .catch(() => setError(true))
            .finally(() => setPageLoading(false));
    }, [pageNumber]);

    if (isPageLoading && pageNumber === 1)
        return <Loader />;
    
    if (error)
        return <h1 className="error title">Fetch Failed</h1>;

    function loadMore() {
        setPageNumber(pageNumber + 1);
    }
    
    return (
        <section className="main threads">
            <div>
                <h1 className="title">Recent threads</h1>

                {account?.confirmed &&
                    <h3 className="description">
                        <Link to="/thread/create">Create new thread</Link>
                    </h3>
                }
            </div>

            <ul className="content column thread-list">
                {page.items.map(thread => (
                    <li key={ thread.id }>
                        <h2 className="title">
                            <Link to={`/thread/${thread.id}`}>{thread.title}</Link>
                        </h2>

                        <div className="info-bar row">
                            <h3 className="description">
                                <Link to={`/user/${thread.user.id}`}>
                                    {thread.user.enabled ? thread.user.userName : <s>{thread.user.userName}</s>}
                                </Link>
                            </h3>

                            <h3 className="description">{dayjs(thread.createdDate).calendar()}</h3>
                        </div>
                    </li>
                ))}
            </ul>

            {isPageLoading ?
                <Loader /> :
                (!page.isLast &&
                    <button type="button" onClick={loadMore}>Load more</button>
                )
            }
        </section>
    );
}