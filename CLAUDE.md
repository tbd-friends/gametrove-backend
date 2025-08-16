# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

GameTrove25 is a .NET 9 microservices architecture for managing game library data. The system provides wrapper APIs for external services (IGDB, PriceCharting) to centralize authentication and data access patterns. **Note: The Angular client code is legacy and should be ignored - this is a backend-only API system.**

## Architecture

### Microservices Structure
- **games-api** (`src/games-api/`): Core game library management API using FastEndpoints
- **igdb-api** (`src/igdb-api/`): IGDB service wrapper using Ardalis.ApiEndpoints  
- **api-gateway** (`src/api-gateway/`): YARP reverse proxy for service orchestration
- **authentication** (`src/authentication/`): Shared Auth0 authentication library
- **aspire-app-host** (`configuration/aspire/aspire-app-host/`): .NET Aspire orchestrator

### Key Technologies
- .NET 9.0 with nullable reference types enabled
- Entity Framework Core 9 with SQL Server and memory caching
- FastEndpoints (games-api) and Ardalis.ApiEndpoints (igdb-api) for minimal APIs
- YARP reverse proxy for API gateway
- Auth0 JWT Bearer authentication with custom abstraction layer
- .NET Aspire for local development orchestration
- OpenTelemetry for observability

### Database Design
The `GameTrackingContext` manages:
- `Games`: Core game entities with platform and publisher relationships
- `GameCopies`: Physical/digital copies with UPC tracking
- `Platforms`: Gaming platforms (console, PC, etc.)
- `Publishers`: Game publishers
- `PriceChartingGameCopyAssociations`: Links to external pricing data

## Development Commands

### Build and Run
```bash
# Build entire solution
dotnet build "Current Structure API.sln"

# Run with Aspire orchestration (recommended)
dotnet run --project configuration/aspire/aspire-app-host/aspire-app-host.csproj

# Run individual services
dotnet run --project src/games-api/games-api.csproj
dotnet run --project src/igdb-api/igdb-api.csproj
dotnet run --project src/api-gateway/api-gateway.csproj
```

### Database Operations
```bash
# Add migrations (from games-api directory)
cd src/games-api
dotnet ef migrations add <MigrationName>

# Update database
dotnet ef database update
```

### Development Environment
```bash
# Restore packages
dotnet restore

# Run with file watching
dotnet watch --project src/games-api/games-api.csproj
```

## Project Patterns

### API Endpoint Structure
- **games-api**: Uses FastEndpoints pattern with separate query/response models
- **igdb-api**: Uses Ardalis.ApiEndpoints with controller-like structure
- All endpoints follow RESTful conventions with proper HTTP status codes

### Configuration Management
- Centralized package versions in `Directory.Packages.props`
- Service-specific settings in individual `appsettings.json` files
- Aspire service defaults shared across all services
- Connection strings managed through Aspire service discovery

### Data Access Pattern
- Pooled DbContext factory for performance (`AddPooledDbContextFactory`)
- Memory caching integration with Entity Framework
- Repository pattern implied through endpoint structure
- Async/await throughout data access layer

### Cross-Cutting Concerns
- CORS configured to allow any origin (development setup)
- OpenTelemetry integration for distributed tracing
- Swagger/OpenAPI documentation generation
- Health checks and service discovery ready

## Service Communication
- Services communicate through the API Gateway
- Service discovery handled by Aspire in development
- Each service registers with OpenTelemetry for distributed tracing
- HTTP client resilience patterns configured

## Database Schema Notes
- Uses GUID identifiers exposed as `Identifier` properties
- Supports search across game names and UPC codes
- Platform and publisher relationships with optional publisher
- Game copies track individual instances with condition states

## Authentication

### Auth0 Integration
The system includes a shared authentication library (`src/authentication/`) that abstracts Auth0 functionality:

#### Configuration
Add Auth0 settings to `appsettings.json`:
```json
{
  "Auth0": {
    "Domain": "{YOUR_AUTH0_DOMAIN}",
    "Audience": "{YOUR_AUTH0_API_IDENTIFIER}",
    "ClientId": "{YOUR_AUTH0_CLIENT_ID}",
    "ClientSecret": "{YOUR_AUTH0_CLIENT_SECRET}",
    "ManagementApiAudience": "{YOUR_AUTH0_MANAGEMENT_API_AUDIENCE}",
    "ValidateIssuer": true,
    "ValidateAudience": true,
    "RequireHttpsMetadata": true
  }
}
```

#### Usage in Program.cs
```csharp
using Authentication.Extensions;

// Add to Program.cs
builder.Services.AddAuth0Authentication(builder.Configuration);

// Add middleware after CORS
app.UseAuthentication();
app.UseAuthorization();
```

#### Endpoint Usage
```csharp
using Authentication.Extensions;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public async Task<IActionResult> SecureEndpoint()
{
    var userInfo = await HttpContext.GetCurrentUserAsync();
    var userId = HttpContext.GetUserId();
    var hasScope = await HttpContext.HasRequiredScopeAsync("read:games");
    
    return Ok(userInfo);
}
```

#### Available Services
- `IAuthenticationService`: User info retrieval, token validation, role management
- `ITokenService`: Auth0 Management API token handling
- Extension methods for `HttpContext` to get user info and check scopes

## Development Guidelines
- Use the Aspire app host for local development
- Services should be developed independently but tested through the gateway
- Entity Framework migrations should be applied to games-api
- Follow existing FastEndpoints/ApiEndpoints patterns for new functionality
- Maintain OpenTelemetry instrumentation for new endpoints
- Use `[Authorize]` attribute and authentication extensions for securing endpoints
- *IMPORTANT* *DO NOT IGNORE* Do not attempt to build the project. Prompt me at the end of a instructions to build if that is the next required step

---
## Security Checklist (Pre-Public & Pre-PR Standard Action)

When preparing any branch for a public push or opening a PR, perform ALL of the following steps. Automate where possible. If any check fails, remediate before proceeding.

### 1. Secret Hygiene
- [ ] Run secret scan: `gitleaks detect -v --config=.gitleaks.toml` (or GitHub Advanced Security if enabled)
- [ ] Ensure no real secrets in: `appsettings*.json`, `*.cs` constants, `docker-compose.yml`, `*.env`
- [ ] Confirm `services.env` is NOT committed (rename to `services.env.example` for samples)
- [ ] Replace any sample credentials with placeholders (e.g. `<CHANGE_ME>`)
- [ ] Rotate any previously exposed secrets and document rotation in SECURITY.md (internal process note)

### 2. Dependency & Supply Chain
- [ ] List vulnerable packages: `dotnet list package --vulnerable --include-transitive`
- [ ] Upgrade or suppress with justification (NEVER ignore Critical/High)
- [ ] Validate central versions in `Directory.Packages.props` (no unpinned floating versions)
- [ ] Check for unexpected added package sources (NuGet.config if present)

### 3. Static Analysis & Build Integrity
- [ ] Enable Treat Warnings as Errors locally when adding analyzers
- [ ] (Planned) Integrate Roslyn security analyzers (e.g. `Microsoft.CodeAnalysis.NetAnalyzers` w/ security rules enabled)
- [ ] (Planned) Add `dotnet format --verify-no-changes` in CI to prevent style drift (reduces review surface & hidden injections)

### 4. Configuration & Infrastructure
- [ ] Inspect `docker-compose.yml` files: no privileged containers, pinned image tags (avoid `latest` for production)
- [ ] Verify SQL Server password not hard-coded anywhere except local `.env`
- [ ] Ensure CORS wildcards are limited or documented as DEV ONLY
- [ ] Confirm no debug/trace sensitive logging in production (e.g. EF `LogTo` may leak SQL)

### 5. Authentication & Authorization
- [ ] Auth0 settings only via secrets manager or environment (NOT committed)
- [ ] Verify policies (`AuthPolicy`) enforce auth for non-public endpoints
- [ ] Plan granular scopes for new endpoints (document required scopes)

### 6. Data Protection & Privacy
- [ ] No PII stored outside intended tables
- [ ] Connection strings use encryption flags where applicable (future: Azure SQL / Always Encrypted options)

### 7. Telemetry & Logging
- [ ] Ensure OpenTelemetry exporter endpoints not leaking credentials
- [ ] Remove/guard any verbose logging of tokens or user claims

### 8. Documentation Updates
- [ ] Update README if new security-relevant setup changes occurred
- [ ] Update SECURITY.md if new disclosure or handling process is defined

### 9. CI/CD (Planned Enhancements)
- [ ] Add GitHub Actions workflow: build, vuln scan, gitleaks, codeql (if enabled)
- [ ] Add status badges (build, security scan) post-publication

### 10. Final Gate
- [ ] Manual diff review: No accidental binary/credential additions
- [ ] Tag reviewer for security sign-off if high-impact change

---
### Standard Commands Bundle
```
# Run full local security sweep (requires gitleaks installed)
pwsh ./scripts/security-scan.ps1
```

### Agent Instructions (Automation Assistants)
Agents MUST perform the following before suggesting a push:
1. Run secret scan (gitleaks) – if tool not present, instruct user to install.
2. Run dependency vulnerability listing.
3. Report any HIGH/CRITICAL issues explicitly.
4. Refuse to proceed if secrets are embedded in code/config; prompt for remediation.

If a build step is required to surface analyzers, you MUST prompt the user explicitly per the project rule above.

---
### Future Improvements (Track & Implement Incrementally)
- Add CodeQL workflow
- Add SBOM generation (`dotnet build /property:GeneratePackageOnBuild=true` + `syft`) & sign artifacts
- Integrate dependency auto-update tooling (Dependabot)
- Add container image scanning (e.g., Trivy) once images are published

---
End Security Checklist
