import axios from "axios";

// Ensure that the API URL is correctly set in environment variables
const API_URL = process.env.REACT_APP_API_URL;

// Get the stored authentication token
const getAuthToken = () => {
    debugger;
  return localStorage.getItem("userToken");
};

// Create an instance of axios with default configuration
const apiClient = axios.create({
  baseURL: API_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

// Add a request interceptor to include the token in headers
apiClient.interceptors.request.use(
  (config) => {
    const token = getAuthToken();
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Define the CRUD operations
const getTodos = async () => {
  try {
    const response = await apiClient.get("todos");
    return response.data;
  } catch (error) {
    console.error("Failed to fetch todos", error);
    throw error;
  }
};

const addTodo = async (todo: { title: string; isCompleted: boolean }) => {
  try {
    const response = await apiClient.post("todos", todo);
    return response.data;
  } catch (error) {
    console.error("Failed to add todo", error);
    throw error;
  }
};

const updateTodo = async (id: number, updatedTodo: { title: string; isCompleted: boolean }) => {
  try {
    const response = await apiClient.put(`todos/${id}`, updatedTodo);
    return response.data;
  } catch (error) {
    console.error("Failed to update todo", error);
    throw error;
  }
};

const deleteTodo = async (id: number) => {
  try {
    const response = await apiClient.delete(`todos/${id}`);
    return response.data;
  } catch (error) {
    console.error("Failed to delete todo", error);
    throw error;
  }
};

export default {
  getTodos,
  addTodo,
  updateTodo,
  deleteTodo,
};
