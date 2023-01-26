import IEntity from "./IEntity";

export default interface IUser extends IEntity {
    userName: string,
    enabled: boolean,
    admin: boolean
}