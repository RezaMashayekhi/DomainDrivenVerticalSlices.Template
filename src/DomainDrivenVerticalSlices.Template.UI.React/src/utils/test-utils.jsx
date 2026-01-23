import { render } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";
import { ThemeProvider } from "../context/ThemeContext";
import PropTypes from "prop-types";

/**
 * Custom render function that wraps components with necessary providers.
 * Use this instead of @testing-library/react's render for component tests.
 *
 * @param {React.ReactElement} ui - The component to render
 * @param {Object} options - Render options
 * @param {string} options.route - Initial route for MemoryRouter (default: "/")
 * @param {Object} options.renderOptions - Additional options to pass to render
 * @returns {Object} Render result with additional utilities
 */
export function renderWithProviders(
    ui,
    { route = "/", ...renderOptions } = {}
) {
    function Wrapper({ children }) {
        return (
            <ThemeProvider>
                <MemoryRouter initialEntries={[route]}>{children}</MemoryRouter>
            </ThemeProvider>
        );
    }

    Wrapper.propTypes = {
        children: PropTypes.node.isRequired,
    };

    return render(ui, { wrapper: Wrapper, ...renderOptions });
}

// Re-export everything from testing-library
export * from "@testing-library/react";
export { default as userEvent } from "@testing-library/user-event";
