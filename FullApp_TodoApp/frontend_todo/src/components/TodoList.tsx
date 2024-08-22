import React from 'react'
import "../styles/todo.style.scss"
import { TodoItem } from '../models/todo.model'
import TodoBox from './TodoBox';


interface Props {
    todos: TodoItem[];
    setTodos:React.Dispatch<React.SetStateAction<TodoItem[]>>;
}

const TodoList = ({todos, setTodos}:Props) => {
  return (
    <div className='todoList'>
      {todos.map((todo)=>(
        <TodoBox
        todo ={todo}
        key={todo.id} 
        todos={todos}
        setTodos={setTodos}
         />
      ))}
    </div>
  )
}

export default TodoList
