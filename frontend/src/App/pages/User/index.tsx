import { Link, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import dayjs from "dayjs";

import { IPage, IThread, IUser } from "../../api/models";

import { Loader } from "../../components";

import { banById, getUserById } from "../../api/endpoints/users";
import { getThreads } from "../../api/endpoints/threads";

import { useAuth, useFetch } from "../../hooks";

import ProfilePic from "../../assets/icons/profile-pic.png";
import { ReactComponent as Lock } from "../../assets/icons/lock.svg";
import { ReactComponent as Unlock } from "../../assets/icons/unlock.svg";

import "./index.css";

export default function User() {
    const { data: user, isLoading: userLoading, error: userError }
        = useFetch<IUser>(getUserById, parseInt(useParams().id!));

    const { account } = useAuth();

    const [page, setPage] = useState<IPage<IThread>>({} as IPage<IThread>);
    const [pageNumber, setPageNumber] = useState(1);
    const [threadsLoading, setThreadsLoading] = useState(true);
    const [threadsError, setThreadsError] = useState(false);

    const [isBanning, setBanning] = useState(false);

    useEffect(() => {
        if (user.id === undefined)
            return;

        getThreads(pageNumber, user.id)
            .then(res => setPage(p => ({
                items: p.items ? p.items.concat(res.data.items) : res.data.items,
                isLast: res.data.isLast
            })))
            .catch(() => setThreadsError(true))
            .finally(() => setThreadsLoading(false));
    }, [pageNumber, user.id]);

    if (userLoading || isBanning)
        return <Loader />;

    if (!!userError)
        return <h1 className="centered error title">User Not Found</h1>;

    function ban() {
        setBanning(true);

        banById(user.id!)
            .then(() => window.location.reload())
            .finally(() => setBanning(false));
    }

    const loadMore = () => setPageNumber(pageNumber + 1);

    return (
        <section className="user-profile main">
            <section className="content column">
                <section className="user-header row">
                    <div className="center column">
                        <img src={ProfilePic} className="user-avatar" alt=""/>

                        <div>
                            <h2 className="title">{user.userName}</h2>

                            {user.banned && <h3 className="description">Banned</h3>}
                        </div>

                        {user.id === account?.id &&
                            <h3 className="description">
                                <Link to="/account">Edit Account</Link>
                            </h3>
                        }
                    </div>
                </section>

                {account?.admin && account.id !== user.id &&
                    <section className="full-width option-bar row">
                        {user.banned ?
                            <Unlock className="clickable icon" onClick={ban} /> :
                            <Lock className="clickable icon" onClick={ban} />
                        }
                    </section>
                }
            </section>

            {!threadsLoading && !threadsError && !!page.items.length &&
                <section className="column">
                    <h2 className="title">Recent user's threads</h2>

                    <ul className="content column">
                        {page.items.map(thread => (
                            <li key={ thread.id }>
                                <h2 className="title">
                                    <Link to={`/thread/${thread.id}`}>{thread.title}</Link>
                                </h2>

                                <div className="info-bar row">
                                    <h3 className="description">
                                        <Link to={`/user/${thread.user.id}`}>{thread.user.userName}</Link>
                                    </h3>

                                    <h3 className="description">{dayjs(thread.createdDate).calendar()}</h3>
                                </div>
                            </li>
                        ))}
                    </ul>

                    {!page.isLast && <button type="button" onClick={loadMore} className="centered">Load more</button>}
                </section>
            }

            {threadsError && <h2 className="centered error title">Couldn't fetch user's threads</h2>}
        </section>
    );
}