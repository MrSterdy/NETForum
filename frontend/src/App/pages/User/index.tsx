import { Link, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import dayjs from "dayjs";

import { IPage, IThread, IUser } from "../../api/models";

import { Loader, Error } from "../../components";

import { getUserById } from "../../api/endpoints/users";
import { getThreadsByUserId } from "../../api/endpoints/threads";

import { useAuth, useFetch } from "../../hooks";

import ProfilePic from "../../assets/icons/profile-pic.png";

import "./index.css";

export default function User() {
    const { data: user, isLoading: userLoading, error: userError }
        = useFetch<IUser>(getUserById, parseInt(useParams().id!));

    const { account } = useAuth();

    const [page, setPage] = useState<IPage<IThread>>({} as IPage<IThread>);
    const [pageNumber, setPageNumber] = useState(1);
    const [threadsLoading, setThreadsLoading] = useState(true);
    const [threadsError, setThreadsError] = useState("");

    useEffect(() => {
        if (user.id === undefined)
            return;

        getThreadsByUserId(user.id, pageNumber)
            .then(res => setPage(p => ({
                items: p.items ? p.items.concat(res.data.items) : res.data.items,
                isLast: res.data.isLast
            })))
            .catch(err => setThreadsError((err as Error).message))
            .finally(() => setThreadsLoading(false));
    }, [pageNumber, user.id]);

    if (userLoading)
        return <Loader />;

    if (userError)
        return <Error message="User not found" />;

    const loadMore = () => setPageNumber(pageNumber + 1);

    return (
        <section className="user-profile main">
            <section className="user-header content row">
                <div className="center column">
                    <img src={ProfilePic} className="user-avatar" alt=""/>

                    <h2 className="title">{user.userName}</h2>

                    {user.id === account?.id &&
                        <h3 className="description">
                            <Link to="/account">Edit Account</Link>
                        </h3>
                    }
                </div>
            </section>

            {!threadsLoading &&
                <section className="column">
                    <h2 className="title">Recent user's threads</h2>

                    <ul className="content column">
                        {page.items.map(thread => (
                            <li key={ thread.id }>
                                <h2 className="title">
                                    <Link to={`/thread/${thread.id}`}>{ thread.title }</Link>
                                </h2>

                                <div className="info-bar row">
                                    <h3 className="description">
                                        <Link to={ `/user/${thread.user.id}` }>{ thread.user.userName }</Link>
                                    </h3>

                                    <h3 className="description">{dayjs(thread.createdDate).calendar()}</h3>
                                </div>
                            </li>
                        ))}
                    </ul>

                    {!page.isLast && <button type="button" onClick={ loadMore } className="centered">Load more</button>}
                </section>
            }

            {threadsError && <Error message="Fetch failed" />}
        </section>
    );
}