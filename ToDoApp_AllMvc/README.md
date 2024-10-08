# ASP.NET MVC To-Do Application

This is a multi-user To-Do application built with ASP.NET MVC. The application supports user authentication, ensuring that each user's tasks are private and secure. The project includes logging, auditing, and a well-organized structure to support maintainability and scalability.

## Features

- Multi-user support with authentication
- Users can create, read, update, and delete their own to-do items
- Secure: No user can access another user's to-do list
- Logging and auditing of user activities
- Well-documented code with a focus on best practices

## Technologies Used

- **ASP.NET Core MVC** for the application framework
- **Entity Framework Core** for database interaction
- **Identity Framework** for user authentication and management
- **MSSQL Server** as the database

## Prerequisites

- **Visual Studio 2022** or later
- **.NET SDK 6.0** or later
- **MSSQL Server** (local or remote instance)

## Setup Instructions

1. **Clone the repository**:
    ```bash
    git clone https://github.com/your-username/Kenisank/TodoApps.git
    cd ToDoApp_AllMvc
    ```

2. **Set up the database**:
    - Ensure MSSQL Server is running on your machine.
    - Update the `appsettings.json` or `appsettings.Development.json` file with your database connection string:
      ```json
      "ConnectionStrings": {
          "TodoConnection": "Server=.\\SQLEXPRESS;Database=TodoDb;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"

      }
      ```

3. **Apply migrations to set up the database schema**:
    - Open the Package Manager Console in Visual Studio and run:
      ```bash
      Update-Database
      ```

4. **Run the application**:
    - In Visual Studio, press `F5` to run the application.
    - The application will start on `https://localhost:5001` or another configured port.

## Usage

### Registration and Login

1. Navigate to the registration page.
2. Register a new account with a valid email and password.
3. After registration, log in with your credentials.

### Managing To-Do Items

1. Once logged in, navigate to the To-Do list page.
2. Add new tasks using the "Add Task" form.
3. Edit or delete tasks by clicking on the respective buttons next to each task.

### Logging and Auditing

- All actions such as creating, editing, and deleting tasks are logged.
- Logs are can be viewed in the debugged console and they are also store in databased on Audit Table as configured in `Program.cs`.

### Security

- Only authenticated users can access the To-Do lists.
- Each user's tasks are private and inaccessible to other users.

## Logging

- By default, logs are output to the console and can be viewed in the Visual Studio Output window.

## Troubleshooting

- **Connection issues**: Ensure that the connection string in `appsettings.json` is correct and that SQL Server is running.
- **EF Migrations**: If migrations fail, ensure that the database is accessible and that the connection string is correct.
- **User authentication**: If you have issues logging in or registering, check the Identity setup in `Program.cs` and ensure migrations have been applied correctly.

## Contributing

If you would like to contribute to this project, please fork the repository and submit a pull request.

