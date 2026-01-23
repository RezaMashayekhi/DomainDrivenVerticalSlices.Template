import { Link } from "react-router-dom";

/**
 * 404 Page Not Found component.
 */
const PageNotFound = () => (
    <div className="flex min-h-[60vh] flex-col items-center justify-center text-center">
        <h1 className="mb-4 text-6xl font-bold text-gray-800 dark:text-gray-200">
            404
        </h1>
        <p className="mb-8 text-xl text-gray-600 dark:text-gray-400">
            Oops! The page you&apos;re looking for doesn&apos;t exist.
        </p>
        <Link
            to="/"
            className="rounded-lg bg-[var(--color-primary-light)] px-6 py-3 text-white transition-colors hover:opacity-90 dark:bg-[var(--color-primary-dark)]"
        >
            Go back home
        </Link>
    </div>
);

export default PageNotFound;
