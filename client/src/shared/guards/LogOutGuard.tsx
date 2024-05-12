import React from "react";
import { Navigate } from "react-router-dom";
import { useSelector } from "react-redux";
import { RootState } from "../../state/store"; // Make sure the path to your store is correct

interface PrivateRouteProps {
  children: JSX.Element;
}

const LogOutGuard: React.FC<PrivateRouteProps> = ({ children }) => {
  const isLoggedIn = useSelector((state: RootState) => state.auth.isLoggedIn);

  return !isLoggedIn ? children : <Navigate to="/" replace />;
};

export default LogOutGuard;
