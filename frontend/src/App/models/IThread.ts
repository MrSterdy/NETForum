import IEntity from "./IEntity";
import IUser from "./IUser";

export default interface IThread extends IEntity {
    userId: number,
    user?: IUser,
    
    title: string,
    content: string
}