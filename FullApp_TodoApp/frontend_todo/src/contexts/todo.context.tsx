import React, { createContext, useState, useCallback, ReactNode } from 'react';
import { TodoService } from '../services/todo.service';

interface Todo {
  id: number;
  title: string;
  isCompleted: boolean;
}

interface TodoContextProps {
  todos: Todo[];
  fetchTodos: () => Promise<void>;
  addTodo: (title: string) => Promise<void>;
  updateTodo: (id: number, title: string) => Promise<void>;
  deleteTodo: (id: number) => Promise<void>;
}

const defaultContext: TodoContextProps = {
  todos: [],
  fetchTodos: async () => {},
  addTodo: async () => {},
  updateTodo: async () => {},
  deleteTodo: async () => {},
};

export const TodoContext = createContext<TodoContextProps>(defaultContext);

export const TodoProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [todos, setTodos] = useState<Todo[]>([]);
  const token = localStorage.getItem('authToken') || '';

  const fetchTodos = useCallback(async () => {
    try {
        debugger;
      const todos = await TodoService.fetchTodos(token);
      debugger;
      setTodos(todos);
    } catch (err) {
      console.error('Failed to fetch todos:', err);
    }
  }, [token]);


  debugger;
  const addTodo = useCallback(async (title: string) => {
    debugger;
    try {
        debugger;
      await TodoService.addTodo(title, token);
      debugger;
      await fetchTodos();
    } catch (err) {
      console.error('Failed to add todo:', err);
    }
  }, [fetchTodos, token]);

  const updateTodo = useCallback(async (id: number, title: string) => {
    try {
      await TodoService.updateTodo(id, title, token);
      await fetchTodos();
    } catch (err) {
      console.error('Failed to update todo:', err);
    }
  }, [fetchTodos, token]);

  const deleteTodo = useCallback(async (id: number) => {
    try {
      await TodoService.deleteTodo(id, token);
      await fetchTodos();
    } catch (err) {
      console.error('Failed to delete todo:', err);
    }
  }, [fetchTodos, token]);

  return (
    <TodoContext.Provider value={{ todos, fetchTodos, addTodo, updateTodo, deleteTodo }}>
      {children}
    </TodoContext.Provider>
  );
};
