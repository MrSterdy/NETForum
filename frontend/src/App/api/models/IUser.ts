import IEntity from "./IEntity";

export default interface IUser extends IEntity {
    email: string,
    userName: string,
    confirmed: boolean
}