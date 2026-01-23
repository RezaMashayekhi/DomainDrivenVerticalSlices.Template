/**
 * Footer component displaying the current year.
 */
const Footer = () => {
    const currentYear = new Date().getFullYear();

    return (
        <footer className="bg-[var(--color-primary-light)] p-4 text-center text-gray-100 dark:bg-[var(--color-primary-dark)] dark:text-gray-300">
            <p>
                © {currentYear} Domain Driven Vertical Slices Template. MIT
                Licensed.
            </p>
        </footer>
    );
};

export default Footer;
