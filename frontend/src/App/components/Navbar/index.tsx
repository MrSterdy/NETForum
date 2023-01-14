import { Link } from "react-router-dom";

import { ReactComponent as Logo } from "../../assets/icons/logo.svg";
import "./index.css";

export default function Navbar() {
    return (
        <nav>
            <Logo />

            <ul>
                <li>
                    <Link to="/account">Account</Link>
                </li>
            </ul>
        </nav>
    );
}