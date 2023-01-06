import { Routes, Route } from "react-router-dom";

import { Navbar } from "./components";
import Home from "./pages/Home";

import "./index.css";

export default function App() {
  return (
      <>
          <Navbar />

          <Routes>
              <Route path="/">
                  <Route index element={ <Home /> } />
              </Route>
          </Routes>
      </>
  );
}
