import React from 'react';
import { Button } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import '../styles/todo.style.scss'

interface todoProps{
    todo:string;
    setTodo: React.Dispatch<React.SetStateAction<string>>;
    handleAddItem:(e:React.FormEvent)=>void;
}

const InputField = ({todo, setTodo, handleAddItem}:todoProps) => {
  return (
    <form className="input" onSubmit={handleAddItem}>
      <input type="input" 
      value={todo}
      onChange={(e)=>setTodo(e.target.value)}
      placeholder="Enter a new to-do item" className="inputBox" />
      <input
        type="submit"
        className="addButton"
        value="+"
      />
    
     
    </form>
  );
}

export default InputField;
