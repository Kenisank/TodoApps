import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";
import Login from "../pages/Login";
import { AuthProvider } from "../contexts/auth.context";

// Mocking useNavigate
const mockNavigate = jest.fn();
jest.mock("react-router-dom", () => ({
  ...jest.requireActual("react-router-dom"),
  useNavigate: () => mockNavigate,
}));

describe("Login Component", () => {
  test("renders Login form", () => {
    render(
      <AuthProvider>
        <MemoryRouter>
          <Login />
        </MemoryRouter>
      </AuthProvider>
    );

    // Check if the elements are rendered
    expect(screen.getByLabelText(/Username/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Password/i)).toBeInTheDocument();
    expect(screen.getByRole("button", { name: /Login/i })).toBeInTheDocument();
  });

  test("displays error on failed login", async () => {
    render(
      <AuthProvider>
        <MemoryRouter>
          <Login />
        </MemoryRouter>
      </AuthProvider>
    );

    fireEvent.change(screen.getByLabelText(/Username/i), {
      target: { value: "wrongUsername" },
    });
    fireEvent.change(screen.getByLabelText(/Password/i), {
      target: { value: "wrongPassword" },
    });

    fireEvent.click(screen.getByRole("button", { name: /Login/i }));

    // Assuming there's a delay for the login request
    await screen.findByText("Failed to login. Please check your credentials.");

    expect(
      screen.getByText("Failed to login. Please check your credentials.")
    ).toBeInTheDocument();
  });
});
