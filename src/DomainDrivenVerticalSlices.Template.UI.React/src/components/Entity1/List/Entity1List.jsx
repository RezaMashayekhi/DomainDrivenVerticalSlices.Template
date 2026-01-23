import { useEffect, useState, useCallback } from "react";
import {
    getAllEntity1,
    searchEntity1ByProperty1,
    deleteEntity1ById,
} from "../../../services/Entity1Service";
import { useNavigate } from "react-router-dom";
import Entity1ListItem from "../ListItem/Entity1ListItem";
import {
    PlusCircleIcon,
    MagnifyingGlassIcon,
} from "@heroicons/react/24/outline";
import DeleteModal from "../../modals/DeleteModal";
import ErrorModal from "../../modals/ErrorModal";
import Button from "../../common/Button";

const Entity1List = () => {
    const [entity1List, setEntity1List] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [searchQuery, setSearchQuery] = useState("");
    const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
    const [selectedEntityId, setSelectedEntityId] = useState(null);
    const [isErrorModalOpen, setIsErrorModalOpen] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");
    const navigate = useNavigate();
    const itemsPerPage = 5;

    const fetchEntities = useCallback(async () => {
        try {
            const data = searchQuery.trim()
                ? await searchEntity1ByProperty1(searchQuery)
                : await getAllEntity1();
            setEntity1List(data);
        } catch (error) {
            setErrorMessage("Failed to fetch entities. Please try again.");
            setIsErrorModalOpen(true);
        }
    }, [searchQuery]);

    useEffect(() => {
        const delayDebounce = setTimeout(() => {
            fetchEntities();
        }, 300);
        return () => clearTimeout(delayDebounce);
    }, [searchQuery, fetchEntities]);

    useEffect(() => {
        setCurrentPage(1);
    }, [searchQuery]);

    const handleDeleteClick = (id) => {
        setSelectedEntityId(id);
        setIsDeleteModalOpen(true);
    };

    const confirmDelete = async () => {
        try {
            const success = await deleteEntity1ById(selectedEntityId);
            setIsDeleteModalOpen(false);
            if (success) {
                fetchEntities();
            } else {
                setErrorMessage("Failed to delete entity. Please try again.");
                setIsErrorModalOpen(true);
            }
        } catch (error) {
            setIsDeleteModalOpen(false);
            setErrorMessage("Failed to delete entity. Please try again.");
            setIsErrorModalOpen(true);
        }
    };

    const handleEditClick = (id) => {
        navigate(`/edit-entity1/${id}`);
    };

    const handleAddEntity = () => {
        navigate("/add-entity1");
    };

    const lastIndex = currentPage * itemsPerPage;
    const firstIndex = lastIndex - itemsPerPage;
    const currentItems = entity1List.slice(firstIndex, lastIndex);
    const totalPages = Math.ceil(entity1List.length / itemsPerPage);

    return (
        <div className="container mx-auto">
            <div className="rounded-lg bg-[var(--color-form-background-light)] p-6 shadow-lg dark:bg-[var(--color-form-background-dark)]">
                <div className="mb-6 flex items-center justify-between">
                    <h1 className="text-2xl font-bold text-gray-800 dark:text-gray-200">
                        List of Entity1
                    </h1>
                    <Button
                        onClick={handleAddEntity}
                        aria-label="Add Entity1"
                        className="flex items-center gap-2"
                    >
                        <PlusCircleIcon className="h-5 w-5" />
                        Add New
                    </Button>
                </div>

                <div className="relative mb-4">
                    <MagnifyingGlassIcon className="absolute left-3 top-1/2 h-5 w-5 -translate-y-1/2 text-gray-500 dark:text-gray-400" />
                    <input
                        type="text"
                        className="w-full rounded-lg border border-gray-300 bg-[var(--color-input-background-light)] py-3 pl-10 pr-4 text-gray-800 placeholder-gray-500 focus:border-[var(--color-primary-light)] focus:outline-none focus:ring-2 focus:ring-[var(--color-primary-light)] dark:border-gray-600 dark:bg-[var(--color-input-background-dark)] dark:text-gray-200 dark:placeholder-gray-400"
                        placeholder="Search by Property1..."
                        value={searchQuery}
                        onChange={(e) => setSearchQuery(e.target.value)}
                    />
                </div>

                <div className="space-y-3">
                    {currentItems.length === 0 ? (
                        <p className="py-8 text-center text-gray-500 dark:text-gray-400">
                            No entities found.
                        </p>
                    ) : (
                        currentItems.map((entity) => (
                            <Entity1ListItem
                                key={entity.id}
                                entity={entity}
                                onDelete={() => handleDeleteClick(entity.id)}
                                onEdit={() => handleEditClick(entity.id)}
                            />
                        ))
                    )}
                </div>

                {totalPages > 1 && (
                    <div className="mt-6 flex items-center justify-center gap-2">
                        <Button
                            variant="outlined"
                            size="sm"
                            onClick={() =>
                                setCurrentPage((p) => Math.max(1, p - 1))
                            }
                            disabled={currentPage === 1}
                        >
                            Previous
                        </Button>
                        {Array.from({ length: totalPages }, (_, index) => (
                            <button
                                key={index + 1}
                                onClick={() => setCurrentPage(index + 1)}
                                className={`h-8 w-8 rounded-lg transition-colors ${
                                    currentPage === index + 1
                                        ? "bg-[var(--color-primary-light)] text-white dark:bg-[var(--color-primary-dark)]"
                                        : "text-gray-700 hover:bg-gray-200 dark:text-gray-300 dark:hover:bg-gray-700"
                                }`}
                            >
                                {index + 1}
                            </button>
                        ))}
                        <Button
                            variant="outlined"
                            size="sm"
                            onClick={() =>
                                setCurrentPage((p) =>
                                    Math.min(totalPages, p + 1)
                                )
                            }
                            disabled={currentPage === totalPages}
                        >
                            Next
                        </Button>
                    </div>
                )}
            </div>

            <DeleteModal
                isOpen={isDeleteModalOpen}
                onConfirm={confirmDelete}
                onCancel={() => setIsDeleteModalOpen(false)}
            />
            <ErrorModal
                isOpen={isErrorModalOpen}
                onClose={() => setIsErrorModalOpen(false)}
                message={errorMessage}
            />
        </div>
    );
};

export default Entity1List;
