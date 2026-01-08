# User API dotnet

🚀 Modern API for user management built with ASP.NET Core and MongoDB

🧾 License MIT

## Overview
- API focused on authentication and user management operations
- Interactive documentation with Swagger in development environment
- Persistence in MongoDB with strong type mapping
- Modular structure with well defined layers and low coupling

## Architecture
- Clean Architecture with layers
- Domain with entities and value objects
- Application with services business rules and notifications
- Persistence with repositories and database context
- Endpoints using minimal API with clear composition

## Design Patterns
- Repository to abstract data access
- Factory to encapsulate complex entity creation and domain rules
- Value Object for Email FullName Password
- DTO to standardize input and output
- Service Layer to orchestrate use cases
- Notification to communicate domain failures
- Dependency Injection for inversion of dependency
- Domain Mapping: Implemented via extension methods to perform high-performance, type-safe conversions from Domain Entities to Response DTOs without the overhead of reflection.

## SOLID Principles
- Single Responsibility each class has a clear focus
- Open Closed easy extension without changing stable code
- Liskov Substitution safe use of abstractions and contracts
- Interface Segregation small and specific contracts
- Dependency Inversion depend on injected abstractions

## Object Calisthenics applied
- Prefer objects and avoid loose primitives
- Keep Entities encapsulated
- Small and cohesive classes
- Short and objective methods
- Meaningful and explicit names
- Preferred immutability in value objects
- Clean DTO Mapping: Decouples the internal domain representation from the external API contracts using a manual mapping approach for maximum transparency and performance.

## Technologies
- .NET 10
- ASP.NET Core
- MongoDB
- Swagger OpenAPI

## Prerequisites
- .NET 10 SDK installed
- Docker and Docker Compose installed

## How to run locally
- Restore dependencies

```bash
dotnet restore
```

- Build

```bash
dotnet build
```

- Run

```bash
dotnet run --project User.Api
```

- Access Swagger in development environment
- Default root URL during local run
- http://localhost:5000 or profile defined port

## How to use with Docker
- Start MongoDB and API with Compose

```bash
docker compose up -d
```

- API available at
- http://localhost:8080
- Environment variables applied to the service
- ASPNETCORE_ENVIRONMENT Development
- ASPNETCORE_URLS http://0.0.0.0:8080
- MongoDbConfigurations__ConnectionString mongodb://mongo:27017
- MongoDbConfigurations__DatabaseName UserDb

## Build and Deploy with Docker
- Build image

```bash
docker build -t user-api:latest -f User.Api/Dockerfile .
```

- Run image

```bash
docker run -d --name user-api -p 8080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Development \
  -e ASPNETCORE_URLS=http://0.0.0.0:8080 \
  -e MongoDbConfigurations__ConnectionString=mongodb://mongo:27017 \
  -e MongoDbConfigurations__DatabaseName=UserDb \
  user-api:latest
```

- Tag and push to registry

```bash
docker tag user-api:latest your-registry.com/user-api:latest
docker push your-registry.com/user-api:latest
```

- Deployment on server
- Pull image from registry
- Configure environment variables
- Publish port 8080

## Key Endpoints
- Authentication
- Login and secure cookie issuance
- Registration create user at POST /auth/register
- Endpoints in AuthEndpoints.cs

- Users
- CRUD with domain validations
- Protected with authorization required
- Endpoints in UserEndpoints.cs
## Security Notes
- Authentication cookies with secure policy and strict SameSite
- Users endpoints require authorization
- Reverse proxy with TLS is recommended in production

## Docker Compose
- File is located at docker-compose.yml
- Mongo service
- User API service with dependency on Mongo

## License
- MIT
- Free use with responsibility and attribution
