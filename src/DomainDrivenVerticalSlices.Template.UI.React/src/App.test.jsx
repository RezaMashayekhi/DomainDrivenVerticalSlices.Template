import { render, screen } from "@testing-library/react";
import App from "./App";

describe("App Component Tests", () => {
    test("renders the navigation with Home link", () => {
        render(<App />);
        const homeLink = screen.getByRole("link", { name: /home/i });
        expect(homeLink).toBeInTheDocument();
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

    test("renders the dark mode toggle", () => {
        render(<App />);
        // Toggle should be accessible
        const toggleButton = screen.getByRole("button", {
            name: /switch to (dark|light) mode/i,
        });
        expect(toggleButton).toBeInTheDocument();
    });
});
