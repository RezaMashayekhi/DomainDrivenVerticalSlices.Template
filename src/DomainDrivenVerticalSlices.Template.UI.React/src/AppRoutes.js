import Entity1List from "./components/Entity1/List/Entity1List";
import AddEntity1 from "./components/Entity1/Add/AddEntity1";
import EditEntity1 from "./components/Entity1/Edit/EditEntity1";

const AppRoutes = [
    {
        index: true,
        element: <Entity1List />,
    },
    {
        path: "/add-entity1",
        element: <AddEntity1 />,
    },
    {
        path: "/edit-entity1/:id",
        element: <EditEntity1 />,
    },
];

export default AppRoutes;
