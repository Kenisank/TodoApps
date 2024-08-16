import axios from "axios";

const API_URL = process.env.REACT_APP_API_URL;

const login = async (email: string, password: string) => {
    debugger;
  const response = await axios.post(`${API_URL}auth/login`, { email, password });
  if (response.data.token) {
    debugger;
    localStorage.setItem("userToken", response.data.token);
  }
  debugger;
  return response.data;
};

const logout = () => {
  localStorage.removeItem("userToken");
};

const getCurrentUser = () => {
  return localStorage.getItem("userToken");
};

export default {
  login,
  logout,
  getCurrentUser,
};
