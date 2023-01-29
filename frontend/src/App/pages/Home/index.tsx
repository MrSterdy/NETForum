import { MouseEvent as RMouseEvent, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import dayjs from "dayjs";

import { Loader } from "../../components";

import { IPage, IThread } from "../../api/models";

import { getThreadsByPage, search as searchByTitle } from "../../api/endpoints/threads";

import { useAuth } from "../../hooks";

import { ReactComponent as Views } from "../../assets/icons/eye.svg";
import { ReactComponent as Comments } from "../../assets/icons/comment.svg";
import { ReactComponent as Search } from "../../assets/icons/search.svg";

import "./index.css";

export default function Home() {
    const [mainPage, setMainPage] = useState<IPage<IThread>>({} as IPage<IThread>);
    const [mainPageNumber, setMainPageNumber] = useState(1);

    const [searchTitle, setSearchTitle] = useState("");
    const [searchPage, setSearchPage] = useState<IPage<IThread>>({} as IPage<IThread>);
    const [searchPageNumber, setSearchPageNumber] = useState(0);

    const [isMainPageLoading, setMainPageLoading] = useState(true);
    const [isSearchPageLoading, setSearchPageLoading] = useState(false);

    const [error, setError] = useState(false);

    const { account } = useAuth();

    // TODO: deal with this gross shit

    useEffect(() => {
        if (mainPageNumber === 0)
            return;

        setMainPageLoading(true);

        getThreadsByPage(mainPageNumber)
            .then(res => setMainPage(p => ({
                items: p.items ? p.items.concat(res.data.items) : res.data.items,
                isLast: res.data.isLast
            })))
            .catch(() => setError(true))
            .finally(() => setMainPageLoading(false));
    }, [mainPageNumber]);

    useEffect(() => {
        if (searchPageNumber === 0)
            return;

        setSearchPageLoading(true);

        searchByTitle(searchTitle, searchPageNumber)
            .then(res => setSearchPage(p => ({
                items: p.items ? p.items.concat(res.data.items) : res.data.items,
                isLast: res.data.isLast
            })))
            .catch(() => setError(true))
            .finally(() => setSearchPageLoading(false));
    }, [searchTitle, searchPageNumber]);
    
    if (error)
        return <h1 className="error title">Fetch Failed</h1>;

    function loadMoreMain() {
        setMainPageNumber(mainPageNumber + 1);
    }

    function loadMoreSearch() {
        setSearchPageNumber(searchPageNumber + 1);
    }

    function search(event: RMouseEvent<SVGElement, MouseEvent>) {
        const title = new FormData(event.currentTarget.closest("form")!).get("title") as string;
        if (title.length === 0) {
            setMainPageLoading(true);

            setSearchPage({} as IPage<IThread>)
            setSearchPageNumber(0);

            setMainPageNumber(1);
        } else {
            setSearchPageLoading(true);

            setMainPage({} as IPage<IThread>);
            setMainPageNumber(0);

            setSearchPageNumber(1);
        }

        setSearchTitle(title);
    }
    
    return (
        <section className="main threads">
            <form className="search">
                <h2 className="title">Search</h2>

                <div className="center row">
                    <input type="text" name="title" placeholder="Title" />
                    <Search className="clickable icon" onClick={search} />
                </div>
            </form>

            {isSearchPageLoading && <Loader />}

            <section>
                {searchPageNumber === 0 &&
                    <div>
                        <h1 className="title">Recent threads</h1>

                        {account?.emailConfirmed &&
                            <h3 className="description">
                                <Link to="/thread/create">Create new thread</Link>
                            </h3>
                        }
                    </div>
                }

                {((isMainPageLoading && mainPage.isLast === undefined) || (isSearchPageLoading && searchPage.isLast === undefined)) ?
                    <Loader /> :
                    <ul className="content column thread-list">
                        {(searchPageNumber === 0 ? mainPage : searchPage).items.map(thread => (
                            <li key={thread.id}>
                                <div className="info-bar row">
                                    <h2 className="title">
                                        <Link to={`/thread/${thread.id}`}>{thread.title}</Link>
                                    </h2>

                                    <ul className="center row">
                                        <li className="row">
                                            <h3 className="title">{thread.views}</h3>
                                            <Views className="icon" />
                                        </li>
                                        <li className="row">
                                            <h3 className="title">{thread.commentCount}</h3>
                                            <Comments className="icon" />
                                        </li>
                                    </ul>
                                </div>

                                <div className="info-bar row">
                                    <h3 className="description">
                                        <Link to={`/user/${thread.user.id}`}>
                                            {thread.user.banned ? <s>{thread.user.userName}</s> : thread.user.userName}
                                        </Link>
                                    </h3>

                                    <h3 className="description">{dayjs(thread.createdDate).calendar()}</h3>
                                </div>
                            </li>
                        ))}
                    </ul>
                }
            </section>

            {mainPage.isLast === false && <button type="button" onClick={loadMoreMain}>Load more</button>}
            {searchPage.isLast === false && <button type="button" onClick={loadMoreSearch}>Load more</button>}
        </section>
    );
}