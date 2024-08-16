import axios from "axios";


const API_URL = process.env.REACT_APP_API_URL;


const register = async (username: string, email: string, password: string) => {
  
  const response = await axios.post(`${API_URL}auth/register`, { username, email, password });
 
  if (response.data.token) {
    debugger;
    localStorage.setItem("userToken", response.data.token);
  }
  debugger;
  return response.data;
};

const login = async (username: string, password: string) => {
    debugger;
  const response = await axios.post(`${API_URL}auth/login`, { username, password });

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
  register,
  login,
  logout,
  getCurrentUser,
};
