import IUser from "./IUser";

export default interface IAccount extends IUser {
    email: string,
    confirmed: boolean
}