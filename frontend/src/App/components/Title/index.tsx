import "./index.css";

interface TitleProps {
    title: string
}

export default ({ title }: TitleProps) => <h1 className="title">{ title }</h1>
