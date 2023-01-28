import IEntity from "./IEntity";
import IUser from "./IUser";

export default interface IThread extends IEntity {
    user: IUser,

    createdDate: string,

    title: string,
    content: string

    views: number,

    commentCount: number
}