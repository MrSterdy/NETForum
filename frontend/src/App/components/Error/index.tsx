import "./index.css";

interface ErrorProps {
    message: string
}

export default function Error({ message }: ErrorProps) {
    return (
        <span className="error">{ message }</span>
    );
}