import {useEffect, useState } from "react";
import axios from "axios";

import Error from "../../components/Error"
import Title from "../../components/Title";
import { IPage, IThread } from "../../models";
import Button from "../../components/Button";
import Loader from "../../components/Loader";

import "./index.css";

export default function Home() {
    const [page, setPage] = useState<IPage<IThread>>();
    const [pageNumber, setPageNumber] = useState(1);
    const [isLoading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        const fetchThreads = async () => {
            try {
                const res = await axios.get<IPage<IThread>>(`https://localhost:7191/api/thread/page/${pageNumber}`);

                setPage({
                    items: page ? page.items.concat(res.data.items) : res.data.items,
                    isLast: res.data.isLast
                });
            } catch (e: unknown) {
                setError((e as Error).message);
            } finally {
                setLoading(false);
            }
        };
        
        fetchThreads();
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

            { !page!.isLast && <Button name="Load more" onClick={loadMore}/> }
        </>
    );
}