# Banking Solution

This is a demonstration project for a simple banking system with REST API using Clean Architecture, .NET 9, Entity Framework Core (SQLite), and LiteBus for CQRS. It supports account and transaction management with unit and integration tests.

---

## Project Structure

The backend is located in `BankingSolutionBackend`, consisting of:

- `BankingSolution.WebApi` — ASP.NET Core Web API  
- `BankingSolution.Application` — CQRS layer (commands, queries, DTOs)  
- `BankingSolution.Domain` — Entities and interfaces  
- `BankingSolution.Infrastructure` — EF Core implementation with SQLite  
- `BankingSolution.Tests.Unit` — Unit tests (Moq, xUnit)  
- `BankingSolution.Tests.Integration` — Integration tests with in-memory SQLite  

---

## Requirements

To run the project locally, you will need:

- [.NET SDK 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)
- [Git](https://git-scm.com/)
- [EF Core CLI tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) (optional for migrations)

---

## Run Locally (Without Docker)

### 1. Clone the repository

```bash
git clone https://github.com/sanioooook/Banking-Solution.git
cd Banking-Solution/BankingSolutionBackend
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Apply database migrations

```bash
dotnet ef database update --project BankingSolution.Infrastructure --startup-project BankingSolution.WebApi
```

### 4. Run the API

```bash
dotnet run --project BankingSolution.WebApi
```

The API will be available at:

- `https://localhost:5001`
- `http://localhost:5000`

For test API:

- `https://localhost:5001/swagger`
- `http://localhost:5000/swagger`

---

## Run With Docker

Make sure Docker and Docker Compose are installed and running on your machine.

### Option 1: Using docker-compose

```bash
cd Banking-Solution
docker-compose up --build
```

This will build the image and start the application. The API will be available on port `5000`.

### Option 2: Manual Docker commands

```bash
cd Banking-Solution/BankingSolutionBackend

# Build the image
docker build -t banking-solution .

# Run the container
docker run -d -p 5000:80 --name banking-solution banking-solution
```

### Option 3: Using Prebuilt Image from GitHub Container Registry (docker-compose)

You can use the prebuilt image instead of building locally:

```bash
cd Banking-Solution
docker-compose -f docker-compose.yml -f docker-compose.override.pull.yml up
```
To do this, create a docker-compose.override.pull.yml file with:

```yaml
services:
  banking-solution:
    image: ghcr.io/sanioooook/banking-solution:latest
    ports:
      - "5000:80"
```

### Option 4: Using Prebuilt Image (docker run)

```bash
docker run -d -p 5000:80 ghcr.io/sanioooook/banking-solution:latest
```

---

## Running Tests

From the `BankingSolutionBackend` directory:

```bash
# Run unit tests
dotnet test BankingSolution.Tests.Unit

# Run integration tests
dotnet test BankingSolution.Tests.Integration
```

Both test projects are configured for .NET 9. Integration tests use in-memory SQLite.

---

## Technologies Used

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core (SQLite)
- CQRS via LiteBus
- FluentValidation
- Serilog (console and file)
- xUnit, Moq
- Docker, GitHub Actions

---

## CI/CD and Docker Publishing

The project includes a GitHub Actions workflow `.github/workflows/ci.yml` that:

- Runs unit and integration tests on push/pull to `master`
- Builds a Docker image and pushes it to GitHub Container Registry under:

```
ghcr.io/sanioooook/banking-solution
```

To publish a new image with version tag:

```bash
git tag v1.0.0
git push origin v1.0.0
```

If no tag is provided, the `latest` tag will be used.

---

## API Endpoints

Typical endpoints include:

- `GET /api/accounts`
- `GET /api/accounts/{id}`
- `POST /api/accounts`
- `PUT /api/accounts/{id}`
- `DELETE /api/accounts/{id}`
- `GET /api/transactions`
- `GET /api/transactions/account/{accountId}`
- `POST /api/transactions`

Refer to `BankingSolution.WebApi.Controllers` for full usage.

---

## Notes

- All integration tests run in parallel with isolated in-memory databases.
- Default database provider is SQLite.
- The system follows Clean Architecture principles.
- All dependencies and projects are managed using .NET 9 SDK.

---

## Repository

GitHub: [https://github.com/sanioooook/Banking-Solution](https://github.com/sanioooook/Banking-Solution)
