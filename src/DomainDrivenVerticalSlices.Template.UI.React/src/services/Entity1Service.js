const API_BASE_URL = process.env.REACT_APP_API_BASE_URL;

export const getAllEntity1 = async () => {
    try {
        const response = await fetch(`${API_BASE_URL}/all`);
        if (!response.ok) {
            throw new Error("Network response was not ok");
        }
        return await response.json();
    } catch (error) {
        console.error("Failed to fetch all Entity1: ", error);
        return [];
    }
};

export const getEntity1ById = async (id) => {
    try {
        const response = await fetch(`${API_BASE_URL}/${id}`);
        if (!response.ok) {
            throw new Error("Network response was not ok");
        }
        return await response.json();
    } catch (error) {
        console.error(`Failed to fetch Entity1 by ID ${id}: `, error);
        return null;
    }
};

export const searchEntity1ByProperty1 = async (property1) => {
    try {
        const response = await fetch(
            `${API_BASE_URL}/list?property1=${encodeURIComponent(property1)}`
        );
        if (!response.ok) {
            throw new Error("Network response was not ok");
        }
        return await response.json();
    } catch (error) {
        console.error("Failed to search Entity1 by property1: ", error);
        return [];
    }
};

export const updateEntity1 = async (entity) => {
    try {
        const response = await fetch(`${API_BASE_URL}/${entity.id}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                Entity1: entity,
            }),
        });
        if (!response.ok) {
            const errorResponse = await response.json();
            throw new Error(
                `Failed to update Entity1: ${JSON.stringify(errorResponse)}`
            );
        }
        return true;
    } catch (error) {
        console.error("Failed to update Entity1: ", error);
        return false;
    }
};

export const deleteEntity1ById = async (id) => {
    try {
        const response = await fetch(`${API_BASE_URL}/${id}`, {
            method: "DELETE",
        });
        if (!response.ok) {
            throw new Error("Failed to delete Entity1");
        }
        return true;
    } catch (error) {
        console.error("Failed to delete Entity1: ", error);
        return false;
    }
};

export const createEntity1 = async (entity) => {
    try {
        const response = await fetch(`${API_BASE_URL}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                property1: entity.valueObject1.property1,
            }),
        });
        if (!response.ok) {
            const errorResponse = await response.json();
            throw new Error(
                `Failed to create Entity1: ${JSON.stringify(errorResponse)}`
            );
        }
        return await response.json();
    } catch (error) {
        console.error("Failed to create Entity1: ", error);
        return null;
    }
};
