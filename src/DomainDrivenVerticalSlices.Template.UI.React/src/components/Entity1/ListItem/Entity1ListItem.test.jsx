import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import { useNavigate } from "react-router-dom";
import { vi } from "vitest";
import Entity1ListItem from "./Entity1ListItem";

vi.mock("react-router-dom", () => ({
    useNavigate: vi.fn(),
}));

describe("Entity1ListItem Tests", () => {
    const mockNavigate = vi.fn();
    const mockDelete = vi.fn();
    const entity = {
        id: "1",
        valueObject1: {
            property1: "Test Property",
        },
    };

    beforeEach(() => {
        mockNavigate.mockReset();
        mockDelete.mockReset();
        useNavigate.mockReturnValue(mockNavigate);
    });

    test("renders entity details correctly", () => {
        render(<Entity1ListItem entity={entity} onDelete={mockDelete} />);

        expect(screen.getByText("Test Property")).toBeInTheDocument();
        expect(screen.getByLabelText("Edit")).toBeInTheDocument();
        expect(screen.getByLabelText("Delete")).toBeInTheDocument();
    });

    test("navigates to edit page on edit button click", () => {
        render(<Entity1ListItem entity={entity} onDelete={mockDelete} />);
        const editButton = screen.getByLabelText("Edit");
        fireEvent.click(editButton);
        expect(mockNavigate).toHaveBeenCalledWith(`/edit-entity1/${entity.id}`);
    });

    test("calls delete function on delete button click", () => {
        render(<Entity1ListItem entity={entity} onDelete={mockDelete} />);
        const deleteButton = screen.getByLabelText("Delete");
        fireEvent.click(deleteButton);
        expect(mockDelete).toHaveBeenCalledWith(entity.id);
    });
});
