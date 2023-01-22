import { Routes, Route } from "react-router-dom";
import dayjs from "dayjs";
import Calendar from "dayjs/plugin/calendar";

import { Error, Navbar, AuthProvider } from "./components";
import {
    User,
    Home,
    Thread,
    Account,
    Login,
    Signup,
    ConfirmEmail,
    ConfirmNewEmail,
    ThreadCreate,
    ResetPassword
} from "./pages";

import "./index.css";

dayjs.extend(Calendar);

export default function App() {
  return (
      <>
          <Navbar />

          <Routes>
              <Route path="/">
                  <Route index element={<Home />} />

                  <Route path="account">
                      <Route index element={
                          <AuthProvider>
                              <Account />
                          </AuthProvider>
                      } />

                      <Route path="login" element={<Login />} />
                      <Route path="signup" element={<Signup />} />

                      <Route path="confirm-email" element={<ConfirmEmail />} />
                      <Route path="confirm-new-email" element={
                          <AuthProvider>
                              <ConfirmNewEmail />
                          </AuthProvider>
                      } />

                      <Route path="reset-password" element={<ResetPassword />} />
                  </Route>

                  <Route path="thread">
                      <Route path=":id" element={<Thread />} />

                      <Route path="create" element={
                          <AuthProvider>
                              <ThreadCreate />
                          </AuthProvider>
                      } />
                  </Route>

                  <Route path="user">
                      <Route path=":id" element={<User />} />
                  </Route>
              </Route>

              <Route path="*" element={<Error message="Page not found" />} />
          </Routes>
      </>
  );
}
