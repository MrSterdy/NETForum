import "./index.css";

interface ButtonProps {
    name: string,
    onClick: () => void;
}

export default ({ name, onClick }: ButtonProps) => 
    <button type="button" className="btn" onClick={ onClick }>{ name }</button>;
