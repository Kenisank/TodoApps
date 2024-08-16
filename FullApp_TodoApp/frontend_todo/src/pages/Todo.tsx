import React, { useEffect, useState } from "react";
import { List, ListItem, ListItemText, Divider, CircularProgress, Typography, Box } from "@mui/material";
import todoService from "../services/todo.service";

// Define an interface for Todo items
interface TodoItem {
  id: number;
  title: string;
  isCompleted: boolean;
}

const Todo = () => {
  const [todos, setTodos] = useState<TodoItem[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchTodos = async () => {
      try {
        // Fetch todos from the API
        const data = await todoService.getTodos();

        // Add a demo todo item
        const demoTodo: TodoItem = { id: 1, title: "Demo Todo Item", isCompleted: false };
        
        // Set todos state with fetched data and demo item
        setTodos([...data, demoTodo]);
      } catch (error) {
        setError("Failed to fetch todos. Please try again later.");
        console.error("Error fetching todos:", error); // Log the error for debugging
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
      <Typography
        variant="h4"
        component="h1"
        sx={{
          color: "#3f51b5", // Primary color
          fontWeight: "bold",
          marginBottom: 2,
          textAlign: "center", // Center align the text
        }}
      >
        To-do List
      </Typography>
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
