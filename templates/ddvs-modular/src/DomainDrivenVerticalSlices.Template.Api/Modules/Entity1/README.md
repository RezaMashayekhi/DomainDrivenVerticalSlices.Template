# Entity1 Module

The Entity1 module demonstrates the modular vertical slice structure.

Each feature keeps its request, response, validator, handler, and endpoint close together. Domain rules live under `Domain`, and EF Core mapping lives under `Infrastructure`.

## Events are inert extension points

`Entity1CreatedDomainEvent` (raised on the entity) and `Contracts/Entity1CreatedIntegrationEvent`
are **not dispatched or published by any built-in mechanism** — this template intentionally has no
mediator or hidden event pipeline. They exist as typed extension points: if your application needs
event handling, add an explicit dispatch step (for example, collect and clear `DomainEvents` in a
`SaveChangesAsync` override, or publish integration events from a handler) and delete them if you
do not. Do not assume side effects occur when an entity is created.
