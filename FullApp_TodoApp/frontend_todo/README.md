# Multi-User To-Do App

## Overview
This is a multi-user To-Do application built using React, TypeScript, and an ASP.NET Minimal API backend. The app supports authentication, ensures data privacy, and follows best practices in design, code structure, logging, auditing, documentation, and testing.

## Features
- User Authentication (Login/Register)
- Todo management (Add, Edit, Delete, View)
- Separation of frontend into reusable components
- API request handling with services
- Global state management with Context API
- Logging of user actions and errors

## Folder Structure
- `src/`
  - `components/` - Contains reusable components (e.g., `TodoItem`, `Header`).
  - `contexts/` - Contains context providers (e.g., `AuthContext`).
  - `services/` - Handles API requests (e.g., `authService`, `todoService`, `logger`).
  - `pages/` - Contains page components (e.g., `Login`, `Register`, `Todo`).
  - `styles/` - Contains global and component-specific styles.
  - `tests/` - Contains unit and integration tests.

## Setup
1. Clone the repository:
   ```bash
   git clone https://github.com/Kenisank/TodoApps.git
   cd FullApp_TodoApp/frontend_todo


### Install dependencies:
npm install

### Create a .env file with the following content:
REACT_APP_API_URL=http://localhost:5000/api

### Start the development server:
npm start