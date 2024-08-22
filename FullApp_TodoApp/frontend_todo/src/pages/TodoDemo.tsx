import { Box, Typography, Button, List, ListItem, ListItemText, Divider, CircularProgress } from '@mui/material'
import LogoutIcon from '@mui/icons-material/Logout';
import React, { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/auth.context';
import log from '../services/logger.service';
import '../styles/todo.style.scss'
import InputField from '../components/InputField';
import { TodoItem } from '../models/todo.model';
import TodoList from '../components/TodoList';
import todoService from '../services/todo.service';

const TodoDemo:React.FC = () => {
    const navigate = useNavigate();
    const { logout } = useAuth();
    const[todo, setTodo]=useState<string>("");
    const [todos, setTodos] = useState<TodoItem[]>([]);
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
      

    useEffect(() => {
      const fetchTodos = async () => {
        log.info("Fetching todos");
        try {
          const data = await todoService.getTodos();
          
          setTodos([...data]);
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
      }
      

      const handleAddItem=async(e:React.FormEvent)=>{
        e.preventDefault();
        


        
        if(todo){
          debugger;
 
          log.info("Adding todo item");
          try {
            debugger;
            const response = await todoService.addTodo({
              title: todo,
              isCompleted: false
            });
         
            debugger;
            if (response!=null || response!=undefined) {
              console.log('New todo added:', response.data);
              log.info("Todos fetched successfully");
              setTodos([...todos, response]);
            setTodo(""); // Clear the input field
            } else {
              console.error('Failed to add todo');
            }
          
          } catch (error) {
            log.error("Error fetching todo item ", { error });
            setError("Failed to add todo item. Please try again later.");
          } finally {
            setLoading(false);
          }
        }


      }


      
         if (loading) return <CircularProgress />;
       
         if (error) return <Typography color="error">{error}</Typography>;
         

  return (
    <Box sx={{ padding: 2 }} >
      <Box className="header-container">
        <Box className="top">
        <Typography variant="h4" component="h1"  className="header">
          My Todo List
        </Typography>
        <Button variant="contained" className='logout-button' sx={{ backgroundColor:'white', color:'red', fontFamily: 'Trebuchet MS, sans-serif' }} onClick={handleLogout} startIcon={<LogoutIcon />}>
          Logout
        </Button>

        </Box>
        <InputField todo={todo} setTodo={setTodo} handleAddItem={handleAddItem} />
      </Box>
      
      <TodoList todos={todos} setTodos={ setTodos}/>

      <List sx={{ fontFamily: 'Trebuchet MS, sans-serif' }}>
        
      </List>
    </Box>
  )
}

export default TodoDemo
