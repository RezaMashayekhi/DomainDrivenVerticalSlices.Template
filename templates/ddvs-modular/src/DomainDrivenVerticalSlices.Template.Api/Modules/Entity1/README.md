# Entity1 Module

The Entity1 module demonstrates the modular vertical slice structure.

Each feature keeps its request, response, validator, handler, and endpoint close together. Domain rules live under `Domain`, and EF Core mapping lives under `Infrastructure`.
