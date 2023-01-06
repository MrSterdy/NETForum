import axios from "axios";
import { useEffect, useState } from "react";

export default function useFetch<T>(url: string) {
    const [data, setData] = useState<T>();
    const [isLoading, setLoading] = useState(true);
    const [error, setError] = useState("");
    
    useEffect(() => {
        const fetch = async () => {
            try {
                const res = await axios.get<T>(url);

                setData(res.data);
            } catch (e: unknown) {
                setError((e as Error).message);
            } finally {
                setLoading(false);
            }
        };
        
        fetch();
    }, [url]);
    
    return { data, isLoading, error };
}