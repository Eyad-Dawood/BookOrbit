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
- Strong observability setup with OpenTelemetry, Prometheus, Grafana, Jaeger, and Seq.

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
- Postman, Insomnia, or any HTTP client is optional but useful for testing the API.

### Steps

1. Clone the repository.
2. Open a terminal inside the `Code` folder.
3. Run:

```bash
docker compose up --build
```

## Services And Credentials

### API

- HTTP Base URL: `http://localhost:7240`
- HTTPS Base URL: `https://localhost:7241`
- Health: `http://localhost:7240/health`
- Metrics: `http://localhost:7240/metrics`
- OpenAPI JSON: `http://localhost:7240/openapi/v1.json`

### Grafana

- URL: `http://localhost:3000`
- Username: `admin`
- Password: `sa123456`

### Seq

- URL: `http://localhost:8081`
- Ingestion port: `http://localhost:5341`
- Username : `admin`
- Password : `MyStrongPass123!`

### Prometheus

- URL: `http://localhost:9090`
- Targets page: `http://localhost:9090/targets`
- API target status endpoint: `http://localhost:9090/api/v1/targets`

### Jaeger

- UI URL: `http://localhost:16686`
- OTLP gRPC receiver: `localhost:4317`
- OTLP HTTP receiver: `localhost:4318`

### SQL Server

- Host: `localhost`
- Port: `1433`
- Username: `sa`
- Password: `MyStrongPass123!`
- Database: `BookOrbitDb`

### Important URL Note

Use the correct protocol with the correct port:

- `http://localhost:7240` is HTTP only
- `https://localhost:7241` is HTTPS only

Do not send `https` requests to port `7240`.

For example, this is wrong:

```text
https://localhost:7240/api/v1/identity/token
```

Use one of these instead:

```text
http://localhost:7240/api/v1/identity/token
https://localhost:7241/api/v1/identity/token
```

If your HTTP client has trouble with the self-signed HTTPS certificate, use the HTTP endpoint on port `7240` for local testing.

### API Testing Note

The `.http` file inside the project does not work reliably for API testing in this setup because of the certificate issue.

If you want to test the API, use Postman or another API client such as Insomnia instead.

A Postman collection file is already included in the [`Tests`](Tests) folder:

- [`BookOrbit API.postman_collection.json`](Tests\BookOrbit API.postman_collection.json)

Import that collection into Postman to test the API endpoints more easily.

## Step By Step: From Clone To First API Call

### 1. Clone the repository

```bash
git clone <your-repository-url>
```

### 2. Move into the project folder

```bash
cd BookOrbit/Code
```

### 3. Start the full Docker stack

```bash
docker compose up --build
```

Wait until the containers finish starting.

### 4. Confirm the API is running

Open:

- `http://localhost:7240/health`

Expected response:

```text
Healthy
```

### 5. Make your first API call

Use the identity token endpoint with one of the seeded users.

HTTP:

```http
POST http://localhost:7240/api/v1/identity/token
Content-Type: application/json
```

HTTPS:

```http
POST https://localhost:7241/api/v1/identity/token
Content-Type: application/json
```

Example request body:

```json
{
  "email": "admin@bookorbit.com",
  "password": "Admin@123456"
}
```

If the request succeeds, the API returns an access token and a refresh token.

### 6. Open the API document

You can inspect the generated OpenAPI document at:

- `http://localhost:7240/openapi/v1.json`
- `https://localhost:7241/openapi/v1.json`

### 7. Open the observability tools

- Grafana: `http://localhost:3000`
- Grafana username: `admin`
- Grafana password: `sa123456`
- Seq: `http://localhost:8081`
- Seq login: not required in the current setup
- Prometheus: `http://localhost:9090`
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

Verified infrastructure:

- Prometheus scrapes the API successfully
- Jaeger receives traces for the `bookorbit` service
- Seq is reachable
- Grafana is reachable

## Current Database Caveat

The project was tested both with existing volumes and with fresh volumes removed using:

```bash
docker compose down -v
docker compose up --build
```

The current startup flow works correctly in a fresh environment.

## Student Default Image

The fallback student image is now included in the published Docker image through:

`BookOrbit.Api/uploads/Students/DefaultStudentImage.png`

That file should remain in the repository so the default student image behavior keeps working inside Docker.

## Seeded Data

On development startup, the application initializes the database and seeds:

- Student role
- Admin role
- Sample student users
- Admin user

Seeded login example:

- Email: `student1@std.mans.edu.eg`
- Password: `sa123456`

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
