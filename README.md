# Clean Architecture

byggt med Clean Architecture, CQRS och MediatR.  
Projektet använder JWT‑autentisering, repository‑pattern, pipelines och sökfiltrering.

## Arkitektur

Projektet följer Clean Architecture med fyra lager:

- **Domain** – entiteter och kärnlogik (TaskItem, User)
- **Application** – CQRS (Commands/Queries), DTOs, Validators, Behaviors
- **Infrastructure** – EF Core, DbContext, repositories
- **API** – controllers, DI, autentisering

# 🔄 CQRS‑översikt

CQRS använts för att separera skriv‑ och läsoperationer i tydliga flöden.

## Flöde

Controller
↓
MediatR (Command/Query)
↓
ValidationBehavior (validering → 400 vid fel)
↓
Handler (domänlogik)
↓
OperationResult (Success/Failure)
↓
HTTP Response

# Felhanteringspipeline

Backend använder en centraliserad och konsekvent felhantering baserad på:

- MediatR Pipeline Behaviors  
- FluentValidation  
- OperationResult<T>  
- Minimal Exception Middleware  

Detta ger ett robust och förutsägbart API.