import PropTypes from "prop-types";
import { PencilIcon, TrashIcon } from "@heroicons/react/24/outline";

const Entity1ListItem = ({ entity, onDelete, onEdit }) => {
    return (
        <div className="flex items-center justify-between rounded-lg border border-gray-200 bg-white p-4 transition-shadow hover:shadow-md dark:border-gray-700 dark:bg-gray-800">
            <span className="font-medium text-gray-800 dark:text-gray-200">
                {entity.valueObject1.property1}
            </span>
            <div className="flex gap-2">
                <button
                    className="rounded-lg p-2 text-blue-600 transition-colors hover:bg-blue-100 dark:text-blue-400 dark:hover:bg-blue-900/30"
                    onClick={onEdit}
                    aria-label="Edit"
                    title="Edit"
                >
                    <PencilIcon className="h-5 w-5" />
                </button>
                <button
                    className="rounded-lg p-2 text-red-600 transition-colors hover:bg-red-100 dark:text-red-400 dark:hover:bg-red-900/30"
                    onClick={onDelete}
                    aria-label="Delete"
                    title="Delete"
                >
                    <TrashIcon className="h-5 w-5" />
                </button>
            </div>
        </div>
    );
};

Entity1ListItem.propTypes = {
    entity: PropTypes.shape({
        id: PropTypes.string.isRequired,
        valueObject1: PropTypes.shape({
            property1: PropTypes.string,
        }).isRequired,
    }).isRequired,
    onDelete: PropTypes.func.isRequired,
    onEdit: PropTypes.func.isRequired,
};

export default Entity1ListItem;
