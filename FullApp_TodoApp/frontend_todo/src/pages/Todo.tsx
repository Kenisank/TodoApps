import React, { useEffect, useState } from "react";
import { List, ListItem, ListItemText, Divider, CircularProgress, Typography, Box, Button } from "@mui/material";
import todoService from "../services/todo.service";
import { useAuth } from "../contexts/auth.context";
import { useNavigate } from "react-router-dom"; 
import "../styles/todo.style.css";
import log from "../services/logger.service"; // Import the logger

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
  const navigate = useNavigate();

  useEffect(() => {
    const fetchTodos = async () => {
      log.info("Fetching todos");
      try {
        const data = await todoService.getTodos();
        const demoTodo: TodoItem = { id: 1, title: "Demo Todo Item", isCompleted: false };
        setTodos([...data, demoTodo]);
        log.info("Todos fetched successfully");
      } catch (error) {
        log.error("Error fetching todos", { error });
        setError("Failed to fetch todos. Please try again later.");
      } finally {
        setLoading(false);
      }
    };

    fetchTodos();
  }, []);

  const handleLogout = () => {
    log.info("User logged out");
    logout();
    navigate("/login");
  };

  if (loading) return <CircularProgress />;
  if (error) return <Typography color="error">{error}</Typography>;

  return (
    <Box sx={{ padding: 2 }}>
      <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', marginBottom: 2 }}>
        <Typography variant="h4" component="h1">
          My Todo List
        </Typography>
        <Button variant="contained" color="secondary" onClick={handleLogout}>
          Logout
        </Button>
      </Box>
      <List>
        {todos.map((todo) => (
          <React.Fragment key={todo.id}>
            <ListItem button>
              <ListItemText
                primary={todo.title}
                secondary={todo.isCompleted ? "Completed" : "Not Completed"}
              />
            </ListItem>
            <Divider />
          </React.Fragment>
        ))}
      </List>
    </Box>
  );
};

export default Todo;
