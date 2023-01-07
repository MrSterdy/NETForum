import "./index.css";

interface ErrorProps {
    message: string
}

export default function Error({ message }: ErrorProps) {
    return (
        <p className="error">{ message }</p>
    );
}