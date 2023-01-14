import { Routes, Route } from "react-router-dom";

import { Error, Navbar } from "./components";
import { User, Home, Thread, Account } from "./pages";

import "./index.css";

import Login from "./pages/Auth/Login";
import Signup from "./pages/Auth/Signup";
import ThreadCreate from "./pages/Thread/Create";
import AuthenticatedRoute from "./components/AuthenticatedRoute";

export default function App() {
  return (
      <>
          <Navbar />

          <Routes>
              <Route path="/" element={ <Home /> } />

              <Route path="*" element={ <Error message="Page not found" /> } />

              <Route path="thread/:id" element={ <Thread /> } />
              <Route path="thread/create" element={
                  <AuthenticatedRoute>
                      <ThreadCreate />
                  </AuthenticatedRoute>
              } />

              <Route path="user/:id" element={ <User /> } />

              <Route path="login" element={ <Login /> } />
              <Route path="signup" element={ <Signup /> } />

              <Route path="account" element={
                  <AuthenticatedRoute>
                      <Account />
                  </AuthenticatedRoute>
              } />
          </Routes>
      </>
  );
}
