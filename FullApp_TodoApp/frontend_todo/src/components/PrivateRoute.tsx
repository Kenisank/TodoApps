import React from "react";
import { Navigate, useLocation, Outlet } from "react-router-dom";
import { useAuth } from "../contexts/AuthContent";

const PrivateRoute: React.FC = () => {
  const { userToken } = useAuth();
  const location = useLocation();

  if (!userToken) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  return <Outlet />; // Render the child routes if authenticated
};

export default PrivateRoute;
