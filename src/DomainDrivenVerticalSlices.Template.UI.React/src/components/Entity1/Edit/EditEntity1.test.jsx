import React from "react";
import { render, screen } from "@testing-library/react";
import EditEntity1 from "./EditEntity1";

jest.mock("../Form/Entity1Form", () => (props) => (
    <div data-testid="entity-form" edit-mode={props.isEdit.toString()}></div>
));

describe("EditEntity1 Component Tests", () => {
    test("renders EntityForm with isEdit prop as true", () => {
        render(<EditEntity1 />);
        const formElement = screen.getByTestId("entity-form");
        expect(formElement).toHaveAttribute("edit-mode", "true");
    });
});
