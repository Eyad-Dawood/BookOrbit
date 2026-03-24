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
- **BookOrbit.Domain**: Entities, Value Objects, Domain Errors, and Result abstractions.

---

## 🛠️ Tech Stack

- **Framework**: .NET 9 (ASP.NET Core Web API)
- **Persistence**: SQL Server, Entity Framework Core
- **Messaging/Logic**: MediatR, FluentValidation
- **Auth**: ASP.NET Core Identity, JWT
- **Observability**: OpenTelemetry, Prometheus, Grafana, Jaeger, Seq
- **DevOps**: Docker, Docker Compose

---

## 🚦 Getting Started

### Prerequisites
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
