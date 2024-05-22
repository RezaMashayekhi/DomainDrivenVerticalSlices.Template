import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import PropTypes from "prop-types";
import "./Entity1Form.css";
import {
    getEntity1ById,
    updateEntity1,
    createEntity1,
} from "../../../services/Entity1Service";

const EntityForm = ({ isEdit }) => {
    const [entity, setEntity] = useState({ valueObject1: { property1: "" } });
    const { id } = useParams();
    const navigate = useNavigate();

    useEffect(() => {
        if (isEdit) {
            getEntity1ById(id).then(setEntity);
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
        const result = isEdit
            ? await updateEntity1(entity)
            : await createEntity1(entity);
        if (result) {
            navigate("/");
        }
    };

    return (
        <div className="form-container">
            <form onSubmit={handleSubmit} data-testid="entity-form">
                <div className="input-container">
                    <div className="input-label">Property1:</div>
                </div>
                <div className="input-container">
                    <input
                        className="input-field"
                        type="text"
                        name="property1"
                        data-testid="property1-input"
                        value={entity.valueObject1.property1}
                        onChange={handleChange}
                    />
                </div>
                <div className="button-container">
                    <button
                        className="cancel-button"
                        type="button"
                        onClick={() => navigate("/")}
                    >
                        Cancel
                    </button>
                </div>
                <div className="button-container">
                    <button className="submit-button" type="submit">
                        {isEdit ? "Update" : "Add"}
                    </button>
                </div>
            </form>
        </div>
    );
};

EntityForm.propTypes = {
    isEdit: PropTypes.bool.isRequired,
};

export default EntityForm;
