import { createContext, useContext, useState, useEffect, useMemo } from "react";
import PropTypes from "prop-types";

const ThemeContext = createContext(undefined);

/**
 * ThemeProvider component that manages dark mode state globally.
 * Uses localStorage for persistence and respects system preferences.
 */
export function ThemeProvider({ children }) {
    const [isDark, setIsDark] = useState(() => {
        // Check localStorage first
        if (typeof window !== "undefined") {
            const stored = localStorage.getItem("theme");
            if (stored) {
                return stored === "dark";
            }
            // Fall back to system preference
            return window.matchMedia("(prefers-color-scheme: dark)").matches;
        }
        return false;
    });

    useEffect(() => {
        // Update document class and localStorage when theme changes
        document.documentElement.classList.toggle("dark", isDark);
        localStorage.setItem("theme", isDark ? "dark" : "light");
    }, [isDark]);

    const value = useMemo(
        () => ({
            isDark,
            toggle: () => setIsDark((prev) => !prev),
            setDark: (dark) => setIsDark(dark),
        }),
        [isDark]
    );

    return (
        <ThemeContext.Provider value={value}>{children}</ThemeContext.Provider>
    );
}

ThemeProvider.propTypes = {
    children: PropTypes.node.isRequired,
};

/**
 * Hook to access theme context.
 * @returns {{ isDark: boolean, toggle: () => void, setDark: (dark: boolean) => void }}
 */
export function useTheme() {
    const context = useContext(ThemeContext);
    if (context === undefined) {
        throw new Error("useTheme must be used within a ThemeProvider");
    }
    return context;
}
