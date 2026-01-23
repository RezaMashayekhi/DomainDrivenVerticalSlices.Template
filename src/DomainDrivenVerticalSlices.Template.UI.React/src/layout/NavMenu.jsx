import { Link } from "react-router-dom";
import { useTheme } from "../context/ThemeContext";
import { SunIcon, MoonIcon } from "@heroicons/react/24/outline";

/**
 * Navigation menu component with dark mode toggle.
 */
const NavMenu = () => {
    const { isDark, toggle } = useTheme();

    return (
        <nav className="bg-[var(--color-primary-light)] p-4 shadow dark:bg-[var(--color-primary-dark)]">
            <div className="container mx-auto flex items-center justify-between">
                <Link
                    to="/"
                    className="text-lg font-semibold text-gray-100 hover:text-gray-200 dark:text-gray-300"
                >
                    Home
                </Link>
                <div className="flex items-center space-x-4">
                    <Link
                        to="/add-entity1"
                        className="text-lg font-semibold text-gray-100 hover:text-gray-200 dark:text-gray-300"
                    >
                        Add Entity1
                    </Link>
                    <button
                        onClick={toggle}
                        title="Toggle light/dark theme"
                        aria-label={
                            isDark
                                ? "Switch to light mode"
                                : "Switch to dark mode"
                        }
                        className="flex h-8 w-14 cursor-pointer items-center rounded-full bg-gray-700 p-1 transition-colors duration-200 dark:bg-[var(--color-background-darker-dark)]"
                    >
                        <div
                            className={`flex h-6 w-6 transform items-center justify-center rounded-full bg-[var(--color-form-background-light)] shadow-md transition-transform duration-200 dark:bg-gray-700 ${
                                isDark ? "translate-x-6" : ""
                            }`}
                        >
                            {isDark ? (
                                <MoonIcon className="h-4 w-4 text-gray-300" />
                            ) : (
                                <SunIcon className="h-4 w-4 text-yellow-500" />
                            )}
                        </div>
                    </button>
                </div>
            </div>
        </nav>
    );
};

export default NavMenu;
