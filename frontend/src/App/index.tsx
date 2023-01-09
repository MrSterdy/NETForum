import { Routes, Route } from "react-router-dom";

import { Error, Navbar } from "./components";
import { User, Home, Thread } from "./pages";

import "./index.css";

import Login from "./pages/Auth/Login";

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
          </Routes>
      </>
  );
}
