import Entity1List from "./components/Entity1/List/Entity1List";
import Entity1Form from "./components/Entity1/Form/Entity1Form";
import PageNotFound from "./layout/PageNotFound";

const AppRoutes = [
    {
        index: true,
        element: <Entity1List />,
    },
    {
        path: "/add-entity1",
        element: <Entity1Form isEdit={false} />,
    },
    {
        path: "/edit-entity1/:id",
        element: <Entity1Form isEdit={true} />,
    },
    {
        path: "*",
        element: <PageNotFound />,
    },
];

export default AppRoutes;
