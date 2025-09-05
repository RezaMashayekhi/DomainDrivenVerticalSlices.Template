import React from "react";
import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import Entity1Form from "./Entity1Form";
import { useParams, useNavigate } from "react-router-dom";
import { vi } from "vitest";
import * as service from "../../../services/Entity1Service";

vi.mock("react-router-dom", () => ({
    useParams: vi.fn(),
    useNavigate: vi.fn(),
}));

vi.mock("../../../services/Entity1Service", () => ({
    getEntity1ById: vi.fn(),
    updateEntity1: vi.fn(),
    createEntity1: vi.fn(),
}));

describe("Entity1Form Tests", () => {
    const mockNavigate = vi.fn();
    beforeEach(() => {
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
        render(<Entity1Form isEdit={true} />);

        await waitFor(() =>
            expect(service.getEntity1ById).toHaveBeenCalledWith("1")
        );
        expect(screen.getByDisplayValue("Edit Test")).toBeInTheDocument();
    });

    test("form submits correctly and navigates on success", async () => {
        render(<Entity1Form isEdit={true} />);
        const input = screen.getByTestId("property1-input");
        fireEvent.change(input, { target: { value: "Updated Test" } });
        fireEvent.submit(screen.getByTestId("entity-form"));

        await waitFor(() => expect(service.updateEntity1).toHaveBeenCalled());
        expect(mockNavigate).toHaveBeenCalledWith("/");
    });

    test("cancel button navigates to home", () => {
        render(<Entity1Form isEdit={false} />);
        fireEvent.click(screen.getByText("Cancel"));
        expect(mockNavigate).toHaveBeenCalledWith("/");
    });
});
