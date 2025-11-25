# Step 1: Project Setup - Clean Architecture Inventory Management System

## Overview
We're building an Inventory Management System using Clean Architecture with:
- **Backend**: .NET 8 Web API with Clean Architecture
- **Frontend**: React with TypeScript
- **Patterns**: CQRS, Mediator (MediatR), Repository Pattern
- **Principles**: SOLID, DDD (Domain-Driven Design basics)

## Clean Architecture Layer Structure

```
InventoryManagement/
├── src/
│   ├── Core/
│   │   ├── InventoryManagement.Domain/          # Entities, Value Objects, Enums
│   │   └── InventoryManagement.Application/     # Business Logic, CQRS, Interfaces
│   ├── Infrastructure/
│   │   └── InventoryManagement.Infrastructure/  # Data Access, External Services
│   └── Presentation/
│       ├── InventoryManagement.API/             # Web API Controllers, Middleware
│       └── InventoryManagement.Web/             # React Frontend (separate)
└── tests/
    ├── InventoryManagement.Application.Tests/
    ├── InventoryManagement.Domain.Tests/
    └── InventoryManagement.API.Tests/
```

## Why This Structure?

### Layer Dependencies (Inside-Out):
1. **Domain** (Core) - No dependencies on anything
   - Contains: Entities, Value Objects, Domain Events, Exceptions
   - Purpose: Pure business rules and domain logic

2. **Application** (Core) - Depends only on Domain
   - Contains: Use Cases (CQRS Commands/Queries), DTOs, Interfaces, MediatR handlers
   - Purpose: Application business logic, orchestration

3. **Infrastructure** - Depends on Application & Domain
   - Contains: EF Core DbContext, Repositories, External API clients
   - Purpose: Technical implementations, database, external services

4. **API/Presentation** - Depends on Application (uses Infrastructure via DI)
   - Contains: Controllers, Middleware, Program.cs
   - Purpose: HTTP endpoints, request/response handling

**The Dependency Rule**: Inner layers don't know about outer layers. This makes the business logic independent of frameworks and databases.

---

## Task 1: Create Projects

### Create Project Structure:

#### 1. Create Domain Project (Class Library) inside src/Core folder
```
- Template: "Class Library"
- Name: InventoryManagement.Domain
- Framework: .NET 8.0
- Location: src/Core/InventoryManagement.Domain
```

**Delete the default Class1.cs file.**

#### 2. Create Application Project (Class Library) inside src/Core folder
```
- Template: "Class Library"
- Name: InventoryManagement.Application
- Framework: .NET 8.0
- Location: src/Core/InventoryManagement.Application
```

**Delete the default Class1.cs file.**

#### 3. Create Infrastructure Project (Class Library) inside src/Infrastructure folder
```
- Template: "Class Library"
- Name: InventoryManagement.Infrastructure
- Framework: .NET 8.0
- Location: src/Infrastructure/InventoryManagement.Infrastructure
```

**Delete the default Class1.cs file.**

#### 4. Create Web API Project inside src/Presentation
```
- Template: "ASP.NET Core Web API"
- Name: InventoryManagement.API
- Framework: .NET 8.0
- Location: src/Presentation/InventoryManagement.API
- Authentication: None
- Configure for HTTPS: Yes
- Enable OpenAPI support: Yes
- Use controllers: Yes (not minimal APIs)
- Enable Docker: No (for now)
```

#### 5. Create Test Projects inside tests folder

**Application Tests:**
```
- Template: "xUnit Test Project"
- Name: InventoryManagement.Application.Tests
- Framework: .NET 8.0
- Location: tests/InventoryManagement.Application.Tests
```

**Domain Tests:**
```
- Template: "xUnit Test Project"
- Name: InventoryManagement.Domain.Tests
- Framework: .NET 8.0
- Location: tests/InventoryManagement.Domain.Tests
```

**API Tests:**
```
- Template: "xUnit Test Project"
- Name: InventoryManagement.API.Tests
- Framework: .NET 8.0
- Location: tests/InventoryManagement.API.Tests
```

---

## Task 2: Set Up Project References

### Reference Chain (following Clean Architecture):

**InventoryManagement.Application** references:
- InventoryManagement.Domain

**InventoryManagement.Infrastructure** references:
- InventoryManagement.Application
- InventoryManagement.Domain

**InventoryManagement.API** references:
- InventoryManagement.Application
- InventoryManagement.Infrastructure

**Test Projects** reference their respective projects:
- Application.Tests → Application + Domain
- Domain.Tests → Domain
- API.Tests → API + Application + Infrastructure


---

## Task 3: Install NuGet Packages

### Domain Project Packages:
```
No packages needed initially - Domain should be pure C# with no external dependencies
```

### Application Project Packages:
```powershell
# Open Package Manager Console: Tools → NuGet Package Manager → Package Manager Console
# Set Default Project to: InventoryManagement.Application

Install-Package MediatR -Version 12.4.0
Install-Package FluentValidation -Version 11.9.2
Install-Package FluentValidation.DependencyInjectionExtensions -Version 11.9.2
Install-Package AutoMapper -Version 13.0.1
Install-Package Microsoft.Extensions.DependencyInjection.Abstractions -Version 8.0.0
```

**What these packages do:**
- **MediatR**: Implements the Mediator pattern for CQRS (Commands & Queries)
- **FluentValidation**: Validates commands/queries before processing
- **AutoMapper**: Maps between domain entities and DTOs
- **DependencyInjection.Abstractions**: For registering services

### Infrastructure Project Packages:
```powershell
# Set Default Project to: InventoryManagement.Infrastructure

Install-Package Microsoft.EntityFrameworkCore -Version 8.0.8
Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 8.0.8
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.8
Install-Package Microsoft.EntityFrameworkCore.Design -Version 8.0.8
Install-Package Microsoft.Extensions.Configuration -Version 8.0.0
Install-Package Microsoft.Extensions.Options.ConfigurationExtensions -Version 8.0.0
```

**What these packages do:**
- **EF Core**: ORM for database access
- **SqlServer**: SQL Server database provider
- **Tools & Design**: For migrations and database updates

### API Project Packages:
```powershell
# Set Default Project to: InventoryManagement.API

Install-Package Microsoft.EntityFrameworkCore.Design -Version 8.0.8
Install-Package Swashbuckle.AspNetCore -Version 6.7.3
Install-Package Microsoft.AspNetCore.Mvc.NewtonsoftJson -Version 8.0.8
```

**What these packages do:**
- **EF Core Design**: Required for running migrations from API project
- **Swashbuckle**: Swagger/OpenAPI documentation (usually pre-installed)
- **NewtonsoftJson**: Better JSON serialization options

### Test Projects Packages:
```powershell
# For each test project, install:

# Application.Tests
Install-Package xUnit -Version 2.9.0
Install-Package xUnit.runner.visualstudio -Version 2.8.2
Install-Package Moq -Version 4.20.70
Install-Package FluentAssertions -Version 6.12.0
Install-Package Microsoft.NET.Test.Sdk -Version 17.11.1

# Domain.Tests (same packages)
Install-Package xUnit -Version 2.9.0
Install-Package xUnit.runner.visualstudio -Version 2.8.2
Install-Package FluentAssertions -Version 6.12.0
Install-Package Microsoft.NET.Test.Sdk -Version 17.11.1

# API.Tests (additional packages)
Install-Package xUnit -Version 2.9.0
Install-Package xUnit.runner.visualstudio -Version 2.8.2
Install-Package Moq -Version 4.20.70
Install-Package FluentAssertions -Version 6.12.0
Install-Package Microsoft.NET.Test.Sdk -Version 17.11.1
Install-Package Microsoft.AspNetCore.Mvc.Testing -Version 8.0.8
```

**What these packages do:**
- **xUnit**: Testing framework
- **Moq**: Mocking library for unit tests
- **FluentAssertions**: Better assertion syntax
- **AspNetCore.Mvc.Testing**: Integration testing for API

---

## Task 5: Create Folder Structure

### In Domain Project, create folders:
```
InventoryManagement.Domain/
├── Entities/
├── ValueObjects/
├── Enums/
├── Exceptions/
└── Events/
```

### In Application Project, create folders:
```
InventoryManagement.Application/
├── Common/
│   ├── Interfaces/
│   ├── Behaviors/
│   ├── Mappings/
│   └── Models/
├── Features/
│   └── (we'll create feature folders later)
└── DependencyInjection.cs (we'll create this file next)
```

### In Infrastructure Project, create folders:
```
InventoryManagement.Infrastructure/
├── Data/
│   ├── Configurations/
│   └── Repositories/
└── DependencyInjection.cs (we'll create this file next)
```