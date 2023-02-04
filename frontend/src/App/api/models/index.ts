export interface IEntity {
    id?: number
}

export interface IUser extends IEntity {
    userName: string,

    banned: boolean,

    admin: boolean
}

export interface IAccount extends IUser {
    email: string,
    emailConfirmed: boolean
}

export interface IThread extends IEntity {
    user: IUser;

    createdDate: string;
    modifiedDate?: string;

    title: string;
    content: string;

    views: number;

    tags: ITag[];

    commentCount: number;
}

export interface ITag extends IEntity {
    name: string
}

export interface IComment extends IEntity {
    user: IUser;

    createdDate: string;
    modifiedDate?: string;

    threadId: number;

    content: string;
}

export interface IPage<T extends IEntity> {
    items: T[],

    isLast: boolean
}