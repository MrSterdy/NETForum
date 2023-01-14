import {Link, useNavigate, useParams} from "react-router-dom";

import { IThread } from "../../api/models";
import { Error, Loader } from "../../components";
import {deleteThread, getThreadById} from "../../api/thread";
import useFetch from "../../hooks/useFetch";
import useAuth from "../../hooks/useAuth";

import { ReactComponent as Trash } from "../../assets/icons/trash.svg";
import "./index.css";
import {useState} from "react";

export default function Thread() {
    const [isReady, setReady] = useState(false);

    const navigate = useNavigate();

    const { user } = useAuth();

    const { data, isLoading, error } = useFetch<IThread>(getThreadById, parseInt(useParams().id!));

    if (isLoading)
        return <Loader />;

    if (error)
        return <Error message="Thread not found" />;

    async function onClickDeleteHandler() {
        if (!isReady)
            return setReady(true);

        await deleteThread(data.id!);

        navigate("/");
    }

    return (
        <section className="main">
            <div>
                <h1 className="title">{ data.title }</h1>

                <h3 className="description">
                    <Link to={ `user/${data.user.id}` }>{ data.user.userName }</Link>
                </h3>
            </div>

            <div className="content">
                <article>{data.content}</article>

                { user?.id === data.user.id &&
                    <ul className="row option-bar">
                        <li className="center row">
                            { isReady && "Are you sure?" }
                            <Trash className="clickable icon" onClick={ onClickDeleteHandler } />
                        </li>
                    </ul>
                }
            </div>
        </section>
    );
}