# Developer Store

A sales application where users can select customers and products. Discounts are applied based on product quantity. The system includes:

- Auth User
- Sales management (create, get, cancel, cancelItem)
- Product management (create, get, update, delete)
- Customer management (create, get, update, delete)
- User management page (get) (Admin-only)

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
- **Application Shared** `Ambev.DeveloperEvaluation.Common`
- **Inversion of Control** `Ambev.DeveloperEvaluation.IoC`

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
cd DeveloperStore/frontend
node -v
npm -v
```

### Running Frontend
From the frontend folder (`DeveloperStore/frontend`):
```bash
cd DeveloperStore/frontend
npm install
npm start
```
### User and password default 
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

### Step 1: Build and Start Containers (`DeveloperStore/backend`):
```bash
cd DeveloperStore/backend
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

### Step 3: Database Migrations
**Important:** To apply migrations, you must run the following command **inside the WebApi project** following the current structure:

```bash
cd DeveloperStore/backend/src/Ambev.DeveloperEvaluation.WebApi
dotnet ef database update 
```

### Step 4: Database Seeding
```csharp
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<DefaultContext>();
    ctx.Database.Migrate();
    await DefaultSeed.RunAsync(ctx);
    DbSeeder.EnsureSeeded(ctx);
}
```

### Step 5: Troubleshooting
- Ensure container exposes **5001**
- Validate certificate path
- Run:
```bash
docker ps
docker logs salesapi
```
If certificate issues occur, disable SSL verification temporarily:
```bash
cd DeveloperStore/backend
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

---

## Tests
Run automated tests with (`DeveloperStore/backend/tests`):
```bash
DeveloperStore/backend/tests/Ambev.DeveloperEvaluation.Functional
dotnet test

DeveloperStore/backend/tests/Ambev.DeveloperEvaluation.Integration
dotnet test

DeveloperStore/backend/tests/Ambev.DeveloperEvaluation.Unit
dotnet test
```
This runs unit, integration and functional tests.

---

## Author
Huxley Ruanis
