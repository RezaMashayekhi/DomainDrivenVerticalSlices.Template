import { http, HttpResponse } from "msw";

const API_BASE_URL = "/api/entity1";

// Sample data for testing
const mockEntities = [
    { id: "1", valueObject1: { property1: "Test Entity 1" } },
    { id: "2", valueObject1: { property1: "Test Entity 2" } },
    { id: "3", valueObject1: { property1: "Sample Item" } },
];

export const handlers = [
    // GET all entities
    http.get(`${API_BASE_URL}/all`, () => {
        return HttpResponse.json(mockEntities);
    }),

    // GET entity by ID
    http.get(`${API_BASE_URL}/:id`, ({ params }) => {
        const entity = mockEntities.find((e) => e.id === params.id);
        if (entity) {
            return HttpResponse.json(entity);
        }
        return new HttpResponse(null, { status: 404 });
    }),

    // GET search by property1
    http.get(`${API_BASE_URL}/list`, ({ request }) => {
        const url = new URL(request.url);
        const property1 = url.searchParams.get("property1") || "";
        const filtered = mockEntities.filter((e) =>
            e.valueObject1.property1
                .toLowerCase()
                .includes(property1.toLowerCase())
        );
        return HttpResponse.json(filtered);
    }),

    // POST create entity
    http.post(API_BASE_URL, async ({ request }) => {
        const body = await request.json();
        const newEntity = {
            id: crypto.randomUUID(),
            valueObject1: { property1: body.property1 },
        };
        return HttpResponse.json(newEntity, { status: 201 });
    }),

    // PUT update entity
    http.put(`${API_BASE_URL}/:id`, async ({ params, request }) => {
        const body = await request.json();

        if (
            !body ||
            typeof body.property1 !== "string" ||
            body.property1.trim() === ""
        ) {
            return new HttpResponse(null, { status: 400 });
        }

        const entity = mockEntities.find((e) => e.id === params.id);
        if (!entity) {
            return new HttpResponse(null, { status: 404 });
        }

        entity.valueObject1.property1 = body.property1;

        return new HttpResponse(null, { status: 204 });
    }),

    // DELETE entity
    http.delete(`${API_BASE_URL}/:id`, ({ params }) => {
        const entity = mockEntities.find((e) => e.id === params.id);
        if (entity) {
            return new HttpResponse(null, { status: 204 });
        }
        return new HttpResponse(null, { status: 404 });
    }),
];
