import axios from 'axios';

const API_URL = process.env.REACT_APP_API_URL;

export const TodoService = {
  async fetchTodos(token: string) {
    const response = await axios.get(`${API_URL}todos`, { headers: { Authorization: `Bearer ${token}` } });
    return response.data;
  },

  async addTodo(todo: string, token: string) {
    await axios.post(`${API_URL}todos`, { title: todo, isCompleted: false }, { headers: { Authorization: `Bearer ${token}` } });
  },

  async updateTodo(id: number, title: string, token: string) {
    await axios.put(`${API_URL}todos/${id}`, { title, isCompleted: false }, { headers: { Authorization: `Bearer ${token}` } });
  },

  async deleteTodo(id: number, token: string) {
    await axios.delete(`${API_URL}todos/${id}`, { headers: { Authorization: `Bearer ${token}` } });
  },
};
