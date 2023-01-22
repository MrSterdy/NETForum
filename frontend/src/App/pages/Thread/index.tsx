import React, { MouseEvent as RMouseEvent, useEffect, useState, Fragment } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import dayjs from "dayjs";

import { IPage, IThread, IComment } from "../../api/models";

import { Error, Loader } from "../../components";

import { updateThreadById, deleteThread, getThreadById } from "../../api/endpoints/threads";
import { getCommentsByPage, createComment, deleteCommentById, updateCommentById } from "../../api/endpoints/comments";

import { useFetch, useAuth } from "../../hooks";

import { ReactComponent as Delete } from "../../assets/icons/trash.svg";
import { ReactComponent as Edit } from "../../assets/icons/pencil.svg";
import { ReactComponent as Confirm } from "../../assets/icons/check.svg";
import { ReactComponent as Cancel } from "../../assets/icons/cross.svg";
import { ReactComponent as Comment } from "../../assets/icons/comment.svg";

import "./index.css";

export default function Thread() {
    const { account } = useAuth();

    const [isCommenting, setCommenting] = useState(false);
    const [editingComment, setEditingComment] = useState<number>();
    const [readyToDeleteComment, setReadyToDeleteComment] = useState<number>();

    const [isEditingThread, setEditingThread] = useState(false);
    const [isReadyToDeleteThread, setReadyToDeleteThread] = useState(false);

    const threadId = parseInt(useParams().id!);

    const {
        data: thread,
        isLoading: isThreadLoading,
        error: threadError
    } = useFetch<IThread>(getThreadById, threadId);

    const [comments, setComments] = useState<IPage<IComment>>({} as IPage<IComment>);
    const [commentPage, setCommentPage] = useState(1);

    const navigate = useNavigate();

    useEffect(() => {
        getCommentsByPage(commentPage, threadId)
            .then(res => setComments(comments => ({
                items: comments.items ? comments.items.concat(res.data.items) : res.data.items,
                isLast: res.data.isLast
            })));
    }, [commentPage, threadId]);

    if (threadError)
        return <Error message="Thread not found" />;

    if (isThreadLoading)
        return <Loader />;

    function deleteThreadHandler() {
        setReadyToDeleteThread(!isReadyToDeleteThread);
    }

    async function confirmDeleteThreadHandler() {
        await deleteThread(threadId);

        navigate("/");
    }

    function deleteCommentHandler(id?: number) {
        setReadyToDeleteComment(id);
    }

    async function confirmDeleteCommentHandler() {
        await deleteCommentById(readyToDeleteComment as number);

        window.location.reload();
    }

    function editThreadHandler() {
        setEditingThread(!isEditingThread);
    }

    function confirmEditThreadHandler(event: RMouseEvent<SVGSVGElement, MouseEvent>) {
        const data = new FormData(event.currentTarget.closest("form")!);

        updateThreadById(threadId, data.get("title") as string, data.get("content") as string)
            .finally(() => window.location.reload());
    }

    function editCommentHandler(id?: number) {
        setEditingComment(id);
    }

    function confirmEditCommentHandler(event: RMouseEvent<SVGSVGElement, MouseEvent>) {
        const data = new FormData(event.currentTarget.closest("ul")!.previousElementSibling as HTMLFormElement);

        updateCommentById(editingComment!, data.get("content") as string)
            .finally(() => window.location.reload());
    }

    function commentHandler() {
        setCommenting(!isCommenting);
    }

    function confirmCommentHandler(event: RMouseEvent<SVGSVGElement, MouseEvent>) {
        const data = new FormData(event.currentTarget.closest("ul")!.previousElementSibling as HTMLFormElement);

        createComment(threadId, data.get("content") as string)
            .finally(() => window.location.reload());
    }

    function loadMoreCommentsHandler() {
        setCommentPage(commentPage + 1);
    }

    if (isEditingThread)
        return (
            <form className="thread-create main">
                <div>
                    <h2 className="title">Title:</h2>

                    <input className="full-width" type="text" name="title" minLength={4} maxLength={127} defaultValue={thread.title} required />
                </div>

                <div>
                    <h2 className="title">Content:</h2>

                    <article className="content">
                        <textarea className="full-width" name="content" minLength={4} maxLength={32767} defaultValue={thread.content} required></textarea>

                        <ul className="row option-bar">
                            <li>
                                <Confirm className="clickable icon" onClick={confirmEditThreadHandler} />
                            </li>
                            <li>
                                <Cancel className="clickable icon" onClick={editThreadHandler} />
                            </li>
                        </ul>
                    </article>
                </div>

            </form>
        );

    return (
        <section className="thread main">
            <h1 className="title">{thread.title}</h1>

            <section className="content">
                <div className="info-bar row">
                    <h4 className="description">
                        <Link to={`/user/${thread.user.id}`}>{thread.user.userName}</Link>
                    </h4>

                    <h4 className="description calendar">{dayjs(thread.createdDate).calendar()}</h4>
                </div>

                <article>{thread.content}</article>

                {account?.confirmed && !isCommenting &&
                    <ul className="row option-bar">
                        <li>
                            <Comment className="clickable icon" onClick={commentHandler} />
                        </li>
                        {account?.id === thread.user.id &&
                            <>
                                {isReadyToDeleteThread &&
                                    <>
                                        <span>Are you sure?</span>

                                        <li>
                                            <Confirm className="clickable icon" onClick={confirmDeleteThreadHandler} />
                                        </li>
                                        <li>
                                            <Cancel className="clickable icon" onClick={deleteThreadHandler} />
                                        </li>
                                    </>
                                }
                                {!isReadyToDeleteThread &&
                                    <>
                                        <li>
                                            <Edit className="clickable icon" onClick={editThreadHandler} />
                                        </li>
                                        <li className="center row">
                                            <Delete className="clickable icon" onClick={deleteThreadHandler} />
                                        </li>
                                    </>
                                }
                            </>
                        }
                    </ul>
                }
            </section>

            {(!!comments.items?.length || isCommenting) &&
                <section className="column comments">
                    <h2 className="title">Comments</h2>

                    <ul className="column">
                        {isCommenting &&
                            <li className="column content">
                                <form className="comment-create">
                                    <textarea name="full-width content" minLength={4} maxLength={32767} required></textarea>
                                </form>

                                <ul className="row option-bar">
                                    <li>
                                        <Confirm className="clickable icon" onClick={confirmCommentHandler} />
                                    </li>
                                    <li>
                                        <Cancel className="clickable icon" onClick={commentHandler} />
                                    </li>
                                </ul>
                            </li>
                        }

                        {comments.items?.map(c =>
                            <Fragment key={c.id}>
                                {editingComment === c.id &&
                                    <li className="column content">
                                        <form className="comment-create">
                                            <textarea name="full-width content" minLength={4} maxLength={32767} defaultValue={c.content} required></textarea>
                                        </form>

                                        <ul className="row option-bar">
                                            <li>
                                                <Confirm className="clickable icon" onClick={confirmEditCommentHandler} />
                                            </li>
                                            <li>
                                                <Cancel className="clickable icon" onClick={() => editCommentHandler()} />
                                            </li>
                                        </ul>
                                    </li>
                                }

                                {editingComment !== c.id &&
                                    <li className="column">
                                        <article className="content">
                                            <div className="info-bar row">
                                                <h4 className="description">
                                                    <Link to={`/user/${c.user.id}`}>{c.user.userName}</Link>
                                                </h4>

                                                <h4 className="description calendar">{dayjs(c.createdDate).calendar()}</h4>
                                            </div>

                                            {c.content}

                                            {c.user.id === account?.id &&
                                                (
                                                    (readyToDeleteComment === c.id || readyToDeleteComment === undefined)
                                                    && editingComment === undefined
                                                ) &&
                                                <ul className="row option-bar">
                                                    {readyToDeleteComment === c.id &&
                                                        <>
                                                            <span>Are you sure?</span>

                                                            <li>
                                                                <Confirm className="clickable icon" onClick={confirmDeleteCommentHandler} />
                                                            </li>
                                                            <li>
                                                                <Cancel className="clickable icon" onClick={() => deleteCommentHandler()} />
                                                            </li>
                                                        </>
                                                    }
                                                    {readyToDeleteComment === undefined &&
                                                        <>
                                                            <li>
                                                                <Edit className="clickable icon" onClick={() => editCommentHandler(c.id)} />
                                                            </li>
                                                            <li>
                                                                <Delete className="clickable icon" onClick={() => deleteCommentHandler(c.id)} />
                                                            </li>
                                                        </>
                                                    }
                                                </ul>
                                            }
                                        </article>
                                    </li>
                                }
                            </Fragment>
                        )}
                    </ul>

                    {!comments.isLast && <button onClick={loadMoreCommentsHandler}>Load more</button>}
                </section>
            }
        </section>
    );
}