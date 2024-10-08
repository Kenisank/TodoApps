# To-Do Application Backend

## Overview

This project is a multi-user To-Do application backend, built with ASP.NET Core Minimal API, Entity Framework Core, and SQL Server.

### **API Endpoints**
Endpoint	Method	Description
/api/auth/register	POST	Registers a new user
/api/auth/login	POST	Authenticates a user and returns a JWT token
/api/todos	GET	Fetches todos for the logged-in user
/api/todos	POST	Adds a new todo for the logged-in user
/api/todos/{id}	PUT	Updates an existing todo
/api/todos/{id}	DELETE	Deletes a todo item



## Features

- User Registration and Authentication (JWT)
- Todo Management (CRUD Operations)
- Logging (Serilog)
- Auditing User Actions

## Getting Started

### Prerequisites

- [.NET 6 or later](https://dotnet.microsoft.com/download)
- [MSSQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/Kenisank/TodoApps.git
   cd FullApp_TodoApp/Backend



#### 2. **Testing**

Testing ensures your application behaves as expected. Here’s how to set up unit tests and integration tests for your project.

##### 2.1 **Unit Tests**

Create a test project using xUnit:

```bash
dotnet new xunit -n Backend.Tests



