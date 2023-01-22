import { Link } from "react-router-dom";

import { ReactComponent as Logo } from "../../assets/icons/logo.svg";

import { useAuth } from "../../hooks";

import "./index.css";

export default function Navbar() {
    const { account } = useAuth();

    return (
        <nav className="full-width">
            <Logo />

            <ul>
                <li>
                    {!account?.confirmed && <Link to="/account/login">Log in</Link>}
                    {account?.confirmed && <Link to={`/user/${account.id}`}>{account.userName}</Link>}
                </li>
            </ul>
        </nav>
    );
}