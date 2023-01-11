import { Link } from "react-router-dom";

import "./index.css";

export default function Navbar() {
    return (
        <nav>
            <object data="/assets/icons/logo.svg">Logo</object>

            <ul>
                <li>
                    <Link to="/account">Account</Link>
                </li>
            </ul>
        </nav>
    );
}