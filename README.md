# Clean Architecture

byggt med Clean Architecture, CQRS och MediatR.  
Projektet använder JWT‑autentisering, repository‑pattern, pipelines och sökfiltrering.

## Arkitektur

Projektet följer Clean Architecture med fyra lager:

- **Domain** – entiteter och kärnlogik (TaskItem, User)
- **Application** – CQRS (Commands/Queries), DTOs, Validators, Behaviors
- **Infrastructure** – EF Core, DbContext, repositories
- **API** – controllers, DI, autentisering

