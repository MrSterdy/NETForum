import React, { MouseEvent as RMouseEvent, useEffect, useState, Fragment } from "react";
import { Tag } from "react-tag-input";
import { Link, useNavigate, useParams } from "react-router-dom";
import dayjs from "dayjs";

import { IPage, IThread, IComment } from "../../api/models";

import { Loader, TagInput } from "../../components";

import { updateThreadById, deleteThread, getThreadById } from "../../api/endpoints/threads";
import { getCommentsByPage, createComment, deleteCommentById, updateCommentById } from "../../api/endpoints/comments";

import { useAuth } from "../../hooks";

import { ReactComponent as Delete } from "../../assets/icons/trash.svg";
import { ReactComponent as Edit } from "../../assets/icons/pencil.svg";
import { ReactComponent as Confirm } from "../../assets/icons/check.svg";
import { ReactComponent as Cancel } from "../../assets/icons/cross.svg";
import { ReactComponent as Comment } from "../../assets/icons/comment.svg";

import "./index.css";

export default function Thread() {
    const { account } = useAuth();

    const [thread, setThread] = useState<IThread>();
    const [isThreadLoading, setThreadLoading] = useState(true);
    const [threadError, setThreadError] = useState(false);

    const [tags, setTags] = useState<Tag[]>([]);

    const [isSubmittingComment, setSubmittingComment] = useState(false);
    const [isLoadingComments, setLoadingComments] = useState(false);
    const [submitCommentError, setSubmitCommentError] = useState(false);
    const [isCommenting, setCommenting] = useState(false);
    const [editingComment, setEditingComment] = useState<number>();
    const [readyToDeleteComment, setReadyToDeleteComment] = useState<number>();

    const [isSubmittingThread, setSubmittingThread] = useState(false);
    const [submitThreadError, setSubmitThreadError] = useState(false);
    const [isEditingThread, setEditingThread] = useState(false);
    const [isReadyToDeleteThread, setReadyToDeleteThread] = useState(false);

    const [comments, setComments] = useState<IPage<IComment>>({} as IPage<IComment>);
    const [commentPage, setCommentPage] = useState(1);

    const navigate = useNavigate();

    const threadId = parseInt(useParams().id!);

    useEffect(() => {
        getThreadById(threadId)
            .then(res => {
                setThread(res.data);
                setTags(res.data.tags.map(t => ({ id: t.id!.toString(), text: t.name })));
            })
            .catch(() => setThreadError(true))
            .finally(() => setThreadLoading(false));
    }, [threadId]);

    useEffect(() => {
        setLoadingComments(true);

        getCommentsByPage(commentPage, threadId)
            .then(res => setComments(comments => ({
                items: comments.items ? comments.items.concat(res.data.items) : res.data.items,
                isLast: res.data.isLast
            })))
            .finally(() => setLoadingComments(false));
    }, [commentPage, threadId]);

    if (threadError)
        return <h1 className="error title">Thread Not Found</h1>;

    if (isThreadLoading || isSubmittingThread)
        return <Loader />;

    function deleteThreadHandler() {
        setReadyToDeleteThread(!isReadyToDeleteThread);
    }

    async function confirmDeleteThreadHandler() {
        setSubmittingThread(true);

        deleteThread(threadId)
            .then(() => window.location.reload())
            .catch(() => setSubmitThreadError(true))
            .finally(() => setSubmittingThread(false));

        navigate("/");
    }

    function deleteCommentHandler(id?: number) {
        setReadyToDeleteComment(id);
    }

    async function confirmDeleteCommentHandler() {
        setSubmittingComment(true);

        deleteCommentById(readyToDeleteComment as number)
            .then(() => window.location.reload())
            .catch(() => setSubmitCommentError(true))
            .finally(() => setSubmittingComment(false));
    }

    function editThreadHandler() {
        setSubmitThreadError(false);

        setEditingThread(!isEditingThread);
    }

    function confirmEditThreadHandler(event: RMouseEvent<SVGSVGElement, MouseEvent>) {
        setSubmittingThread(true);

        const data = new FormData(event.currentTarget.closest("form")!);

        updateThreadById(
            threadId,
            data.get("title") as string,
            data.get("content") as string,
            tags.map(t => parseInt(t.id))
        )
            .then(() => window.location.reload())
            .catch(() => setSubmitThreadError(true))
            .finally(() => setSubmittingThread(false));
    }

    function editCommentHandler(id?: number) {
        setSubmitCommentError(false);

        setEditingComment(id);
    }

    function confirmEditCommentHandler(event: RMouseEvent<SVGSVGElement, MouseEvent>) {
        setSubmittingComment(true);

        const data = new FormData(event.currentTarget.closest("form")!);

        updateCommentById(editingComment!, data.get("content") as string)
            .then(() => window.location.reload())
            .catch(() => setSubmitCommentError(true))
            .finally(() => setSubmittingComment(false));
    }

    function commentHandler() {
        setSubmitCommentError(false);

        setCommenting(!isCommenting);
    }

    function confirmCommentHandler(event: RMouseEvent<SVGSVGElement, MouseEvent>) {
        setSubmittingComment(true);

        const data = new FormData(event.currentTarget.closest("form")!);

        createComment(threadId, data.get("content") as string)
            .then(() => window.location.reload())
            .catch(() => setSubmitCommentError(true))
            .finally(() => setSubmittingComment(false));
    }

    function loadMoreCommentsHandler() {
        setCommentPage(commentPage + 1);
    }

    if (isEditingThread)
        return (
            <form className="thread-create main">
                <div>
                    <h2 className="title">Title:</h2>

                    <input className="full-width" type="text" name="title" minLength={4} maxLength={127} defaultValue={thread!.title} required />
                </div>

                <div>
                    <h2 className="title">Content:</h2>

                    <article className="content column">
                        <textarea className="full-width" name="content" minLength={4} maxLength={32767} defaultValue={thread!.content} required></textarea>

                        {submitThreadError && <span className="centered error">An error occurred. Please try again later</span>}

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

                <div className="center row">
                    <h3 className="title">Tags:</h3>

                    <TagInput tags={tags} setTags={setTags} />
                </div>
            </form>
        );

    return (
        <section className="thread main">
            <h1 className="title">{thread!.title}</h1>

            <section className="column content">
                <div className="info-bar row">
                    <h4 className="description">
                        <Link to={`/user/${thread!.user.id}`}>
                            {thread!.user.banned ? <s>{thread!.user.userName}</s> : thread!.user.userName}
                        </Link>
                    </h4>

                    <h4 className="description calendar">{dayjs(thread!.createdDate).calendar()}</h4>
                </div>

                <article>{thread!.content}</article>

                {thread!.tags.length !== 0 &&
                    <div className="row">
                        <h4 className="center title">Tags:</h4>

                        <ul className="small row">
                            {thread!.tags.map(t =>
                                <li className="tag" key={t.id}>{t.name}</li>
                            )}
                        </ul>
                    </div>
                }

                {account?.emailConfirmed && !isCommenting &&
                    <ul className="row option-bar">
                        {(account?.admin || account?.id === thread!.user.id) &&
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
                                        {account.id === thread!.user.id &&
                                            <li>
                                                <Edit className="clickable icon" onClick={editThreadHandler} />
                                            </li>
                                        }
                                        <li className="center row">
                                            <Delete className="clickable icon" onClick={deleteThreadHandler} />
                                        </li>
                                    </>
                                }
                            </>
                        }

                        <li>
                            <Comment className="clickable icon" onClick={commentHandler} />
                        </li>
                    </ul>
                }
            </section>

            {isSubmittingComment && <Loader />}

            {!isSubmittingComment && (!!comments.items?.length || isCommenting) &&
                <section className="column comments">
                    <h2 className="title">Comments</h2>

                    <ul className="column">
                        {isCommenting &&
                            <li>
                                <form className="comment-create column content">
                                    <textarea className="full-width" name="content" minLength={4} maxLength={32767} required></textarea>

                                    {submitCommentError && <span className="centered error">An error occurred. Please try again later</span>}

                                    <ul className="row option-bar">
                                        <li>
                                            <Confirm className="clickable icon" onClick={confirmCommentHandler} />
                                        </li>
                                        <li>
                                            <Cancel className="clickable icon" onClick={commentHandler} />
                                        </li>
                                    </ul>
                                </form>
                            </li>
                        }

                        {comments.items?.map(c =>
                            <Fragment key={c.id}>
                                {editingComment === c.id &&
                                    <li>
                                        <form className="comment-create column content">
                                            <textarea className="full-width" name="content" minLength={4} maxLength={32767} defaultValue={c.content} required></textarea>

                                            {submitCommentError && <span className="centered error">An error occurred. Please try again later</span>}

                                            <ul className="row option-bar">
                                                <li>
                                                    <Confirm className="clickable icon" onClick={confirmEditCommentHandler} />
                                                </li>
                                                <li>
                                                    <Cancel className="clickable icon" onClick={() => editCommentHandler()} />
                                                </li>
                                            </ul>
                                        </form>
                                    </li>
                                }

                                {editingComment !== c.id &&
                                    <li className="column">
                                        <article className="content">
                                            <div className="info-bar row">
                                                <h4 className="description">
                                                    <Link to={`/user/${c.user.id}`}>
                                                        {c.user.banned ? <s>{c.user.userName}</s> : c.user.userName}
                                                    </Link>
                                                </h4>

                                                <h4 className="description calendar">{dayjs(c.createdDate).calendar()}</h4>
                                            </div>

                                            {c.content}

                                            {(account?.admin || c.user.id === account?.id) &&
                                                (
                                                    (readyToDeleteComment === c.id || readyToDeleteComment === undefined) &&
                                                    editingComment === undefined
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
                                                            {account!.id === c.user.id &&
                                                                <li>
                                                                    <Edit className="clickable icon" onClick={() => editCommentHandler(c.id)} />
                                                                </li>
                                                            }
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

                    {isLoadingComments ?
                        <Loader /> :
                        (!comments.isLast &&
                            <button className="centered" onClick={loadMoreCommentsHandler}>Load more</button>
                        )
                    }
                </section>
            }
        </section>
    );
}