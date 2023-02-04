import React from "react";
import { Routes, Route } from "react-router-dom";
import dayjs from "dayjs";
import Calendar from "dayjs/plugin/calendar";

import { Navbar, AuthRequired } from "./components";

import {
    User,
    Home,
    Thread,
    Account,
    Login,
    Signup,
    ConfirmEmail,
    ChangeEmail,
    ThreadCreate,
    ResetPassword,
    TagCreate,
    TagEdit
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
                          <AuthRequired>
                              <Account />
                          </AuthRequired>
                      } />

                      <Route path="login" element={<Login />} />
                      <Route path="signup" element={<Signup />} />

                      <Route path="confirm-email" element={<ConfirmEmail />} />
                      <Route path="change-email" element={
                          <AuthRequired>
                              <ChangeEmail />
                          </AuthRequired>
                      } />

                      <Route path="reset-password" element={<ResetPassword />} />
                  </Route>

                  <Route path="thread">
                      <Route path=":id" element={<Thread />} />

                      <Route path="create" element={
                          <AuthRequired>
                              <ThreadCreate />
                          </AuthRequired>
                      } />
                  </Route>

                  <Route path="tag">
                      <Route path="create" element={
                          <AuthRequired>
                              <TagCreate />
                          </AuthRequired>
                      } />

                      <Route path="edit/:id" element={
                          <AuthRequired>
                              <TagEdit />
                          </AuthRequired>
                      } />
                  </Route>

                  <Route path="user">
                      <Route path=":id" element={<User />} />
                  </Route>
              </Route>

              <Route path="*" element={<h1 className="error title">Page Not Found</h1>} />
          </Routes>
      </>
  );
}
