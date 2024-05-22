import React from 'react';
import PropTypes from 'prop-types';
import { useNavigate } from 'react-router-dom';
import './Entity1ListItem.css';

const Entity1ListItem = ({ entity, onDelete }) => {
    const navigate = useNavigate();

    const handleEdit = () => {
        navigate(`/edit-entity1/${entity.id}`);
    };

    return (
        <div className="entity">
            <span className="entity-valueObject1-property1">
                {entity.valueObject1.property1}
            </span>
            <div className="icon-container">
                <button className="icon edit-icon" onClick={handleEdit} aria-label="Edit">
                    ✏️
                </button>
                <button
                    className="icon delete-icon"
                    onClick={() => onDelete(entity.id)}
                    aria-label="Delete"
                >
                    🗑️
                </button>
            </div>
        </div>
    );
};

Entity1ListItem.propTypes = {
    entity: PropTypes.shape({
        id: PropTypes.string.isRequired,
        valueObject1: PropTypes.shape({
            property1: PropTypes.string
        }).isRequired
    }).isRequired,
    onDelete: PropTypes.func.isRequired
};

export default Entity1ListItem;
