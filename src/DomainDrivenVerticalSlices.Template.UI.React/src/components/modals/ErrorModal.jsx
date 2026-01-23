import PropTypes from "prop-types";
import {
    Dialog,
    DialogPanel,
    DialogTitle,
    DialogBackdrop,
} from "@headlessui/react";
import { XCircleIcon } from "@heroicons/react/24/outline";
import Button from "../common/Button";

/**
 * Error modal component for displaying error messages.
 */
const ErrorModal = ({
    isOpen,
    onClose,
    title = "Error",
    message = "An error occurred. Please try again.",
}) => (
    <Dialog open={isOpen} onClose={onClose} className="relative z-50">
        <DialogBackdrop className="fixed inset-0 bg-gray-500/75 transition-opacity" />
        <div className="fixed inset-0 z-10 overflow-y-auto">
            <div className="flex min-h-full items-end justify-center p-4 text-center sm:items-center sm:p-0">
                <DialogPanel className="relative transform overflow-hidden rounded-lg bg-white text-left shadow-xl transition-all sm:my-8 sm:w-full sm:max-w-lg dark:bg-gray-900">
                    <div className="bg-red-50 px-4 pb-4 pt-5 sm:p-6 sm:pb-4 dark:bg-red-900/20">
                        <div className="sm:flex sm:items-start">
                            <div className="mx-auto flex h-12 w-12 shrink-0 items-center justify-center rounded-full bg-red-100 sm:mx-0 sm:h-10 sm:w-10 dark:bg-red-800">
                                <XCircleIcon
                                    aria-hidden="true"
                                    className="h-6 w-6 text-red-600 dark:text-red-400"
                                />
                            </div>
                            <div className="mt-3 text-center sm:ml-4 sm:mt-0 sm:text-left">
                                <DialogTitle
                                    as="h3"
                                    className="text-lg font-medium leading-6 text-gray-900 dark:text-gray-200"
                                >
                                    {title}
                                </DialogTitle>
                                <div className="mt-2">
                                    <p className="text-sm text-gray-500 dark:text-gray-300">
                                        {message}
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="bg-red-100 px-4 py-3 sm:flex sm:flex-row-reverse sm:px-6 dark:bg-red-900/30">
                        <Button
                            variant="danger"
                            size="sm"
                            onClick={onClose}
                            className="w-full sm:w-auto"
                        >
                            Close
                        </Button>
                    </div>
                </DialogPanel>
            </div>
        </div>
    </Dialog>
);

ErrorModal.propTypes = {
    isOpen: PropTypes.bool.isRequired,
    onClose: PropTypes.func.isRequired,
    title: PropTypes.string,
    message: PropTypes.string,
};

export default ErrorModal;
