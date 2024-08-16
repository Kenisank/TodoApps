import React, { useEffect, useState } from "react";
import { List, ListItem, ListItemText, Divider, CircularProgress, Typography } from "@mui/material";
import todoService from "../services/todo.service";

const Todo = () => {
  const [todos, setTodos] = useState<any[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchTodos = async () => {
      try {
        const data = await todoService.getTodos();
        setTodos(data);
      } catch (error) {
        setError("Failed to fetch todos");
      } finally {
        setLoading(false);
      }
    };

    fetchTodos();
  }, []);

  if (loading) return <CircularProgress />;
  if (error) return <Typography color="error">{error}</Typography>;

  return (
    <List>
      {todos.map((todo: any, index: number) => (
        <div key={todo.id}>
          <ListItem>
            <ListItemText primary={todo.title} secondary={todo.isCompleted ? "Completed" : "Pending"} />
          </ListItem>
          {index < todos.length - 1 && <Divider />}
        </div>
      ))}
    </List>
  );
};

export default Todo;
