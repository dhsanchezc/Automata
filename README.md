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

## Deploy to Azure Container Apps (manual setup)
One-time setup to enable the GitHub Actions deploy job:

1. Create the Container App (portal or CLI). Example:
```
az containerapp up \
  --name automata-api \
  --resource-group automata-rg \
  --location brazilsouth \
  --environment automata-env \
  --image mcr.microsoft.com/k8se/quickstart:latest \
  --target-port 8080 \
  --ingress external
```

2. Store GHCR pull credentials on the app (private images):
```
az containerapp registry set \
  --name automata-api \
  --resource-group automata-rg \
  --server ghcr.io \
  --username <GHCR_USERNAME> \
  --password <GHCR_TOKEN>
```

3. Add GitHub Actions secret `AZURE_CREDENTIALS` using the JSON from:
```
az ad sp create-for-rbac --sdk-auth
```

After that, pushes to `main` build/test, push the image to GHCR, and update the Container App via the workflow in `.github/workflows/main-build-publish.yml`.
