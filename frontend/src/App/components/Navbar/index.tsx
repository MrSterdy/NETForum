import {Link, useNavigate} from "react-router-dom";

import { ReactComponent as Logo } from "../../assets/icons/logo.svg";

import { useAuth } from "../../hooks";

import "./index.css";

export default function Navbar() {
    const { account } = useAuth();

    const navigate = useNavigate();

    function navigateToHome() {
        navigate("/");
    }

    return (
        <nav className="full-width">
            <Logo className="clickable" onClick={navigateToHome} />

            <ul>
                <li>
                    {account?.emailConfirmed ?
                        <Link to={`/user/${account.id}`}>{account.userName}</Link> :
                        <Link to="/account/login">Log in</Link>
                    }
                </li>
            </ul>
        </nav>
    );
}