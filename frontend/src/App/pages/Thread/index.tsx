import { MouseEvent as RMouseEvent, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";

import { IThread } from "../../api/models";
import { Error, Loader } from "../../components";
import { updateThreadById, deleteThread, getThreadById } from "../../api/thread";
import useFetch from "../../hooks/useFetch";
import useAuth from "../../hooks/useAuth";

import { ReactComponent as Delete } from "../../assets/icons/trash.svg";
import { ReactComponent as Edit } from "../../assets/icons/pencil.svg";
import { ReactComponent as Confirm } from "../../assets/icons/check.svg";
import { ReactComponent as Cancel } from "../../assets/icons/cross.svg";
import { ReactComponent as Comment } from "../../assets/icons/comment.svg";
import "./index.css";
import createComment from "../../api/comment";

export default function Thread() {
    const [isSending, setSending] = useState(false);
    const [isCommenting, setCommenting] = useState(false);
    const [isEditing, setEditing] = useState(false);
    const [isReady, setReady] = useState(false);

    const navigate = useNavigate();

    const { user } = useAuth();

    const id = parseInt(useParams().id!);

    const { data, isLoading, error } = useFetch<IThread>(getThreadById, id);

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

    function onClickEditHandler() {
        setEditing(!isEditing);
    }

    function onClickCommentHandler() {
        setCommenting(!isCommenting);
    }

    function onClickConfirmHandler(event: RMouseEvent<SVGSVGElement, MouseEvent>) {
        setSending(true);

        const data = new FormData(event.currentTarget.closest("form")!);

        updateThreadById(id, {
            title: data.get("title") as string,
            content: data.get("content") as string
        })
            .finally(() => window.location.reload());
    }

    function onClickCommentConfirmHandler(event: RMouseEvent<SVGSVGElement, MouseEvent>) {
        const data = new FormData(event.currentTarget.closest("ul")!.previousElementSibling as HTMLFormElement);

        createComment({
            threadId: id,
            content: data.get("content") as string
        })
            .finally(() => window.location.reload());
    }

    if (isSending)
        return (
            <section className="main">
                <div className="content">
                    <Loader />
                </div>
            </section>
        );

    if (isEditing)
        return (
            <form className="thread-create main">
                <input type="text" name="title" minLength={4} maxLength={127} defaultValue={ data.title } required />

                <article className="content">
                    <textarea name="content" minLength={4} maxLength={32767} defaultValue={ data.content } required></textarea>

                    <ul className="row option-bar">
                        <li>
                            <Confirm className="clickable icon" onClick={ onClickConfirmHandler } />
                        </li>
                        <li>
                            <Cancel className="clickable icon" onClick={ onClickEditHandler } />
                        </li>
                    </ul>
                </article>
            </form>
        );

    return (
        <section className="main">
            <div>
                <h1 className="title">{ data.title }</h1>

                <h3 className="description">
                    <Link to={ `user/${data.user.id}` }>{ data.user.userName }</Link>
                </h3>
            </div>

            <article className="content">
                <article>{data.content}</article>

                { user?.confirmed && !isCommenting &&
                    <ul className="row option-bar">
                        <li>
                            <Comment className="clickable icon" onClick={ onClickCommentHandler } />
                        </li>
                        { user?.id === data.user.id &&
                            <>
                                <li>
                                    <Edit className="clickable icon" onClick={ onClickEditHandler } />
                                </li>
                                <li className="center row">
                                    { isReady && "Are you sure?" }
                                    <Delete className="clickable icon" onClick={ onClickDeleteHandler } />
                                </li>
                            </>
                        }
                    </ul>
                }
            </article>

            { !!data.comments.length &&
                <section className="column comments">
                    <h2 className="title">Comments</h2>

                    <ul className="column">
                        { isCommenting &&
                            <li className="column content">
                                <form className="comment-create">
                                    <textarea name="content" minLength={4} maxLength={32767} required></textarea>
                                </form>

                                <ul className="row option-bar">
                                    <li>
                                        <Confirm className="clickable icon" onClick={ onClickCommentConfirmHandler } />
                                    </li>
                                    <li>
                                        <Cancel className="clickable icon" onClick={ onClickCommentHandler } />
                                    </li>
                                </ul>
                            </li>
                        }

                        { data.comments.map(c =>
                            <li className="column" key={c.id}>
                                <h3 className="description">
                                    <Link to={`/user/${c.user.id}`}>{ c.user.userName }</Link>
                                </h3>

                                <article className="content">
                                    { c.content }
                                </article>
                            </li>
                        ) }
                    </ul>
                </section>
            }
        </section>
    );
}