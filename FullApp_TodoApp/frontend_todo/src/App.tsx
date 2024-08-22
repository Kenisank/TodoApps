import React from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import Login from './pages/Login';
import Register from './pages/Register';
import Todo from './pages/Todo';
import PrivateRoute from './components/PrivateRoute';
import TodoDemo from './pages/TodoDemo';

function App() {
  return (
    <>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        
        {/* PrivateRoute wraps the protected routes */}
        <Route element={<PrivateRoute />}>
        <Route path="/tododemo" element={<TodoDemo />} />
          <Route path="/todo" element={<Todo />} />
        </Route>
        
        {/* Fallback route */}
        <Route path="*" element={<Navigate to="/login" />} />
      </Routes>
    </>
  );
}

export default App;
