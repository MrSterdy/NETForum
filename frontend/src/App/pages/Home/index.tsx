import { useEffect, useState } from "react";
import axios from "axios";

import { Error, Title, Button, Loader } from "../../components";
import { IPage, IThread } from "../../models";

import "./index.css";

export default function Home() {
    const [page, setPage] = useState<IPage<IThread>>();
    const [pageNumber, setPageNumber] = useState(1);
    const [isLoading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        axios.get<IPage<IThread>>(`${process.env.REACT_APP_THREAD_PAGE_URL}/${pageNumber}`)
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
        <>
            <Title title="Recent threads" />
            
            <ul className="main thread-list">
                { page!.items.map(thread => (
                    <li key={ thread.id }>
                        <h3 className="title"><a href="#">{ thread.title }</a></h3>
                        <h4 className="description"><a href="#">{ thread.user!.username }</a></h4>
                    </li>
                )) }
            </ul>

            { !page!.isLast && <Button name="Load more" onClick={ loadMore } /> }
        </>
    );
}