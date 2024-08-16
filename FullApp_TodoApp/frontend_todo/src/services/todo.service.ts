import axios from "axios";

const API_URL = process.env.REACT_APP_API_URL;

const getTodos = async () => {
  try {
    debugger
    const response = await axios.get(`${API_URL}api/todos`);
    debugger;
    return response.data;
  } catch (error) {
    console.error("Error fetching todos:", error);
    throw error;
  }
};

export default {
  getTodos,
};
