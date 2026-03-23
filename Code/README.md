# BookOrbit

BookOrbit is a layered ASP.NET Core Web API for a university-focused book sharing and borrowing platform. The project is built with a clean separation between `Api`, `Application`, `Infrastructure`, and `Domain`, and it already includes the core architectural patterns that the rest of the app will follow.

## Project Status

This project is **not completed yet**.

Development is currently paused because of college exams, but the structure is already in place and the rest of the application is intended to continue on the same pattern used in the current implementation.

At the moment, the repository should be viewed as a strong backend foundation rather than a finished product.

## Current Strengths

- Clean layered architecture with clear separation of concerns.
- Domain-first structure with entities, value objects, errors, and result handling.
- CQRS-style application layer using MediatR handlers and pipeline behaviors.
- FluentValidation integrated at the application boundary.
- JWT authentication and refresh token flow.
- Role-based and policy-based authorization.
- API versioning and OpenAPI support.
- Global exception handling and consistent problem details responses.
- Output caching and hybrid caching support.
- Rate limiting for normal and sensitive endpoints.
- SQL Server persistence with Entity Framework Core and seeded startup data.
- Dockerized local environment.
- Strong observability setup with:
  - OpenTelemetry
  - Prometheus metrics scraping
  - Grafana dashboards
  - Jaeger tracing
  - Seq structured logging

## Implemented Areas

The project already includes a working pattern for:

- Identity and token endpoints
- Student registration and update flows
- Student image handling
- Validation, error mapping, caching, authorization, and middleware wiring

The domain model also already contains structure for additional modules such as books, book copies, lending listings, borrowing requests, borrowing transactions, borrowing reviews, and point transactions, even though those parts are not finished yet.

## Architecture

The solution is organized as:

- `BookOrbit.Api`
  - Controllers, middleware, API contracts, OpenAPI setup, authentication wiring, rate limiting, caching, and OpenTelemetry configuration.
- `BookOrbit.Application`
  - Use cases, commands, queries, validators, DTOs, interfaces, and MediatR pipeline behaviors.
- `BookOrbit.Infrastructure`
  - EF Core, Identity, authorization handlers, token generation, email service, caching service, and database initialization/seeding.
- `BookOrbit.Domain`
  - Entities, value objects, enums, domain errors, and result abstractions.

This is one of the main strengths of the project: the implementation pattern is already established, so adding the remaining modules should be straightforward and consistent.

## Observability

One of the strongest parts of this project is the observability stack.

The API is configured with OpenTelemetry and exports telemetry to the local container stack. The repository already includes support for:

- Metrics via Prometheus
- Dashboards via Grafana
- Distributed tracing via Jaeger
- Structured logging via Seq

This means the project is already prepared for:

- Request tracing
- Performance visibility
- Operational metrics
- Debugging through centralized logs

## Running The Project

### Prerequisites

- Docker must be installed on your machine.
- Docker Compose support must be available through Docker Desktop / Docker Engine.

### Steps

1. Clone the repository.
2. Open a terminal inside the `Code` folder.
3. Run:

```bash
docker compose up --build
```

### Main Services

After startup, these services are expected to be available:

- API: `http://localhost:7240`
- Seq: `http://localhost:8081`
- Prometheus: `http://localhost:9090`
- Grafana: `http://localhost:3000`
- Jaeger: `http://localhost:16686`

## Verified Runtime Status

The project was tested with:

```bash
docker compose up --build
```

and the container stack starts successfully.

Verified endpoints:

- `http://localhost:7240/health`
- `http://localhost:7240/openapi/v1.json`
- `http://localhost:7240/metrics`

## Current Database Caveat

There is an important database caveat in the current local Docker setup.

The existing SQL Server volume may already contain tables created from an older startup flow, while EF Core migration history is missing or incomplete. In that case, automatic migration tries to create tables that already exist, which causes SQL exceptions during startup.

The app now tolerates that state and continues running, but this is still not the clean ideal state for the database.

If you start with a fresh SQL Server volume, the setup should be cleaner.

## Important Note About Student Default Image

There is currently an issue with the default student image.

The API expects the fallback image file:

`uploads/Students/DefaultStudentImage.png`

but this file is not cloned into the Docker image automatically in the current setup. If you want the student image fallback behavior to work correctly, add that file manually inside the expected path after running the project.

## Seeded Data

On development startup, the application initializes the database and seeds:

- Student role
- Admin role
- Sample student users
- Admin user

This makes local testing easier while the rest of the application is still under development.

## What Is Still Missing

The main missing part right now is completion, not structure.

The architectural base is already there, but the project still needs:

- The remaining feature modules to be fully implemented
- Broader endpoint coverage across the existing domain structure
- Tests
- Additional production-hardening work
- Final polishing and completion of the whole application flow

Tests and the remaining supporting work are planned to be added with the completion of the app after exams.

## Notes

- This repository currently represents a well-prepared backend foundation.
- The existing implementation shows the intended project pattern clearly.
- The remaining modules are expected to follow the same architecture and conventions already used here.

## Tech Stack

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- MediatR
- FluentValidation
- Serilog
- Seq
- OpenTelemetry
- Prometheus
- Grafana
- Jaeger
- Docker
