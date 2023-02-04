import React, { MouseEvent as RMouseEvent, useEffect, useState } from "react";
import { Tag } from 'react-tag-input';
import { Link } from "react-router-dom";
import dayjs from "dayjs";

import { Loader, TagInput } from "../../components";

import { IPage, IThread, IUser } from "../../api/models";

import { getThreads } from "../../api/endpoints/threads";
import { searchUsers } from "../../api/endpoints/users";

import { useAuth } from "../../hooks";

import { ReactComponent as Views } from "../../assets/icons/eye.svg";
import { ReactComponent as Comments } from "../../assets/icons/comment.svg";
import { ReactComponent as Search } from "../../assets/icons/search.svg";
import { ReactComponent as Settings } from "../../assets/icons/settings.svg";

export default function Home() {
    const [mainPage, setMainPage] = useState<IPage<IThread>>();
    const [mainPageNumber, setMainPageNumber] = useState(1);

    const [searchTitle, setSearchTitle] = useState("");

    const [showSettings, setShowSettings] = useState(false);
    const [searchUser, setSearchUser] = useState<IUser>();
    const [searchTags, setSearchTags] = useState([] as Tag[]);

    const [users, setUsers] = useState<IPage<IUser>>();

    const [searchPage, setSearchPage] = useState<IPage<IThread>>();
    const [searchPageNumber, setSearchPageNumber] = useState(0);

    const [isMainPageLoading, setMainPageLoading] = useState(true);
    const [isSearchPageLoading, setSearchPageLoading] = useState(false);

    const [error, setError] = useState(false);

    const { account } = useAuth();

    useEffect(() => {
        if (mainPageNumber === 0)
            return;

        getThreads(mainPageNumber)
            .then(res => setMainPage(p => ({
                items: p?.items.concat(res.data.items) ?? res.data.items,
                isLast: res.data.isLast
            })))
            .catch(() => setError(true))
            .finally(() => setMainPageLoading(false));
    }, [mainPageNumber]);

    useEffect(() => {
        if (searchPageNumber === 0)
            return;

        getThreads(
            searchPageNumber,
            searchUser?.id,
            searchTitle,
            searchTags.map(t => parseInt(t.id))
        )
            .then(res => setSearchPage(p => ({
                items: p?.items.concat(res.data.items) ?? res.data.items,
                isLast: res.data.isLast
            })))
            .catch(() => setError(true))
            .finally(() => setSearchPageLoading(false));
    }, [searchTitle, searchPageNumber, searchTags, searchUser?.id]);
    
    if (error)
        return <h1 className="error title">Fetch Failed</h1>;

    function loadMoreMain() {
        setMainPageLoading(true);
        setMainPageNumber(p => p + 1);
    }

    function loadMoreSearch() {
        setSearchPageLoading(true);
        setSearchPageNumber(p => p + 1);
    }

    function showSearchSettings() {
        setShowSettings(s => !s);
    }

    function search(event: RMouseEvent<SVGElement, MouseEvent>) {
        const title = new FormData(event.currentTarget.closest("form")!).get("title") as string;

        if (title.length === 0 && searchTags.length === 0 && searchUser === undefined) {
            setMainPageLoading(true);

            setSearchPage(undefined);
            setSearchPageNumber(0);

            setMainPageNumber(1);
        } else {
            setSearchPageLoading(true);

            setMainPage(undefined);
            setMainPageNumber(0);

            setSearchPageNumber(1);
        }

        setSearchTitle(title);
    }

    function fetchUsers(event: React.ChangeEvent<HTMLInputElement>) {
        setSearchUser(undefined);

        const username = event.currentTarget.value;

        if (username.length < 2)
            return setUsers({ items: [], isLast: true });

        searchUsers(1, username)
            .then(res => {
                if (res.data.items.length === 1) {
                    const user = res.data.items[0];

                    if (user.userName === username) {
                        res.data.items = [];

                        setSearchUser(user);
                    }
                }

                setUsers(res.data)
            });
    }

    function setUser(event: React.MouseEvent<HTMLButtonElement>) {
        const username = event.currentTarget.innerText;

        const user = users!.items.find(u => u.userName === username)!;

        const input = event.currentTarget.closest("ul")!.previousElementSibling as HTMLInputElement;
        input.value = user.userName;

        setSearchUser(user);
        setUsers({ items: [], isLast: true });
    }
    
    return (
        <section className="main threads">
            <form className="search column">
                <h2 className="title">Search</h2>

                <div className="center row">
                    <input type="text" name="title" placeholder="Title" />

                    <Settings className="clickable icon" onClick={showSearchSettings} />
                    <Search className="clickable icon" onClick={search} />
                </div>

                <div className="column settings content" style={{display: showSettings ? "flex" : "none"}}>
                    <div className="center row">
                        <span>User:</span>

                        <div className="full-width column">
                            <input type="text" name="username" placeholder="Enter username" onChange={fetchUsers} />

                            <ul className="user-suggestions row">
                                {users?.items.map(u =>
                                    <li key={u.id}>
                                        <button className="small" type="button" onClick={setUser}>{u.userName}</button>
                                    </li>
                                )}
                            </ul>
                        </div>
                    </div>

                    <div className="center row">
                        <span>Tags:</span>

                        <TagInput tags={searchTags} setTags={setSearchTags} />
                    </div>

                    {account?.admin &&
                        <h3 className="description">
                            <Link to="/tag/create">Create new tag</Link>
                        </h3>
                    }
                </div>
            </form>

            <section className="column">
                {mainPage &&
                    <div>
                        <h1 className="title">Recent threads</h1>

                        {account?.emailConfirmed &&
                            <h3 className="description">
                                <Link to="/thread/create">Create new thread</Link>
                            </h3>
                        }
                    </div>
                }

                {((isMainPageLoading && !mainPage) || (isSearchPageLoading && !searchPage)) ?
                    <Loader /> :
                    <ul className="content column thread-list">
                        {(mainPage ?? searchPage!).items.map(thread => (
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

            {mainPage?.isLast === false &&
                (isMainPageLoading ? <Loader /> : <button type="button" onClick={loadMoreMain}>Load more</button>)
            }

            {searchPage?.isLast === false &&
                (isSearchPageLoading ? <Loader /> : <button type="button" onClick={loadMoreSearch}>Load more</button>)
            }
        </section>
    );
}