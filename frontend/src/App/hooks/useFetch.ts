import { useEffect, useState } from "react";
import axios from "axios";

export default function useFetch<T>(url: string) {
    const [data, setData] = useState<T>();
    const [isLoading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        axios.get<T>(url)
            .then(res => setData(res.data))
            .catch(err => setError((err as Error).message))
            .finally(() => setLoading(false));
    }, [url]);

    return { data, isLoading, error };
}