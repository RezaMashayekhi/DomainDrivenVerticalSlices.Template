import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import Entity1Form from "./Entity1Form";
import { useParams, useNavigate } from "react-router-dom";
import { vi } from "vitest";
import * as service from "../../../services/Entity1Service";
import { ThemeProvider } from "../../../context/ThemeContext";

vi.mock("react-router-dom", () => ({
    useParams: vi.fn(),
    useNavigate: vi.fn(),
}));

vi.mock("../../../services/Entity1Service", () => ({
    getEntity1ById: vi.fn(),
    updateEntity1: vi.fn(),
    createEntity1: vi.fn(),
}));

const renderWithTheme = (component) => {
    return render(<ThemeProvider>{component}</ThemeProvider>);
};

describe("Entity1Form Tests", () => {
    const mockNavigate = vi.fn();
    beforeEach(() => {
        vi.clearAllMocks();
        useNavigate.mockReturnValue(mockNavigate);
        useParams.mockReturnValue({ id: "1" });
        service.getEntity1ById.mockResolvedValue({
            valueObject1: { property1: "Test" },
        });
        service.updateEntity1.mockResolvedValue(true);
        service.createEntity1.mockResolvedValue(true);
    });

    test("loads and displays data correctly in edit mode", async () => {
        service.getEntity1ById.mockResolvedValue({
            valueObject1: { property1: "Edit Test" },
        });
        renderWithTheme(<Entity1Form isEdit={true} />);

        await waitFor(() =>
            expect(service.getEntity1ById).toHaveBeenCalledWith("1")
        );
        expect(screen.getByDisplayValue("Edit Test")).toBeInTheDocument();
    });

    test("form submits correctly and shows success modal", async () => {
        renderWithTheme(<Entity1Form isEdit={true} />);
        const input = screen.getByTestId("property1-input");
        fireEvent.change(input, { target: { value: "Updated Test" } });
        fireEvent.submit(screen.getByTestId("entity-form"));

        await waitFor(() => expect(service.updateEntity1).toHaveBeenCalled());
        // Check that success modal appears
        await waitFor(() =>
            expect(
                screen.getByText("Entity has been updated successfully.")
            ).toBeInTheDocument()
        );
    });

    test("cancel button opens cancel modal", () => {
        renderWithTheme(<Entity1Form isEdit={false} />);
        fireEvent.click(screen.getByText("Cancel"));
        // Check that cancel modal appears
        expect(
            screen.getByText(/Are you sure you want to cancel/i)
        ).toBeInTheDocument();
    });

    test("confirming cancel navigates to home", async () => {
        renderWithTheme(<Entity1Form isEdit={false} />);
        fireEvent.click(screen.getByText("Cancel"));

        // Find and click the "Yes, Cancel" button in the modal
        const confirmButton = screen.getByText("Yes, Cancel");
        fireEvent.click(confirmButton);

        expect(mockNavigate).toHaveBeenCalledWith("/");
    });

    test("confirming save success navigates to home", async () => {
        renderWithTheme(<Entity1Form isEdit={false} />);
        const input = screen.getByTestId("property1-input");
        fireEvent.change(input, { target: { value: "New Entity" } });
        fireEvent.submit(screen.getByTestId("entity-form"));

        await waitFor(() => expect(service.createEntity1).toHaveBeenCalled());

        // Find and click OK on success modal
        await waitFor(() => expect(screen.getByText("OK")).toBeInTheDocument());
        fireEvent.click(screen.getByText("OK"));

        expect(mockNavigate).toHaveBeenCalledWith("/");
    });
});
