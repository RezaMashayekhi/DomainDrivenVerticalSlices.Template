const DEFAULT_API_BASE_URL = "/api/entity1";
const JSON_HEADERS = {
    "Content-Type": "application/json",
};

const normalizeBaseUrl = (value) =>
    value.endsWith("/") ? value.slice(0, -1) : value;

const getApiBaseUrl = () => {
    const candidate =
        typeof import.meta.env.VITE_API_BASE_URL === "string" &&
        import.meta.env.VITE_API_BASE_URL.trim().length > 0
            ? import.meta.env.VITE_API_BASE_URL.trim()
            : DEFAULT_API_BASE_URL;

    return normalizeBaseUrl(candidate);
};

const buildUrl = (path = "") => `${getApiBaseUrl()}${path}`;

const extractErrorMessage = async (response) => {
    if (typeof response.json === "function") {
        try {
            const payload = await response.json();
            if (typeof payload === "string") {
                return payload;
            }

            if (payload?.message) {
                return payload.message;
            }

            if (payload) {
                return JSON.stringify(payload);
            }
        } catch {
            // Ignore JSON parse issues and attempt to read text.
        }
    }

    if (typeof response.text === "function") {
        try {
            const text = await response.text();
            if (text) {
                return text;
            }
        } catch {
            // Swallow text read errors.
        }
    }

    return response.statusText || "Request failed";
};

const request = async (path, options, { expectResponseBody = true } = {}) => {
    const url = buildUrl(path);
    const response =
        options !== undefined && options !== null
            ? await fetch(url, options)
            : await fetch(url);

    if (!response.ok) {
        throw new Error(await extractErrorMessage(response));
    }

    if (!expectResponseBody) {
        return true;
    }

    if (response.status === 204) {
        return null;
    }

    const isJsonResponse =
        response.headers?.get?.("content-type")?.includes("application/json") ??
        typeof response.json === "function";

    if (!isJsonResponse) {
        return null;
    }

    return await response.json();
};

const safeRequest = async (
    path,
    { options, fallbackValue, logMessage, expectResponseBody = true } = {},
) => {
    try {
        return await request(path, options, { expectResponseBody });
    } catch (error) {
        console.error("%s %o", logMessage, error);
        return fallbackValue;
    }
};

export const getAllEntity1 = async () =>
    safeRequest("/all", {
        fallbackValue: [],
        logMessage: "Failed to fetch all Entity1",
    });

export const getEntity1ById = async (id) =>
    safeRequest(`/${id}`, {
        fallbackValue: null,
        logMessage: `Failed to fetch Entity1 by ID ${id}`,
    });

export const searchEntity1ByProperty1 = async (property1) =>
    safeRequest(`/list?${new URLSearchParams({ property1 })}`, {
        fallbackValue: [],
        logMessage: "Failed to search Entity1 by property1",
    });

export const updateEntity1 = async (entity) => {
    if (!entity?.id) {
        console.error("Failed to update Entity1: invalid entity payload");
        return false;
    }

    const success = await safeRequest(`/${entity.id}`, {
        options: {
            method: "PUT",
            headers: JSON_HEADERS,
            body: JSON.stringify({
                Entity1: entity,
            }),
        },
        fallbackValue: false,
        logMessage: "Failed to update Entity1",
        expectResponseBody: false,
    });

    return Boolean(success);
};

export const deleteEntity1ById = async (id) =>
    safeRequest(`/${id}`, {
        options: {
            method: "DELETE",
        },
        fallbackValue: false,
        logMessage: "Failed to delete Entity1",
        expectResponseBody: false,
    });

export const createEntity1 = async (entity) =>
    safeRequest("", {
        options: {
            method: "POST",
            headers: JSON_HEADERS,
            body: JSON.stringify({
                property1: entity?.valueObject1?.property1,
            }),
        },
        fallbackValue: null,
        logMessage: "Failed to create Entity1",
    });
