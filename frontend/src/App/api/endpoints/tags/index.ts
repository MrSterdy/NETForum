import axios from "redaxios";

import { IPage, ITag } from "../../models";

const endpoint = process.env.REACT_APP_TAGS_URL!;

export function getTagById(id: number) {
    return axios.get<ITag>(endpoint, { params: { id } });
}

export function getTags(page: number, name?: string) {
    const params: Record<string, any> = { page };

    if (name)
        params.name = name;

    return axios.get<IPage<ITag>>(`${endpoint}/search`, { params })
}

export function createTag(name: string) {
    return axios.post(endpoint, { name }, { withCredentials: true });
}

export function updateTagById(id: number, name: string) {
    return axios.put(endpoint, { name }, { withCredentials: true, params: { id } });
}

export function deleteTagById(id: number) {
    return axios.delete(endpoint, { withCredentials: true, params: { id } });
}