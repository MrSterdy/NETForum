import IEntity from "./IEntity";
import IUser from "./IUser";

export default interface IComment extends IEntity {
    user: IUser,
    threadId: number,

    content: string
}