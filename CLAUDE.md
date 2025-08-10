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