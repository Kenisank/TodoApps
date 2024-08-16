import React from "react";
import { render, screen, waitFor } from "@testing-library/react";
import Todo from "../pages/Todo";
import todoService from "../services/todo.service";
import { AuthProvider } from "../contexts/auth.context";

// Mock the todoService
jest.mock("../services/todo.service");

describe("Todo Component", () => {
  test("renders Todo list with fetched todos", async () => {
    // Mock API response
    todoService.getTodos = jest.fn().mockResolvedValue([
      { id: 1, title: "Demo Todo Item", isCompleted: false },
    ]);

    render(
      <AuthProvider>
        <Todo />
      </AuthProvider>
    );

    await waitFor(() => {
      expect(screen.getByText("Demo Todo Item")).toBeInTheDocument();
    });
  });

  test("displays error when fetching todos fails", async () => {
    // Mock API failure
    todoService.getTodos = jest.fn().mockRejectedValue(new Error("Failed to fetch todos"));

    render(
      <AuthProvider>
        <Todo />
      </AuthProvider>
    );

    await waitFor(() => {
      expect(screen.getByText("Failed to fetch todos. Please try again later.")).toBeInTheDocument();
    });
  });
});
