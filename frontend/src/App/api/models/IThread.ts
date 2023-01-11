import IEntity from "./IEntity";
import IUser from "./IUser";

export default interface IThread extends IEntity {
    user: IUser,
    
    title: string,
    content: string
}