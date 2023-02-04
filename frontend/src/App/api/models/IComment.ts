import IEntity from "./IEntity";
import IUser from "./IUser";

export default interface IComment extends IEntity {
    user: IUser;

    createdDate: string;
    modifiedDate?: string;

    threadId: number;

    content: string;
}