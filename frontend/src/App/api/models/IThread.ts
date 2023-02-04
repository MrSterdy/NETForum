import IEntity from "./IEntity";
import IUser from "./IUser";
import ITag from "./ITag";

export default interface IThread extends IEntity {
    user: IUser;

    createdDate: string;

    title: string;
    content: string;

    views: number;

    tags: ITag[];

    commentCount: number;
}