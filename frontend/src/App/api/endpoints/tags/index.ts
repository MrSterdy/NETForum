import axios from "redaxios";

import { IPage, ITag } from "../../models";

const endpoint = process.env.REACT_APP_TAGS_URL!;

export function getTags(page: number, name?: string) {
    const params: Record<string, any> = { page };

    if (name !== undefined)
        params["name"] = name;

    return axios.get<IPage<ITag>>(endpoint, { params })
}