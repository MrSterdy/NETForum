import { useCallback, useEffect, useState } from "react";
import { Response } from "redaxios";

export default function useFetch<T>(func: (...args: any[]) => Promise<Response<T>>, ...params: any[]) {
    const [data, setData] = useState<T>({} as T);
    const [isLoading, setLoading] = useState(true);
    const [error, setError] = useState(0);

    // eslint-disable-next-line react-hooks/exhaustive-deps
    const callback = useCallback(() => func(...params), params);

    useEffect(() => {
        callback()
            .then(res => setData(res.data))
            .catch(res => setError((res as Response<unknown>).status))
            .finally(() => setLoading(false));
    }, [callback]);

    return { data, isLoading, error };
}