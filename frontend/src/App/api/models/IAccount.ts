import IUser from "./IUser";

export default interface IAccount extends IUser {
    email: string,
    emailConfirmed: boolean
}