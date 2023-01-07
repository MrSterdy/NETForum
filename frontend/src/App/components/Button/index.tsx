import "./index.css";

interface ButtonProps {
    name: string,
    onClick: () => void;
}

export default function Button({ name, onClick }: ButtonProps) {
    return <button type="button" className="btn" onClick={ onClick }>{ name }</button>;
}