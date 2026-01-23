import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import Entity1List from "./Entity1List";
import { useNavigate } from "react-router-dom";
import { vi } from "vitest";
import * as Entity1Service from "../../../services/Entity1Service";
import { ThemeProvider } from "../../../context/ThemeContext";

vi.mock("react-router-dom", () => ({
    useNavigate: vi.fn(),
}));

vi.mock("../../../services/Entity1Service", () => ({
    getAllEntity1: vi.fn(),
    searchEntity1ByProperty1: vi.fn(),
    deleteEntity1ById: vi.fn(),
}));

const renderWithTheme = (component) => {
    return render(<ThemeProvider>{component}</ThemeProvider>);
};

describe("Entity1List Component Tests", () => {
    const mockNavigate = vi.fn();

    beforeEach(() => {
        vi.clearAllMocks();
        mockNavigate.mockReset();
        useNavigate.mockReturnValue(mockNavigate);
        Entity1Service.getAllEntity1.mockResolvedValue([]);
        Entity1Service.searchEntity1ByProperty1.mockResolvedValue([]);
        Entity1Service.deleteEntity1ById.mockResolvedValue(true);
    });

    test("initially calls getAllEntity1", async () => {
        renderWithTheme(<Entity1List />);
        await waitFor(() =>
            expect(Entity1Service.getAllEntity1).toHaveBeenCalled()
        );
    });

    test("searches entities when search query is changed", async () => {
        renderWithTheme(<Entity1List />);
        const searchBar = screen.getByPlaceholderText("Search by Property1...");
        fireEvent.change(searchBar, { target: { value: "test" } });
        await waitFor(() => {
            expect(
                Entity1Service.searchEntity1ByProperty1
            ).toHaveBeenCalledWith("test");
        });
    });

    test("navigates to add entity page on add button click", () => {
        renderWithTheme(<Entity1List />);
        fireEvent.click(screen.getByLabelText("Add Entity1"));
        expect(mockNavigate).toHaveBeenCalledWith("/add-entity1");
    });

    test("handles pagination correctly", async () => {
        Entity1Service.getAllEntity1.mockResolvedValue(
            Array.from({ length: 10 }, (_, index) => ({
                id: `unique_id_${index}`,
                valueObject1: { property1: "Property" },
            }))
        );
        renderWithTheme(<Entity1List />);
        await waitFor(() =>
            expect(Entity1Service.getAllEntity1).toHaveBeenCalled()
        );
        // With 10 items and 5 per page, we should have 2 page buttons
        await waitFor(() => {
            expect(
                screen.getAllByRole("button", { name: /^\d+$/ })
            ).toHaveLength(2);
        });

        const pageTwoButton = screen.getByRole("button", { name: "2" });
        fireEvent.click(pageTwoButton);

        await waitFor(() => {
            const displayedItems = screen.getAllByText("Property");
            expect(displayedItems.length).toBeLessThanOrEqual(5);
        });
    });

    test("handles entity deletion with confirmation modal", async () => {
        const mockEntity = {
            id: "unique_id",
            valueObject1: { property1: "Property" },
        };
        Entity1Service.getAllEntity1.mockResolvedValue([mockEntity]);

        Entity1Service.getAllEntity1.mockClear();

        renderWithTheme(<Entity1List />);

        // Wait for the entity to be displayed
        await waitFor(() => {
            expect(screen.getByText("Property")).toBeInTheDocument();
        });

        // Click delete button - should open modal
        const deleteButton = screen.getByLabelText("Delete");
        fireEvent.click(deleteButton);

        // Modal should be visible
        await waitFor(() => {
            expect(
                screen.getByText(/Are you sure you want to delete this item/i)
            ).toBeInTheDocument();
        });

        // Click confirm delete button
        const confirmDeleteButton = screen.getByRole("button", {
            name: /delete/i,
        });
        fireEvent.click(confirmDeleteButton);

        // Wait for the delete operation
        await waitFor(() => {
            expect(Entity1Service.deleteEntity1ById).toHaveBeenCalledWith(
                mockEntity.id
            );
        });
    });

    test("navigates to edit page on edit button click", async () => {
        const mockEntity = {
            id: "unique_id",
            valueObject1: { property1: "Property" },
        };
        Entity1Service.getAllEntity1.mockResolvedValue([mockEntity]);
        renderWithTheme(<Entity1List />);

        // Wait for the entity to be displayed
        await waitFor(() => {
            expect(screen.getByText("Property")).toBeInTheDocument();
        });

        const editButton = screen.getByLabelText("Edit");
        fireEvent.click(editButton);
        expect(mockNavigate).toHaveBeenCalledWith(
            `/edit-entity1/${mockEntity.id}`
        );
    });
});
