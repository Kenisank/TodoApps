import React, { useEffect, useState } from "react";
import { List, ListItem, ListItemText, Divider, CircularProgress, Typography, Box, Button } from "@mui/material";
import todoService from "../services/todo.service";
import { useAuth } from "../contexts/auth.context";
import "../styles/todo.style.css"; 

// Define an interface for Todo items
interface TodoItem {
  id: number;
  title: string;
  isCompleted: boolean;
}

const Todo = () => {
  const { logout } = useAuth();
  const [todos, setTodos] = useState<TodoItem[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchTodos = async () => {
      try {
        const data = await todoService.getTodos();
        const demoTodo: TodoItem = { id: 1, title: "Demo Todo Item", isCompleted: false };
        setTodos([...data, demoTodo]);
      } catch (error) {
        setError("Failed to fetch todos. Please try again later.");
        console.error("Error fetching todos:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchTodos();
  }, []);

  if (loading) return <CircularProgress />;
  if (error) return <Typography color="error">{error}</Typography>;

  return (
    <Box sx={{ padding: 2 }}>
      <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', marginBottom: 2 }}>
        <Typography variant="h4" component="h1" sx={{ color: '#3f51b5', fontWeight: 'bold' }}>
          To-do List
        </Typography>
        <Button
          variant="contained"
          color="primary"
          onClick={logout}
        >
          Logout
        </Button>
      </Box>
      <List>
        {todos.map((todo) => (
          <div key={todo.id}>
            <ListItem>
              <ListItemText
                primary={todo.title}
                secondary={todo.isCompleted ? "Completed" : "Pending"}
              />
            </ListItem>
            <Divider />
          </div>
        ))}
      </List>
    </Box>
  );
};

export default Todo;
