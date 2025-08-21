# Developer Store

A sales application where users can select customers and products. Discounts are applied based on product quantity. The system includes:

- Product management (create, update, delete)
- Customer management (create, update, delete)
- User management page (Admin-only)

---

## Tech Stack

- **Backend:** ASP.NET Core 8 Web API (Kestrel)
- **ORM:** Entity Framework Core (PostgreSQL)
- **Auth:** JWT (role-based, Admin-only endpoints)
- **Frontend:** Angular
- **Container:** Docker + docker-compose
- **Database:** PostgreSQL 15

---

## Architecture

### Adapters
- **Driven (Infrastructure):** `Ambev.DeveloperEvaluation.ORM`
- **Drivers (Web API):** `Ambev.DeveloperEvaluation.WebApi`

### Core
- **Application Layer:** `Ambev.DeveloperEvaluation.Application`
- **Domain Layer:** `Ambev.DeveloperEvaluation.Domain`

### Crosscutting
- `Ambev.DeveloperEvaluation.Common`
- `Ambev.DeveloperEvaluation.IoC`

### Tests
- **Functional:** `Ambev.DeveloperEvaluation.Functional`
- **Integration:** `Ambev.DeveloperEvaluation.Integration`
- **Unit:** `Ambev.DeveloperEvaluation.Unit`

---

## Setup Guide

### Prerequisites
- Install **Node.js** and **npm**
- Verify installation and version:
```bash
node -v
npm -v
```

### Running Frontend
From the frontend folder (`DeveloperStore/frontend`):
```bash
npm install
npm start
```
###User and password default 
```bash
User - admin@gmail.com
password - @dmin123
```
---

## Running with Docker

### Prerequisites
- Docker & Docker Compose installed
- Port **5001** (HTTPS) free
- `aspnetapp.pfx` certificate available in the project root (used for HTTPS)

### Step 1: Build and Start Containers
```bash
docker-compose up -d --build
```
This will start:
- PostgreSQL container (`postgres:15`)
- API container (`salesapi`)

API available at:  
- HTTPS: https://localhost:5001  

### Step 2: Access API & Swagger
- Swagger: [https://localhost:5001/swagger/index.html](https://localhost:5001/swagger/index.html)  
- API base: `https://localhost:5001/api/`  

### Step 3: Database Seeding
```csharp
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<DefaultContext>();
    ctx.Database.Migrate();
    await DefaultSeed.RunAsync(ctx);
    DbSeeder.EnsureSeeded(ctx);
}
```

### Step 4: Troubleshooting
- Ensure container exposes **5001**
- Validate certificate path
- Run:
```bash
docker ps
docker logs salesapi
```
If certificate issues occur, disable SSL verification temporarily (not recommended for production):
```bash
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

---

## Tests
Run automated tests with (`DeveloperStore/backend/tests`):
```bash
dotnet test
```
This runs unit, integration and functional tests.

---

## Author
Huxley Ruanis