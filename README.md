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

## Deploy to Azure

### Deploy to Azure Container Apps (manual setup)
One-time setup to enable the GitHub Actions deploy job:

1. Create the Container App (portal or CLI). Example:
```
# Create the Container App and its environment if needed.
az containerapp up \
  --name automata-api \
  --resource-group automata-rg \
  --location canadacentral \
  --environment automata-env \
  --image mcr.microsoft.com/k8se/quickstart:latest \
  --target-port 8080 \
  --ingress external
```

2. Store GHCR pull credentials on the app (private images):
```
# Save GHCR credentials on the app for private image pulls.
az containerapp registry set \
  --name automata-api \
  --resource-group automata-rg \
  --server ghcr.io \
  --username <GHCR_USERNAME> \
  --password <GHCR_TOKEN>
```

3. Add GitHub Actions secret `AZURE_CREDENTIALS` using the JSON from (scope to your RG):
```
# Create a service principal scoped to the resource group.
az ad sp create-for-rbac \
  --name automata-gh-actions \
  --role Contributor \
  --scopes /subscriptions/<SUBSCRIPTION_ID>/resourceGroups/automata-rg \
  --sdk-auth
```

After that, pushes to `main` build/test, push the image to GHCR, and update the Container App via the workflow in `.github/workflows/main-build-publish.yml`.

### Configure CORS for the frontend (Azure Container Apps)
Set the allowed origin(s) so browsers can call the API:

```
az containerapp update \
  --name automata-api \
  --resource-group automata-rg \
  --set-env-vars Cors__AllowedOrigins__0=https://app.example.com
```

Add more origins by incrementing the index (`Cors__AllowedOrigins__1`, etc.). Use the exact origin and no trailing slash.

### Azure Database for PostgreSQL (recommended)
Use Azure Database for PostgreSQL Flexible Server for a managed database:

```
# 1) Create the server (choose a globally unique name).
az postgres flexible-server create \
  --resource-group automata-rg \
  --name automata-pg \
  --location canadacentral \
  --admin-user automata_admin \
  --admin-password <strong-pass> \
  --tier Burstable \
  --sku-name Standard_B1ms \
  --storage-size 32 \
  --version 18

# 2) Create the database.
az postgres flexible-server db create \
  --resource-group automata-rg \
  --server-name automata-pg \
  --database-name automata
```

Allow Azure services (like ACA) to connect to the database:

```
az postgres flexible-server firewall-rule create \
  -g automata-rg \
  -n automata-pg \
  --rule-name allow-azure \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0
```

Store the connection string and map it to the env var the app expects (older CLI versions need two steps):

```
az containerapp secret set \
  --name automata-api \
  --resource-group automata-rg \
  --secrets db-conn="Host=automata-pg.postgres.database.azure.com;Port=5432;Database=automata;Username=automata_admin;Password=<strong-pass>;Ssl Mode=Require"

az containerapp update \
  --name automata-api \
  --resource-group automata-rg \
  --set-env-vars ConnectionStrings__DefaultConnection=secretref:db-conn
```

### Find service URLs (FQDNs)
Find the PostgreSQL server hostname (FQDN):

```
az postgres flexible-server show \
  -g automata-rg \
  -n automata-pg \
  --query fullyQualifiedDomainName \
  -o tsv
```

Find the API URL (Container App FQDN):

```
az containerapp show \
  -g automata-rg \
  -n automata-api \
  --query properties.configuration.ingress.fqdn \
  -o tsv
```

### Connect with pgAdmin (easiest path)
1. Allow your public IP on the database firewall:
```
az postgres flexible-server firewall-rule create \
  -g automata-rg \
  -n automata-pg \
  --rule-name my-ip \
  --start-ip-address <your.public.ip> \
  --end-ip-address <your.public.ip>
```

2. pgAdmin connection settings:
- Host: `automata-pg.postgres.database.azure.com`
- Port: `5432`
- Maintenance DB: `automata`
- Username: `automata_admin`
- Password: `<strong-pass>`
- SSL Mode: `Require` (no cert file needed)

If pgAdmin runs in Docker, only use a root cert if you mount it into the container.
