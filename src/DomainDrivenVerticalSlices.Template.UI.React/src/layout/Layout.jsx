import PropTypes from "prop-types";
import NavMenu from "./NavMenu";
import Footer from "./Footer";

/**
 * Main layout wrapper component that provides consistent structure
 * across all pages with navigation and footer.
 */
const Layout = ({ children }) => (
    <div className="flex min-h-screen flex-col">
        <NavMenu />
        <main className="flex flex-grow items-center justify-center bg-gradient-to-r from-[var(--color-background-darker-light)] to-[var(--color-background-light)] p-6 dark:from-[var(--color-background-dark)] dark:to-[var(--color-background-darker-dark)]">
            <div className="container mx-auto">{children}</div>
        </main>
        <Footer />
    </div>
);

Layout.propTypes = {
    children: PropTypes.node.isRequired,
};

export default Layout;
