import IEntity from "./IEntity";

export default interface IUser extends IEntity {
    email: string,
    emailConfirmed: boolean,
    username: string
}