import React from "react";
import { Navigate, useLocation, Outlet } from "react-router-dom";
import { useAuth } from "../contexts/AuthContent";

interface PrivateRouteProps {
  component: React.ComponentType<any>;
}

const PrivateRoute: React.FC<PrivateRouteProps> = ({ component: Component }) => {
  const { userToken } = useAuth();
  const location = useLocation();

  if (!userToken) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  return <Component />;
};

export default PrivateRoute;
