# Automata
Microservices-based .NET application for asset maintenance and industrial control using minimal APIs, Kubernetes, and IoT integration.

Refer to `AGENTS.md` for detailed contributor guidelines and architectural expectations.

## Local dev
- Start DBs: `docker compose up -d postgres pgadmin` (Postgres 18 pinned; data in `pgdata`).
- Run API locally for hot reload: `dotnet run --project Automata.Api` (uses localhost connection string).
- Containerized API (when needed): `docker compose up -d --build` to rebuild after API changes; stop if already running with `docker compose stop api`.
- Apply migrations manually after schema changes or volume reset (`docker compose down -v`).

## Build API image (for GHCR or other registries)
Build and run the API container image:

```
docker build -t ghcr.io/github-username/automata-api -f Automata.Api/Dockerfile .
docker run -p 8080:8080 ghcr.io/github-username/automata-api
```
