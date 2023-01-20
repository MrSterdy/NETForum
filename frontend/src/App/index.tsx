import { Routes, Route } from "react-router-dom";
import dayjs from "dayjs";
import Calendar from "dayjs/plugin/calendar";

import { Error, Navbar, AuthProvider } from "./components";
import { User, Home, Thread, Account, Login, Signup, Confirm, ThreadCreate } from "./pages";

import "./index.css";
import ConfirmNewEmail from "./pages/Account/ConfirmNewEmail";

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
                  <AuthProvider>
                      <ThreadCreate />
                  </AuthProvider>
              } />

              <Route path="user/:id" element={ <User /> } />

              <Route path="login" element={ <Login /> } />
              <Route path="signup" element={ <Signup /> } />
              <Route path="confirm" element={ <Confirm /> } />

              <Route path="account" element={
                  <AuthProvider>
                      <Account />
                  </AuthProvider>
              } />
              <Route path="account/confirmNewEmail" element={
                  <AuthProvider>
                      <ConfirmNewEmail />
                  </AuthProvider>
              } />
          </Routes>
      </>
  );
}
