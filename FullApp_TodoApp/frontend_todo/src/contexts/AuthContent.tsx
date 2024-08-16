import React, { createContext, useState, useContext, ReactNode } from "react";
import authService from "../services/auth.service";

interface AuthContextProps {
  userToken: string | null;
  login: (email: string, password: string) => Promise<void>;
  logout: () => void;
}

interface AuthProviderProps {
  children: ReactNode;  // Explicitly define the type for children
}

const AuthContext = createContext<AuthContextProps | undefined>(undefined);

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [userToken, setUserToken] = useState<string | null>(authService.getCurrentUser());

  const login = async (email: string, password: string) => {
    const response = await authService.login(email, password);
    setUserToken(response.token);
  };

  const logout = () => {
    authService.logout();
    setUserToken(null);
  };

  return (
    <AuthContext.Provider value={{ userToken, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};
