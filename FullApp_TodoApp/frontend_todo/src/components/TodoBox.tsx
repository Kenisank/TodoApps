import React, { useState } from 'react'
import { TodoItem } from '../models/todo.model'
import EditNoteIcon from '@mui/icons-material/EditNote';
import DeleteOutlineOutlinedIcon from '@mui/icons-material/DeleteOutlineOutlined';
import FileDownloadDoneOutlinedIcon from '@mui/icons-material/FileDownloadDoneOutlined';
import UndoOutlinedIcon from '@mui/icons-material/UndoOutlined';
import log from '../services/logger.service';
import todoService from '../services/todo.service';
import { CircularProgress, Typography } from '@mui/material';

interface Props{
    todo:TodoItem;
    todos:TodoItem[];
        setTodos: React.Dispatch<React.SetStateAction<TodoItem[]>>;
}

const TodoBox = ({todo, todos,setTodos}:Props) => {
  const [error, setError] = useState<string | null>(null);
 
  const [edit, setEdit] = useState<boolean>(false);
  const [editTodo, setEditTodo] = useState<string>(todo.title);


  const completedClass = todo.isCompleted ? 'completed' : 'reverse';

    const handleDoneTodo=async(id: number)=>{
      debugger;
      
        
          
  
  
          
          if(todo){
            debugger;
   
            log.info("Update isComplete Status");
            try {
              debugger;
              const response = await todoService.updateCompletedStatus(id);
           
              debugger;
              if (response!=null || response!=undefined) {
                console.log('is Complete Status is updated', response);
                log.info("is Complete updated successfully");
                debugger;
                setTodos(todos.map((todo)=>
                  todo.id===id?{...todo, isCompleted:!todo.isCompleted}:todo))
                      
              } else {
                console.error('Failed to update isComplete');
              }
            
            } catch (error) {
              log.error("Error updating isComplete ", { error });
              setError("Failed to update isComplete of todo item. Please try again later.");
            } 
          
      };

    };

    const  handleDeleteTodo=async(id: number)=>{
      debugger;
      
        
          
  
  
          
          if(todo){
            debugger;
   
            log.info("Delete Todo Item");
            try {
              debugger;
              const response = await todoService.deleteTodo(id);
           
              debugger;
              if (response!=null || response!=undefined) {
                console.log('Todo Item Delete', response);
                log.info("Deleted successfully");
                debugger;
                setTodos(todos.filter((todo)=>
                  todo.id!==id))
                      
              } else {
                console.error('Failed to delete todo');
              }
            
            } catch (error) {
              log.error("Error delete todo item ", { error });
              setError("Failed to delete todo item. Please try again later.");
            } 
          
      };

    };
      



    const handleEditTodo=async(e:React.FormEvent, id:number)=>{
      e.preventDefault();

      if(todo){
        debugger;

        log.info("Editing Todo Item");
        try {
          debugger;
          const response = await todoService.updateTodo(id,  {
            title: editTodo,
            isCompleted: todo.isCompleted
          });
       
          debugger;
          if (response!=null || response!=undefined) {
            console.log('The todo item is updated', response);
            log.info("The todo item updated successfully");
            debugger;
            setTodos(todos.map((todo)=>
              todo.id===id?{...todo, title:editTodo}:todo));
            setEdit(false);
                  
          } else {
            console.error('Failed to update isComplete');
          }
        
        } catch (error) {
          log.error("Error updating the todo item ", { error });
          setError("Failed to update todo item. Please try again later.");
        } 
      
  };


    }

    
      
       
      if (error) return <Typography color="error">{error}</Typography>;
      



  return (
    <form className={`eachTodo ${completedClass}`} onSubmit={(e)=>handleEditTodo(e,todo.id)} >
      { 
        edit?(
          <input value={editTodo} onChange={(e)=>setEditTodo(e.target.value)}   className="eachTodoTitleInput" />
        ):(

          <span className="eachTodoTitle">{todo.title}</span>
        )
      }

      <div>
        
        {
          
        }
        <span className="icon"
        onClick={()=>{if(!edit&&!todo.isCompleted){
          setEdit(!edit);
        }
        }}
        > {todo.isCompleted?'':<EditNoteIcon />} </span>
        <span className="icon"
         onClick={()=>handleDeleteTodo(todo.id)}
        ><DeleteOutlineOutlinedIcon/></span>
        <span className="icon"
        onClick={()=>handleDoneTodo(todo.id)}
        >{todo.isCompleted?<UndoOutlinedIcon/>:<FileDownloadDoneOutlinedIcon/>}</span>

      </div>
    </form>
  )
}

export default TodoBox
