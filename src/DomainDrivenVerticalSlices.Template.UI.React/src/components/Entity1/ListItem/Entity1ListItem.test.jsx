import { render, screen, fireEvent } from "@testing-library/react";
import { vi } from "vitest";
import Entity1ListItem from "./Entity1ListItem";

describe("Entity1ListItem Tests", () => {
    const mockEdit = vi.fn();
    const mockDelete = vi.fn();
    const entity = {
        id: "1",
        valueObject1: {
            property1: "Test Property",
        },
    };

    beforeEach(() => {
        mockEdit.mockReset();
        mockDelete.mockReset();
    });

    test("renders entity details correctly", () => {
        render(
            <Entity1ListItem
                entity={entity}
                onDelete={mockDelete}
                onEdit={mockEdit}
            />
        );

        expect(screen.getByText("Test Property")).toBeInTheDocument();
        expect(screen.getByLabelText("Edit")).toBeInTheDocument();
        expect(screen.getByLabelText("Delete")).toBeInTheDocument();
    });

    test("calls onEdit when edit button clicked", () => {
        render(
            <Entity1ListItem
                entity={entity}
                onDelete={mockDelete}
                onEdit={mockEdit}
            />
        );
        const editButton = screen.getByLabelText("Edit");
        fireEvent.click(editButton);
        expect(mockEdit).toHaveBeenCalled();
    });

    test("calls onDelete when delete button clicked", () => {
        render(
            <Entity1ListItem
                entity={entity}
                onDelete={mockDelete}
                onEdit={mockEdit}
            />
        );
        const deleteButton = screen.getByLabelText("Delete");
        fireEvent.click(deleteButton);
        expect(mockDelete).toHaveBeenCalled();
    });
});
