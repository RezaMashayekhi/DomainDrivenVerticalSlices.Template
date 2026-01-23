import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import PropTypes from "prop-types";
import Button from "../../common/Button";
import CancelModal from "../../modals/CancelModal";
import SaveModal from "../../modals/SaveModal";
import ErrorModal from "../../modals/ErrorModal";
import {
    getEntity1ById,
    updateEntity1,
    createEntity1,
} from "../../../services/Entity1Service";

const Entity1Form = ({ isEdit = false }) => {
    const [entity, setEntity] = useState({ valueObject1: { property1: "" } });
    const [isCancelModalOpen, setIsCancelModalOpen] = useState(false);
    const [isSaveModalOpen, setIsSaveModalOpen] = useState(false);
    const [isErrorModalOpen, setIsErrorModalOpen] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const { id } = useParams();
    const navigate = useNavigate();

    useEffect(() => {
        if (isEdit && id) {
            setIsLoading(true);
            getEntity1ById(id)
                .then((data) => {
                    if (data) {
                        setEntity(data);
                    } else {
                        setErrorMessage(
                            "Unable to load entity data. Please try again."
                        );
                        setIsErrorModalOpen(true);
                    }
                })
                .catch(() => {
                    setErrorMessage(
                        "Unable to load entity data. Please try again."
                    );
                    setIsErrorModalOpen(true);
                })
                .finally(() => setIsLoading(false));
        }
    }, [id, isEdit]);

    const handleChange = (e) => {
        setEntity({
            ...entity,
            valueObject1: {
                ...entity.valueObject1,
                [e.target.name]: e.target.value,
            },
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setIsLoading(true);

        try {
            const result = isEdit
                ? await updateEntity1(entity)
                : await createEntity1(entity);

            if (result) {
                setIsSaveModalOpen(true);
            } else {
                setErrorMessage("Failed to save changes. Please try again.");
                setIsErrorModalOpen(true);
            }
        } catch {
            setErrorMessage("An error occurred. Please try again.");
            setIsErrorModalOpen(true);
        } finally {
            setIsLoading(false);
        }
    };

    const handleCancel = () => {
        setIsCancelModalOpen(true);
    };

    const handleConfirmCancel = () => {
        setIsCancelModalOpen(false);
        navigate("/");
    };

    const handleConfirmSave = () => {
        setIsSaveModalOpen(false);
        navigate("/");
    };

    return (
        <div className="mx-auto max-w-lg rounded-lg bg-[var(--color-form-background-light)] p-6 shadow-lg dark:bg-[var(--color-form-background-dark)]">
            <h2 className="mb-6 text-xl font-bold text-gray-800 dark:text-gray-200">
                {isEdit ? "Edit Entity1" : "Add New Entity1"}
            </h2>

            <form onSubmit={handleSubmit} data-testid="entity-form">
                <div className="mb-5">
                    <label
                        htmlFor="property1"
                        className="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
                    >
                        Property1
                    </label>
                    <input
                        type="text"
                        id="property1"
                        name="property1"
                        data-testid="property1-input"
                        value={entity.valueObject1.property1}
                        onChange={handleChange}
                        className="block w-full rounded-lg border border-gray-300 bg-[var(--color-input-background-light)] p-2.5 text-sm text-gray-900 shadow-sm focus:border-[var(--color-primary-light)] focus:ring-[var(--color-primary-light)] dark:border-gray-600 dark:bg-[var(--color-input-background-dark)] dark:text-white dark:placeholder-gray-400 dark:focus:border-[var(--color-primary-dark)] dark:focus:ring-[var(--color-primary-dark)]"
                        placeholder="Enter Property1"
                        required
                        disabled={isLoading}
                    />
                </div>

                <div className="flex justify-end space-x-3">
                    <Button
                        variant="outlined"
                        size="sm"
                        onClick={handleCancel}
                        disabled={isLoading}
                    >
                        Cancel
                    </Button>
                    <Button
                        variant="filled"
                        size="sm"
                        type="submit"
                        disabled={isLoading}
                    >
                        {isLoading ? "Saving..." : isEdit ? "Update" : "Add"}
                    </Button>
                </div>
            </form>

            <CancelModal
                isOpen={isCancelModalOpen}
                onConfirm={handleConfirmCancel}
                onClose={() => setIsCancelModalOpen(false)}
            />
            <SaveModal
                isOpen={isSaveModalOpen}
                onConfirm={handleConfirmSave}
                onClose={() => setIsSaveModalOpen(false)}
                title={isEdit ? "Updated Successfully" : "Created Successfully"}
                message={
                    isEdit
                        ? "Entity has been updated successfully."
                        : "New entity has been created successfully."
                }
            />
            <ErrorModal
                isOpen={isErrorModalOpen}
                onClose={() => setIsErrorModalOpen(false)}
                message={errorMessage}
            />
        </div>
    );
};

Entity1Form.propTypes = {
    isEdit: PropTypes.bool,
};

export default Entity1Form;
