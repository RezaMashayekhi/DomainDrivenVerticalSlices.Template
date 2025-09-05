import React from "react";
import { render, screen } from "@testing-library/react";
import { vi } from "vitest";
import AddEntity1 from "./AddEntity1";

vi.mock("../Form/Entity1Form", () => ({
    default: (props) => (
        <div
            data-testid="entity-form"
            edit-mode={props.isEdit.toString()}
        ></div>
    ),
}));

describe("AddEntity1 Component Tests", () => {
    test("renders EntityForm with isEdit prop as false", () => {
        render(<AddEntity1 />);
        const formElement = screen.getByTestId("entity-form");
        expect(formElement).toHaveAttribute("edit-mode", "false");
    });
});
