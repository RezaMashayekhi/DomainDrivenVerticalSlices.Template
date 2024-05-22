import React from "react";
import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import Entity1List from "./Entity1List";
import { useNavigate } from "react-router-dom";
import * as Entity1Service from "../../../services/Entity1Service";

jest.mock("react-router-dom", () => ({
    useNavigate: jest.fn(),
}));

jest.mock("../../../services/Entity1Service", () => ({
    getAllEntity1: jest.fn(),
    searchEntity1ByProperty1: jest.fn(),
    deleteEntity1ById: jest.fn(),
}));

describe("Entity1List Component Tests", () => {
    const mockNavigate = jest.fn();

    beforeEach(() => {
        mockNavigate.mockReset();
        useNavigate.mockReturnValue(mockNavigate);
        Entity1Service.getAllEntity1.mockResolvedValue([]);
        Entity1Service.searchEntity1ByProperty1.mockResolvedValue([]);
        Entity1Service.deleteEntity1ById.mockResolvedValue(true);
    });

    test("initially calls getAllEntity1", async () => {
        render(<Entity1List />);
        await waitFor(() => expect(Entity1Service.getAllEntity1).toHaveBeenCalled());
    });

    test("searches entities when search query is changed", async () => {
        render(<Entity1List />);
        const searchBar = screen.getByPlaceholderText("Search by Property1...");
        fireEvent.change(searchBar, { target: { value: 'test' } });
        await waitFor(() => {
            expect(Entity1Service.searchEntity1ByProperty1).toHaveBeenCalledWith('test');
        });
    });

    test("navigates to add entity page on add button click", () => {
        render(<Entity1List />);
        fireEvent.click(screen.getByLabelText("Add Entity1"));
        expect(mockNavigate).toHaveBeenCalledWith("/add-entity1");
    });

    test("handles pagination correctly", async () => {
        Entity1Service.getAllEntity1.mockResolvedValue(
            Array.from({ length: 10 }, (_, index) => ({
                id: `unique_id_${index}`,
                valueObject1: { property1: "Property" }
            }))
        );
        render(<Entity1List />);
        await waitFor(() => expect(Entity1Service.getAllEntity1).toHaveBeenCalled());
        await waitFor(() => {
            expect(screen.getAllByRole("button", { name: /^\d+$/ })).toHaveLength(4);
        });

        const pageTwoButton = screen.getByRole('button', { name: '2' });
        fireEvent.click(pageTwoButton);

        await waitFor(() => {
            const displayedItems = screen.getAllByText("Property");
            expect(displayedItems.length).toBeLessThanOrEqual(3);
        });
    });


    test("handles entity deletion correctly", async () => {
        const mockEntity = { id: "unique_id", valueObject1: { property1: "Property" } };
        Entity1Service.getAllEntity1.mockResolvedValue([mockEntity]);
        render(<Entity1List />);    
        await waitFor(() => expect(Entity1Service.getAllEntity1).toHaveBeenCalled());
        const deleteButton = screen.getByRole("button", { name: "Delete" });
        window.confirm = jest.fn().mockReturnValueOnce(true);
        fireEvent.click(deleteButton);
        await waitFor(() => expect(Entity1Service.deleteEntity1ById).toHaveBeenCalledWith(mockEntity.id));
        expect(Entity1Service.getAllEntity1).toHaveBeenCalledTimes(2);
    });
    
    test("navigates to edit page on edit button click", async () => {
        const mockEntity = { id: "unique_id", valueObject1: { property1: "Property" } };
        Entity1Service.getAllEntity1.mockResolvedValue([mockEntity]);
        render(<Entity1List />);
        await waitFor(() => expect(Entity1Service.getAllEntity1).toHaveBeenCalled());
        const editButton = screen.getByRole("button", { name: "Edit" });
        fireEvent.click(editButton);
        expect(mockNavigate).toHaveBeenCalledWith(`/edit-entity1/${mockEntity.id}`);
    });
});
