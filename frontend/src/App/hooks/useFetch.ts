import { useCallback, useEffect, useState } from "react";
import { Response } from "redaxios";

export default function useFetch<T>(func: (...args: any[]) => Promise<Response<T>>, ...params: any[]) {
    const [data, setData] = useState<T>({} as T);
    const [isLoading, setLoading] = useState(true);
    const [error, setError] = useState(false);

    const callback = useCallback(() => func(...params), params);

    useEffect(() => {
        callback()
            .then(res => setData(res.data))
            .catch(() => setError(true))
            .finally(() => setLoading(false));
    }, [callback]);

    return { data, isLoading, error };
}