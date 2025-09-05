import React from "react";
import { render, screen } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";
import App from "./App";

describe("App Component Tests", () => {
    test("renders the header with the correct title", () => {
        render(<App />);
        const headerElement = screen.getByText("Domain Driven Vertical Slices");
        expect(headerElement).toBeInTheDocument();
    });

    test("renders the main section with routes", () => {
        render(<App />);
        const mainElement = screen.getByRole("main");
        expect(mainElement).toBeInTheDocument();
    });

    test("renders the footer", () => {
        render(<App />);
        const footerElement = screen.getByRole("contentinfo");
        expect(footerElement).toBeInTheDocument();
    });
});
