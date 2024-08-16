import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";
import Register from "../pages/Register";
import { AuthProvider } from "../contexts/auth.context";

const mockNavigate = jest.fn();
jest.mock("react-router-dom", () => ({
  ...jest.requireActual("react-router-dom"),
  useNavigate: () => mockNavigate,
}));

describe("Register Component", () => {
  test("renders Register form", () => {
    render(
      <AuthProvider>
        <MemoryRouter>
          <Register />
        </MemoryRouter>
      </AuthProvider>
    );

    expect(screen.getByLabelText(/Username/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Email Address/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Password/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Confirm Password/i)).toBeInTheDocument();
    expect(screen.getByRole("button", { name: /Sign Up/i })).toBeInTheDocument();
  });

  test("shows error when passwords do not match", () => {
    render(
      <AuthProvider>
        <MemoryRouter>
          <Register />
        </MemoryRouter>
      </AuthProvider>
    );

    fireEvent.change(screen.getByLabelText(/Password/i), {
      target: { value: "password123" },
    });
    fireEvent.change(screen.getByLabelText(/Confirm Password/i), {
      target: { value: "differentPassword" },
    });

    fireEvent.click(screen.getByRole("button", { name: /Sign Up/i }));

    expect(screen.getByText("Passwords do not match")).toBeInTheDocument();
  });
});
