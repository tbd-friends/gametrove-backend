# GameTrove 25

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

A modular .NET 9 solution for video game cataloging and tracking, built with a clean, layered architecture, FastEndpoints for lightweight APIs, EF Core for persistence, and .NET Aspire for local distributed application orchestration.

## High-Level Architecture

```
+-------------------+      +-------------------+      +------------------+
|    Client App     | ---> |    API Gateway    | ---> |    games-api     |
+-------------------+      | (YARP ReverseProxy|      |  (FastEndpoints) |
                           +-------------------+      +---------+--------+
                                                                |
                                                                v
                                                         +--------------+
                                                         |   EF Core    |
                                                         | SQL Server   |
                                                         +--------------+

+-------------------+
|    igdb-api       |  (Pluggable external data integration)
+-------------------+
```

Projects:
- api-gateway: Reverse proxy / service composition (YARP + service discovery)
- games-api: Public game-related endpoints (FastEndpoints, Swagger, Auth0 auth)
- igdb-api: Integration surface for external game data (IGDB-like provider)
- games-application: Application layer (Mediator-style commands/queries)
- games.infrastructure: EF Core persistence, repositories, DbContext factory
- games.domain: Entities and domain model
- authentication: Auth0 configuration + JWT authentication extensions
- shared-kernel: Cross-cutting abstractions (e.g., IRepository)
- configuration/aspire: Aspire app host & service defaults for local distributed orchestration
- configuration/deployment: Docker compose & infra manifests (SQL Server, registry, etc.)

## Tech Stack
- .NET 9
- FastEndpoints (minimal, high-performance endpoint model)
- EF Core 9 (SQL Server provider, pooled DbContext factory, memory cache)
- YARP Reverse Proxy (api-gateway)
- OpenTelemetry (exporters wired via Aspire .WithOtlpExporter())
- Auth0 (JWT bearer authentication)
- Central Package Management (Directory.Packages.props)

## Getting Started

### 1. Prerequisites
- .NET 9 SDK installed
- Docker Desktop (for local SQL Server)
- (Optional) Auth0 tenant (for fully authenticated flows)

### 2. Clone
```
git clone <your-fork-url> GameTrove25
cd GameTrove25
```

### 3. Run Infrastructure (SQL Server)
Create a local `.env` (do NOT commit) to override sensitive values:
```
SQL_PASS=YourStrong!Pass123
DATA_DIRECTORY=C:/dev/data/gametrove/data
```
Then:
```
cd configuration/deployment/SqlServer
docker compose --env-file ../services.env up -d
```
You may also supply your own env file; ensure `SQL_PASS` matches your connection string.

### 4. Configure Connection String
`src/games-api/appsettings.json` includes a placeholder:
```
"ConnectionStrings": {
  "gametracking-work": "{CONNECTION_STRING}"
}
```
Replace `{CONNECTION_STRING}` locally (User Secrets recommended):
```
dotnet user-secrets init --project src/games-api/games-api.csproj
dotnet user-secrets set "ConnectionStrings:gametracking-work" "Server=localhost;Database=gametracking;User Id=sa;Password=YourStrong!Pass123;TrustServerCertificate=True" --project src/games-api/games-api.csproj
```

### 5. (Optional) Auth0 Settings
Set (user secrets or environment variables):
```
dotnet user-secrets set "Auth0:Domain" "<your-domain>" --project src/games-api/games-api.csproj
# ... similarly Audience, ClientId, ClientSecret, ManagementApiAudience
```
Authentication can be toggled via provided policies; without valid tokens secured endpoints will reject calls.

### 6. Run via Aspire App Host
The Aspire app host wires services & telemetry.
```
dotnet run --project configuration/aspire/aspire-app-host/aspire-app-host.csproj
```
This will launch referenced projects (games-api, igdb-api, api-gateway) with OpenTelemetry exporters registered.

### 7. Direct Project Runs (Alternative)
```
dotnet build
# Run gateway (reverse proxy)
dotnet run --project src/api-gateway/api-gateway.csproj
# Run games API
dotnet run --project src/games-api/games-api.csproj
```

## EF Core & Migrations
Design-time factory: `GameTrackingDbContextFactory` supplies a dummy connection string for tooling; runtime uses configured connection string.

Add a migration:
```
dotnet ef migrations add InitialCreate \
  --project src/games.infrastructure/games.infrastructure.csproj \
  --startup-project src/games-api/games-api.csproj \
  --context GameTrackingContext \
  --output-dir Migrations
```
Apply migrations (at startup, or manually):
```
dotnet ef database update \
  --project src/games.infrastructure/games.infrastructure.csproj \
  --startup-project src/games-api/games-api.csproj \
  --context GameTrackingContext
```

## Request Flow
1. Client calls API Gateway
2. Gateway (YARP) routes to games-api (service discovery aware)
3. games-api endpoint -> application layer (command/query) -> repository via shared kernel abstraction
4. Repository uses pooled EF Core DbContext (scoped) resolved from factory (with memory cache for second-level query caching pattern)
5. Response surfaces via FastEndpoints back through gateway

## Key Design Choices
- Pooled DbContext Factory: Reduces allocation churn under load
- MemoryCache registration: Shared instance injected into EF for second-level caching (when configured)
- Central Package Versions: Single source of truth prevents version drift
- Aspire: Simplifies multi-service local orchestration & telemetry
- YARP + Service Discovery: Decouples clients from concrete service addresses

## Environment Variables (Illustrative)
| Variable | Purpose | Example |
|----------|---------|---------|
| SQL_PASS | SA password for local SQL container | YourStrong!Pass123 |
| DATA_DIRECTORY | Host path for persisted SQL data | C:/dev/data/gametrove/data |
| Auth0:* | Auth0 configuration (Domain, Audience, etc.) | (tenant-specific) |

(Do not commit real secrets; use user secrets or environment config providers.)

## Testing (Future)
No formal test projects yet. Suggested next steps:
- Add unit tests for domain entities & repository abstraction
- Add integration tests with Testcontainers for SQL Server
- Add endpoint tests using FastEndpoints' test host utilities

## Logging & Telemetry
- Console logging currently enabled with EF Core `LogTo(Console.WriteLine)`
- OpenTelemetry exporters configured in Aspire host (`WithOtlpExporter`) – configure OTLP endpoint via env vars when needed

## Extending
- Add caching layer (e.g., Redis) for heavy read endpoints
- Implement pagination & filtering via Ardalis.Specification
- Harden Auth policies (scopes, roles, claims mapping)
- Add rate limiting at gateway

## Common Commands
```
# Build entire solution
dotnet build

# Format (if dotnet format installed)
dotnet format

# List EF migrations
dotnet ef migrations list \
  --project src/games.infrastructure/games.infrastructure.csproj \
  --startup-project src/games-api/games-api.csproj
```

## Troubleshooting
| Issue | Cause | Fix |
|-------|-------|-----|
| Login failures | Auth0 settings missing | Set user secrets for Auth0 section |
| SQL login error | Wrong SA password | Ensure docker env & connection string match |
| Proxy 502 errors | Target API not started | Start games-api or run via Aspire host |
| Migration design-time failure | Missing tooling | `dotnet tool install --global dotnet-ef` |

## Contributing
1. Fork & branch (`feat/<short-description>`)
2. Ensure build passes
3. Keep PRs focused & documented

## Security
- Never commit real secrets (replace with placeholders)
- Rotate credentials periodically
- Consider adding Git hooks / secret scanners

## License
Released under the MIT License. See the LICENSE file in the repository root for full text.

(c) 2025 Terry Burns-Dyson

You are free to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the software, subject to inclusion of the copyright & permission notice.

SPDX-License-Identifier: MIT

## Status
Early development; expect breaking changes as modules evolve.

---
Generated README – refine as the project matures.
