import { Routes, Route } from "react-router-dom";

import { Error, Navbar } from "./components";
import {User, Home, Thread, Account} from "./pages";

import "./index.css";

import Login from "./pages/Auth/Login";
import Register from "./pages/Auth/Register";

export default function App() {
  return (
      <>
          <Navbar />

          <Routes>
              <Route path="/" element={ <Home /> } />

              <Route path="*" element={ <Error message="Page not found" /> } />

              <Route path="thread/:id" element={ <Thread /> } />
              <Route path="user/:id" element={ <User /> } />

              <Route path="login" element={ <Login /> } />
              <Route path="register" element={ <Register /> } />

              <Route path="account" element={ <Account /> } />
          </Routes>
      </>
  );
}
