import { Link } from "react-router-dom";

import { ReactComponent as Logo } from "../../assets/icons/logo.svg";

import { useAuth } from "../../hooks";

import "./index.css";

export default function Navbar() {
    const { user } = useAuth();

    return (
        <nav>
            <Logo />

            <ul>
                <li>
                    {!user?.confirmed && <Link to="/login">Log in</Link>}
                    {user?.confirmed && <Link to={`/user/${user.id}`}>{user.userName}</Link>}
                </li>
            </ul>
        </nav>
    );
}