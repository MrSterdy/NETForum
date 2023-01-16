import { MouseEvent as RMouseEvent, useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";

import { IPage, IThread, IComment } from "../../api/models";

import { Error, Loader } from "../../components";

import { updateThreadById, deleteThread, getThreadById } from "../../api/endpoints/threads";
import { getCommentsByPage, createComment, deleteCommentById } from "../../api/endpoints/comments";

import { useFetch, useAuth } from "../../hooks";

import { ReactComponent as Delete } from "../../assets/icons/trash.svg";
import { ReactComponent as Edit } from "../../assets/icons/pencil.svg";
import { ReactComponent as Confirm } from "../../assets/icons/check.svg";
import { ReactComponent as Cancel } from "../../assets/icons/cross.svg";
import { ReactComponent as Comment } from "../../assets/icons/comment.svg";

import "./index.css";

export default function Thread() {
    const { user } = useAuth();

    const [isCommenting, setCommenting] = useState(false);
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
            .then(res => setComments({
                items: comments.items ? comments.items.concat(res.data.items) : res.data.items,
                isLast: res.data.isLast
            }));
    }, [commentPage]);

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

    function commentHandler() {
        setCommenting(!isCommenting);
    }

    function confirmThreadHandler(event: RMouseEvent<SVGSVGElement, MouseEvent>) {
        const data = new FormData(event.currentTarget.closest("form")!);

        updateThreadById(threadId, {
            title: data.get("title") as string,
            content: data.get("content") as string
        })
            .finally(() => window.location.reload());
    }

    function confirmCommentHandler(event: RMouseEvent<SVGSVGElement, MouseEvent>) {
        const data = new FormData(event.currentTarget.closest("ul")!.previousElementSibling as HTMLFormElement);

        createComment({
            threadId: threadId,
            content: data.get("content") as string
        })
            .finally(() => window.location.reload());
    }

    function loadMoreCommentsHandler() {
        setCommentPage(commentPage + 1);
    }

    if (isEditingThread)
        return (
            <form className="thread-create main">
                <input type="text" name="title" minLength={4} maxLength={127} defaultValue={ thread.title } required />

                <article className="content">
                    <textarea name="content" minLength={4} maxLength={32767} defaultValue={ thread.content } required></textarea>

                    <ul className="row option-bar">
                        <li>
                            <Confirm className="clickable icon" onClick={ confirmThreadHandler } />
                        </li>
                        <li>
                            <Cancel className="clickable icon" onClick={ editThreadHandler } />
                        </li>
                    </ul>
                </article>
            </form>
        );

    return (
        <section className="main">
            <div>
                <h1 className="title">{thread.title}</h1>

                <h3 className="description">
                    <Link to={`user/${thread.user.id}`}>{thread.user.userName}</Link>
                </h3>
            </div>

            <article className="content">
                <article>{thread.content}</article>

                {user?.confirmed && !isCommenting &&
                    <ul className="row option-bar">
                        <li>
                            <Comment className="clickable icon" onClick={commentHandler} />
                        </li>
                        {user?.id === thread.user.id &&
                            <>
                                {isReadyToDeleteThread &&
                                    <>
                                        <span>Are you sure?</span>

                                        <li className="center row">
                                            <Confirm className="clickable icon" onClick={confirmDeleteThreadHandler} />
                                        </li>
                                        <li className="center row">
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
            </article>

            {(!!comments.items?.length || isCommenting) &&
                <section className="column comments">
                    <h2 className="title">Comments</h2>

                    <ul className="column">
                        {isCommenting &&
                            <li className="column content">
                                <form className="comment-create">
                                    <textarea name="content" minLength={4} maxLength={32767} required></textarea>
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
                            <li className="column" key={c.id}>
                                <h3 className="description">
                                    <Link to={`/user/${c.user.id}`}>{c.user.userName}</Link>
                                </h3>

                                <article className="content">
                                    {c.content}

                                    {c.user.id === user?.id && (readyToDeleteComment === c.id || readyToDeleteComment === undefined) &&
                                        <ul className="row option-bar">
                                            {readyToDeleteComment === c.id &&
                                                <>
                                                    <span>Are you sure?</span>

                                                    <li className="center row">
                                                        <Confirm className="clickable icon" onClick={confirmDeleteCommentHandler} />
                                                    </li>
                                                    <li className="center row">
                                                        <Cancel className="clickable icon" onClick={() => deleteCommentHandler()} />
                                                    </li>
                                                </>
                                            }

                                            {readyToDeleteComment === undefined &&
                                                <li className="center row">
                                                    <Delete className="clickable icon" onClick={() => deleteCommentHandler(c.id)} />
                                                </li>
                                            }
                                        </ul>
                                    }
                                </article>
                            </li>
                        )}
                    </ul>

                    {!comments.isLast && <button onClick={loadMoreCommentsHandler}>Load more</button>}
                </section>
            }
        </section>
    );
}