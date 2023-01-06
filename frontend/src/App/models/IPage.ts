import IEntity from "./IEntity";

export default interface IPage<T extends IEntity> {
    items: T[],
    isLast: boolean
}