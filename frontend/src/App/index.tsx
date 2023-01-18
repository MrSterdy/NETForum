import { Routes, Route } from "react-router-dom";
import dayjs from "dayjs";
import Calendar from "dayjs/plugin/calendar";

import { Error, Navbar, AuthRoute } from "./components";
import { User, Home, Thread, Account, Login, Signup, Confirm, ThreadCreate } from "./pages";

import "./index.css";

dayjs.extend(Calendar);

export default function App() {
  return (
      <>
          <Navbar />

          <Routes>
              <Route path="/" element={ <Home /> } />

              <Route path="*" element={ <Error message="Page not found" /> } />

              <Route path="thread/:id" element={ <Thread /> } />
              <Route path="thread/create" element={
                  <AuthRoute>
                      <ThreadCreate />
                  </AuthRoute>
              } />

              <Route path="user/:id" element={ <User /> } />

              <Route path="login" element={ <Login /> } />
              <Route path="signup" element={ <Signup /> } />
              <Route path="confirm" element={ <Confirm /> } />

              <Route path="account" element={
                  <AuthRoute>
                      <Account />
                  </AuthRoute>
              } />
          </Routes>
      </>
  );
}
