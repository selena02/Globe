import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Header from "./shared/Header/Header";
import Footer from "./shared/Footer/Footer";
import Home from "./features/Home/Home";
import Account from "./features/Account/Account";
import Login from "./features/Account/Login/Login";
import Register from "./features/Account/Register/Register";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const App = () => {
  return (
    <Router>
      <div id="app-container">
        <Header />
        <main id="content-wrapper">
          <ToastContainer />
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="account" element={<Account />}>
              <Route path="login" element={<Login />} />
              <Route path="register" element={<Register />} />
            </Route>
          </Routes>
        </main>
        <Footer />
      </div>
    </Router>
  );
};

export default App;
