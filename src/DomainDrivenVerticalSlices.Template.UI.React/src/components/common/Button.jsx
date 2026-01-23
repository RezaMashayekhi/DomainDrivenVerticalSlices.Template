import PropTypes from "prop-types";

/**
 * Reusable Button component with variant styling.
 */
const Button = ({
    children,
    variant = "filled",
    size = "md",
    onClick,
    type = "button",
    disabled = false,
    className = "",
    ...props
}) => {
    const baseClasses =
        "inline-flex items-center justify-center font-medium rounded-lg transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed";

    const sizeClasses = {
        sm: "px-3 py-1.5 text-sm",
        md: "px-4 py-2 text-base",
        lg: "px-6 py-3 text-lg",
    };

    const variantClasses = {
        filled: "bg-[var(--color-primary-light)] text-white hover:opacity-90 focus:ring-[var(--color-primary-light)] dark:bg-[var(--color-primary-dark)]",
        outlined:
            "border-2 border-[var(--color-primary-light)] text-[var(--color-primary-light)] hover:bg-[var(--color-primary-light)] hover:text-white focus:ring-[var(--color-primary-light)] dark:border-[var(--color-primary-dark)] dark:text-gray-200 dark:hover:bg-[var(--color-primary-dark)]",
        text: "text-[var(--color-primary-light)] hover:bg-gray-100 focus:ring-[var(--color-primary-light)] dark:text-gray-300 dark:hover:bg-gray-700",
        danger: "bg-red-600 text-white hover:bg-red-500 focus:ring-red-500",
    };

    return (
        <button
            type={type}
            onClick={onClick}
            disabled={disabled}
            className={`${baseClasses} ${sizeClasses[size]} ${variantClasses[variant]} ${className}`}
            {...props}
        >
            {children}
        </button>
    );
};

Button.propTypes = {
    children: PropTypes.node.isRequired,
    variant: PropTypes.oneOf(["filled", "outlined", "text", "danger"]),
    size: PropTypes.oneOf(["sm", "md", "lg"]),
    onClick: PropTypes.func,
    type: PropTypes.oneOf(["button", "submit", "reset"]),
    disabled: PropTypes.bool,
    className: PropTypes.string,
};

export default Button;
