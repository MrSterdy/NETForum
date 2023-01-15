import IEntity from "./IEntity";
import IUser from "./IUser";
import IComment from "./IComment";

export default interface IThread extends IEntity {
    user: IUser,
    
    title: string,
    content: string

    comments: IComment[]
}