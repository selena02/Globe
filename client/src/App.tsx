import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";
import Header from "./shared/Header/Header";
import Footer from "./shared/Footer/Footer";
import Home from "./features/Home/Home";
import Account from "./features/Account/Account";
import Login from "./features/Account/Login/Login";
import Register from "./features/Account/Register/Register";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import NotFound from "./shared/NotFound/NotFound";
import { useDispatch } from "react-redux";
import { useEffect } from "react";
import { setLogin } from "./state/features/authSlice";
import PrivateRoute from "./shared/guards/PrivateRoute";
import Profile from "./features/Profile/Profile";
import LogOutGuard from "./shared/guards/LogOutGuard";
import Feed from "./features/Feed/Feed";
import Posts from "./features/Profile/Posts/Posts";

const App = () => {
  const dispatch = useDispatch();

  useEffect(() => {
    const token = localStorage.getItem("token");
    const user = JSON.parse(localStorage.getItem("user") as string);

    if (token && user) {
      dispatch(setLogin({ user, token }));
    }
  }, [dispatch]);

  return (
    <Router>
      <div id="app-container">
        <Header />
        <main id="content-wrapper">
          <ToastContainer />
          <Routes>
            <Route path="/" element={<Home />} />
            <Route
              path="account"
              element={
                <LogOutGuard>
                  <Account />
                </LogOutGuard>
              }
            >
              <Route path="login" element={<Login />} />
              <Route path="register" element={<Register />} />
            </Route>
            <Route
              path="/profile/:id"
              element={
                <PrivateRoute>
                  <Profile />
                </PrivateRoute>
              }
            >
              <Route path="" element={<Navigate to="posts" replace />} />
              <Route path="posts" element={<Posts />} />
              <Route path="landmarks" element={<Register />} />
            </Route>
            <Route path="feed" element={<Feed />} />
            <Route path="*" element={<NotFound />} />
          </Routes>
        </main>
        <Footer />
      </div>
    </Router>
  );
};

export default App;
