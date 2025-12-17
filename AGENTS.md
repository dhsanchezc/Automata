# Repository Guidelines

## Project Structure & Module Organization
The solution follows DDD boundaries reinforced by a hexagonal layout. `Automata.Domain` is the core model for aggregates, value objects, and domain services. Application orchestration, commands, and queries live in `Automata.Application`, while adapters for persistence, messaging, and external services sit in `Automata.Infrastructure`. `Automata.Api` exposes Minimal API endpoints through `Program.cs` and handler classes in `ApiHandlers`. Test projects mirror the vertical slice: `Automata.Domain.Tests` for aggregates and policies, `Automata.Api.Tests` for endpoint behavior. Shared configuration values live in `appsettings.json` with environment overrides under `appsettings.Development.json`.

## Build, Test, and Development Commands
- `dotnet restore Automata.sln` pull NuGet dependencies before first build.
- `dotnet build Automata.sln` compile all projects with analyzer checks.
- `dotnet run --project Automata.Api` launch the HTTP API (uses Development settings by default).
- `dotnet test Automata.sln` execute the full xUnit suite with coverage output.
- `docker-compose up -d db` start the PostgreSQL dependency; shut down with `docker-compose down`.

## Coding Style & Naming Conventions
Use 4-space indentation, braces on new lines, and expression-bodied members sparingly. Stick to PascalCase for public types/members, camelCase for locals and parameters, and suffix asynchronous methods with `Async`. Keep handlers lean by delegating business rules to domain/services. Run `dotnet format` prior to pushing to keep Roslyn analyzers satisfied.

## Testing Guidelines
Adopt deterministic, AAA-structured tests and isolate domain logic with in-memory fakes instead of infrastructure calls. Name tests using `Method_Scenario_Result` and store fixtures alongside the feature under `Automata.Domain.Tests` or `Automata.Api.Tests`. Run `dotnet test` before every PR and extend coverage whenever you change behavior.

## Architecture & CQRS Expectations
Align features with the guidance in https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/. Model aggregates around business invariants, expose intent through Commands/Queries in `Automata.Application`, and keep `Automata.Api` handlers as thin adapters. When integrating new services, add ports in the Application layer and adapt them in Infrastructure. Document any cross-boundary events or read-model projections in the PR description.

## Commit & Pull Request Guidelines
Write present-tense commit messages with a concise scope (e.g., `Add maintenance scheduling guard`). Group related changes so each commit remains reviewable. Pull requests should explain the problem, reference issues, list validation commands, and include payload samples or screenshots for API-impacting changes. Highlight any DDD boundary adjustments or new adapters so reviewers can assess architectural impacts.
