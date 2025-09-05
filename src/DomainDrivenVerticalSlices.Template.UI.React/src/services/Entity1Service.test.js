import { vi } from "vitest";
import * as Entity1Service from "./Entity1Service";

describe("Entity1Service", () => {
    beforeEach(() => {
        global.fetch = vi.fn();
        import.meta.env.VITE_API_BASE_URL = "/api/entity1";
    });

    test("getAllEntity1 returns data on success", async () => {
        global.fetch.mockResolvedValue({
            ok: true,
            json: () =>
                Promise.resolve([
                    { id: 1, valueObject1: { property1: "value" } },
                ]),
        });
        const entities = await Entity1Service.getAllEntity1();
        expect(entities).toEqual([
            { id: 1, valueObject1: { property1: "value" } },
        ]);
        expect(global.fetch).toHaveBeenCalledWith(
            `${import.meta.env.VITE_API_BASE_URL}/all`
        );
    });

    test("getAllEntity1 handles network error gracefully", async () => {
        global.fetch.mockResolvedValue({
            ok: false,
            statusText: "Network response was not ok",
        });
        const entities = await Entity1Service.getAllEntity1();
        expect(entities).toEqual([]);
    });

    test("getEntity1ById returns data on success", async () => {
        const mockId = 1;
        const mockEntity = { id: 1, valueObject1: { property1: "value" } };
        global.fetch.mockResolvedValue({
            ok: true,
            json: () => Promise.resolve(mockEntity),
        });
        const entity = await Entity1Service.getEntity1ById(mockId);
        expect(entity).toEqual(mockEntity);
        expect(global.fetch).toHaveBeenCalledWith(
            `${import.meta.env.VITE_API_BASE_URL}/${mockId}`
        );
    });

    test("getEntity1ById handles network error gracefully", async () => {
        const mockId = 1;
        global.fetch.mockResolvedValue({
            ok: false,
            statusText: "Network response was not ok",
        });
        const entity = await Entity1Service.getEntity1ById(mockId);
        expect(entity).toBeNull();
    });

    test("searchEntity1ByProperty1 returns data on success", async () => {
        const mockProperty1 = "value";
        const mockData = [
            { id: 1, valueObject1: { property1: mockProperty1 } },
        ];
        global.fetch.mockResolvedValue({
            ok: true,
            json: () => Promise.resolve(mockData),
        });
        const entities = await Entity1Service.searchEntity1ByProperty1(
            mockProperty1
        );
        expect(entities).toEqual(mockData);
        expect(global.fetch).toHaveBeenCalledWith(
            `${
                import.meta.env.VITE_API_BASE_URL
            }/list?property1=${encodeURIComponent(mockProperty1)}`
        );
    });

    test("searchEntity1ByProperty1 handles network error gracefully", async () => {
        const mockProperty1 = "value";
        global.fetch.mockResolvedValue({
            ok: false,
            statusText: "Network response was not ok",
        });
        const entities = await Entity1Service.searchEntity1ByProperty1(
            mockProperty1
        );
        expect(entities).toEqual([]);
    });

    test("updateEntity1 successfully updates an entity", async () => {
        const mockEntity = { id: 1, valueObject1: { property1: "Test" } };
        global.fetch.mockResolvedValue({
            ok: true,
            json: () => Promise.resolve(true),
        });
        const response = await Entity1Service.updateEntity1(mockEntity);
        expect(response).toBeTruthy();
        expect(global.fetch).toHaveBeenCalledWith(
            `${import.meta.env.VITE_API_BASE_URL}/${mockEntity.id}`,
            expect.objectContaining({
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    Entity1: mockEntity,
                }),
            })
        );
    });

    test("updateEntity1 handles failure correctly", async () => {
        const mockEntity = { id: 1, valueObject1: { property1: "Test" } };
        global.fetch.mockResolvedValue({
            ok: false,
            json: () => Promise.resolve({ message: "Error updating entity" }),
        });
        const response = await Entity1Service.updateEntity1(mockEntity);
        expect(response).toBeFalsy();
    });

    test("deleteEntity1ById successfully deletes an entity", async () => {
        const mockId = 1;
        global.fetch.mockResolvedValue({
            ok: true,
        });
        const response = await Entity1Service.deleteEntity1ById(mockId);
        expect(response).toBeTruthy();
        expect(global.fetch).toHaveBeenCalledWith(
            `${import.meta.env.VITE_API_BASE_URL}/${mockId}`,
            expect.objectContaining({
                method: "DELETE",
            })
        );
    });

    test("deleteEntity1ById handles failure correctly", async () => {
        const mockId = 1;
        global.fetch.mockResolvedValue({
            ok: false,
            statusText: "Error deleting entity",
        });
        const response = await Entity1Service.deleteEntity1ById(mockId);
        expect(response).toBeFalsy();
    });

    test("createEntity1 successfully creates an entity", async () => {
        const mockEntity = { valueObject1: { property1: "Test" } };

        global.fetch.mockResolvedValue({
            ok: true,
            json: () => Promise.resolve({ id: "123", ...mockEntity }),
        });

        const response = await Entity1Service.createEntity1(mockEntity);

        expect(response).toEqual({ id: "123", ...mockEntity });

        expect(global.fetch).toHaveBeenCalledWith(
            `${import.meta.env.VITE_API_BASE_URL}`,
            expect.objectContaining({
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    property1: mockEntity.valueObject1.property1,
                }),
            })
        );
    });

    test("createEntity1 handles failure correctly", async () => {
        const mockEntity = { valueObject1: { property1: "Test" } };

        global.fetch.mockResolvedValue({
            ok: false,
            json: () => Promise.resolve({ message: "Error creating entity" }),
        });

        const response = await Entity1Service.createEntity1(mockEntity);

        expect(response).toBeNull();
    });
});
