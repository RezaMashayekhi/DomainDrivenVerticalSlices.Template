import React from "react";
import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import Entity1Form from "./Entity1Form";
import { useParams, useNavigate } from "react-router-dom";
import * as service from "../../../services/Entity1Service";

jest.mock("react-router-dom", () => ({
    useParams: jest.fn(),
    useNavigate: jest.fn(),
}));

jest.mock("../../../services/Entity1Service", () => ({
    getEntity1ById: jest.fn(),
    updateEntity1: jest.fn(),
    createEntity1: jest.fn(),
}));

describe("Entity1Form Tests", () => {
    const mockNavigate = jest.fn();
    beforeEach(() => {
        useNavigate.mockReturnValue(mockNavigate);
        useParams.mockReturnValue({ id: '1' });
        service.getEntity1ById.mockResolvedValue({ valueObject1: { property1: "Test" } });
        service.updateEntity1.mockResolvedValue(true);
        service.createEntity1.mockResolvedValue(true);
    });

    test("loads and displays data correctly in edit mode", async () => {
        service.getEntity1ById.mockResolvedValue({ valueObject1: { property1: "Edit Test" } });
        render(<Entity1Form isEdit={true} />);
        
        await waitFor(() => expect(service.getEntity1ById).toHaveBeenCalledWith('1'));
        expect(screen.getByDisplayValue("Edit Test")).toBeInTheDocument();
    });

    test("form submits correctly and navigates on success", async () => {
        render(<Entity1Form isEdit={true} />);
        const input = screen.getByTestId("property1-input");
        fireEvent.change(input, { target: { value: 'Updated Test' } });
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

