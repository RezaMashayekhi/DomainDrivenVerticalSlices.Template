import React, { useEffect, useState, useCallback } from "react";
import {
    getAllEntity1,
    searchEntity1ByProperty1,
    deleteEntity1ById,
} from "../../../services/Entity1Service";
import { useNavigate } from "react-router-dom";
import Entity1ListItem from "../ListItem/Entity1ListItem";
import CirclePlusIcon from "../../CirclePlusIcon/CirclePlusIcon";
import "./Entity1List.css";

const Entity1List = () => {
    const [entity1List, setEntity1List] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [searchQuery, setSearchQuery] = useState("");
    const navigate = useNavigate();
    const itemsPerPage = 3;

    const fetchEntities = useCallback(async () => {
        const data = searchQuery.trim()
            ? await searchEntity1ByProperty1(searchQuery)
            : await getAllEntity1();
        setEntity1List(data);
    }, [searchQuery]);

    useEffect(() => {
        const delayDebounce = setTimeout(() => {
            fetchEntities();
        }, 300);
        return () => clearTimeout(delayDebounce);
    }, [searchQuery, fetchEntities]);

    const handleDeleteEntity1 = async (id) => {
        if (window.confirm("Are you sure you want to delete this entity?")) {
            const success = await deleteEntity1ById(id);
            if (success) {
                fetchEntities();
            }
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

    const handlePageClick = (pageNumber) => {
        setCurrentPage(pageNumber);
    };

    return (
        <section className="section-container">
            <h1 className="list-header">List of Entity1</h1>
            <input
                type="text"
                className="search-bar"
                placeholder="Search by Property1..."
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
            />
            <div className="list-container">
                {currentItems.map((entity) => (
                    <Entity1ListItem
                        key={entity.id}
                        entity={entity}
                        onDelete={handleDeleteEntity1}
                        onEdit={() => handleEditClick(entity.id)}
                    />
                ))}
                <button
                    className="add-icon"
                    onClick={handleAddEntity}
                    aria-label="Add Entity1"
                >
                    <CirclePlusIcon />
                </button>

            </div>
            {totalPages > 1 && (
                <div className="pagination">
                    {Array.from({ length: totalPages }, (_, index) => (
                        <button
                            key={index + 1}
                            onClick={() => handlePageClick(index + 1)}
                            className={
                                currentPage === index + 1 ? "current-page" : ""
                            }
                        >
                            {index + 1}
                        </button>
                    ))}
                </div>
            )}
        </section>
    );
};

export default Entity1List;
