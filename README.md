# BookOrbit

BookOrbit is a layered ASP.NET Core Web API for a university-focused book sharing and borrowing platform. Built with a clean separation of concerns, it serves as a robust backend foundation using modern architectural patterns.

## 🚀 Project Status

This project is **under development** (currently paused for college exams).

The core architectural patterns are established, and the repository represents a strong foundation. Future development will follow the existing implementation patterns for the remaining modules.

### Current Implementation Includes:
- Identity & JWT token management (including refresh tokens).
- Student registration and profile management (with image handling).
- Seeded data for testing (Admin & Student roles).
- Comprehensive observability stack.
- Validation, error mapping, and global exception handling.

---

## 🏗️ Architecture

The solution follows a clean, layered architecture:

- **BookOrbit.Api**: Controllers, OpenAPI, Rate Limiting, Caching, and OpenTelemetry.
- **BookOrbit.Application**: MediatR handlers (CQRS), Validators, DTOs, and Pipeline Behaviors.
- **BookOrbit.Infrastructure**: EF Core, Identity, Token Generation, Email Services, and Seeding.
- **BookOrbit.Domain**: , Value Objects, Domain Errors, and Result abstractions.

---

## 🛠️ Tech Stack

- **Framework**: .NET 9 (ASP.NET Core Web API)
- **Persistence**: SQL Server, Entity FEntitiesramework Core
- **Messaging/Logic**: MediatR, FluentValidation
- **Auth**: ASP.NET Core Identity, JWT
- **Observability**: OpenTelemetry, Prometheus, Grafana, Jaeger, Seq
- **DevOps**: Docker, Docker Compose

---

## 🚦 Getting Started

### Prerequisites
<<<<<<< HEAD
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed and running.

### Quick Start
1. **Clone & Navigate**:
   ```bash
   git clone https://github.com/Eyad-Dawood/BookOrbit
   cd BookOrbit/Code
   ```
2. **Launch Stack**:
   ```bash
   docker compose up --build
   ```
3. **Verify Health**: Visit `http://localhost:7240/health`. It should return `Healthy`.

---

## 📊 Service Registry & Credentials

| Service | URL | Credentials (User / Pass) |
| :--- | :--- | :--- |
| **API (HTTP)** | `http://localhost:7240` | - |
| **API (HTTPS)** | `https://localhost:7241` | - |
| **Grafana** | `http://localhost:3000` | `admin` / `sa123456` |
| **Seq** | `http://localhost:8081` | `admin` / `MyStrongPass123!` |
| **Prometheus** | `http://localhost:9090` | - |
| **Jaeger (UI)**| `http://localhost:16686`| - |
| **SQL Server** | `localhost, 1433` | `sa` / `MyStrongPass123!` |

### 🔑 Seeded Test Users
- **Admin**: `admin@bookorbit.com` / `Admin@123456`
- **Student**: `student1@std.mans.edu.eg` / `sa123456`

---

## 📝 Development Notes

### API Testing
- **Postman**: A collection is provided in `Tests/BookOrbit API.postman_collection.json`.
- **SSL Note**: For local testing, prefer the **HTTP** endpoint (`http://localhost:7240`) if you encounter certificate issues with HTTPS in your API client.
- **Warning**: Do not send HTTPS requests to the HTTP port (7240).

### Infrastructure
- **Observability**: The API exports telemetry via OTLP to the container stack. Use Grafana for dashboards and Jaeger for distributed tracing.
- **Database**: To reset the environment completely, use `docker compose down -v`.
- **Images**: A default student image is bundled at `BookOrbit.Api/uploads/Students/DefaultStudentImage.png`.

---

## 🗺️ Roadmap
- [ ] Implement remaining feature modules (Books, Lending, Borrowing).
- [ ] Expand endpoint coverage for existing domain models.
- [ ] Increase unit and integration test coverage.
- [ ] Final production hardening and UI integration.
=======

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
>>>>>>> 54409e5653629315b1d158f519e4fc7b43a054ba
