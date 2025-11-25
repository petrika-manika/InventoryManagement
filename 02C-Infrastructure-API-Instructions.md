# Step 2C: Infrastructure & API Layer - User Management Implementation
## Instructions for GitHub Copilot

---

## Overview
This document provides instructions for building the **Infrastructure** and **API/Presentation** layers. These are the outer layers that connect everything together: database, external services, and HTTP endpoints.

**Important**: This file contains instructions for GitHub Copilot to generate code. The Infrastructure layer implements interfaces from Application, and the API layer exposes functionality via HTTP.

---

## Understanding Infrastructure and API Layers

### What is the Infrastructure Layer?
- **Implements interfaces** from Application layer (Dependency Inversion)
- **Database access** - Entity Framework Core DbContext
- **External services** - Password hashing, JWT generation
- **Framework code** - EF Core configurations, migrations

### What is the API/Presentation Layer?
- **HTTP endpoints** - REST API controllers
- **Authentication** - JWT middleware
- **Error handling** - Global exception middleware
- **API documentation** - Swagger/OpenAPI
- **Entry point** - Program.cs configuration

### Dependency Flow (Inside-Out):
```
Domain (no dependencies)
   ↑
Application (depends on Domain)
   ↑
Infrastructure (depends on Application + Domain)
   ↑
API (depends on Infrastructure + Application)
```

---

## Project Structure

```
InventoryManagement.Infrastructure/
├── Data/
│   ├── ApplicationDbContext.cs
│   ├── ApplicationDbContextSeed.cs
│   └── Configurations/
│       └── UserConfiguration.cs
├── Services/
│   ├── PasswordHasher.cs
│   ├── JwtTokenGenerator.cs
│   └── CurrentUserService.cs
└── DependencyInjection.cs

InventoryManagement.API/
├── Controllers/
│   ├── AuthController.cs
│   └── UsersController.cs
├── Middleware/
│   └── ExceptionHandlingMiddleware.cs
├── Program.cs
└── appsettings.json
```

---

## Task 1: Install Additional NuGet Packages

### Infrastructure Project - Password Hashing

Open NuGet Package Manager Console and run:

```powershell
# Set Default Project to: InventoryManagement.Infrastructure
Install-Package BCrypt.Net-Next -Version 4.0.3
```

**What is BCrypt?** Industry-standard password hashing algorithm (secure, slow on purpose to prevent brute-force attacks)

### API Project - JWT Authentication

```powershell
# Set Default Project to: InventoryManagement.API
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer -Version 8.0.8
```

**What is JWT Bearer?** Middleware for validating JWT tokens in API requests

---

## Task 2: Create DbContext and Entity Configuration

### Task 2.1: ApplicationDbContext

**Location**: `InventoryManagement.Infrastructure/Data/ApplicationDbContext.cs`

**Prompt for Copilot:**

> Create ApplicationDbContext class in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Infrastructure.Data`
> 
> **Requirements**:
> - Public sealed class
> - Imports: `using InventoryManagement.Application.Common.Interfaces;`, `using InventoryManagement.Domain.Entities;`, `using Microsoft.EntityFrameworkCore;`, `using System.Reflection;`
> - Inherits: `DbContext` and implements `IApplicationDbContext`
> 
> **Constructor**:
> - Parameter: `DbContextOptions<ApplicationDbContext> options`
> - Pass options to base constructor
> 
> **Properties**:
> - `public DbSet<User> Users => Set<User>();` (implements interface)
> 
> **OnModelCreating** method (override):
> - Call base.OnModelCreating
> - Apply all entity configurations from current assembly: `modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());`
> 
> **SaveChangesAsync** method (override):
> - Just call base.SaveChangesAsync and return result
> 
> Add XML documentation comment explaining this is EF Core DbContext implementing IApplicationDbContext

### Task 2.2: User Entity Configuration

**Location**: `InventoryManagement.Infrastructure/Data/Configurations/UserConfiguration.cs`

**Prompt for Copilot:**

> Create UserConfiguration class for EF Core following these specifications:
> 
> **Namespace**: `InventoryManagement.Infrastructure.Data.Configurations`
> 
> **Requirements**:
> - Public sealed class
> - Imports: Domain entities, ValueObjects, Microsoft.EntityFrameworkCore, Microsoft.EntityFrameworkCore.Metadata.Builders
> - Implements: `IEntityTypeConfiguration<User>`
> 
> **Configure Method** - Sets up database schema:
> 
> 1. **Table name**: `builder.ToTable("Users");`
> 
> 2. **Primary key**: `builder.HasKey(u => u.Id);`
> 
> 3. **FirstName property**:
>    - Required
>    - MaxLength 100
> 
> 4. **LastName property**:
>    - Required
>    - MaxLength 100
> 
> 5. **Email property** (Value Object):
>    - Required
>    - MaxLength 255
>    - HasConversion: `email => email.Value` (to database), `value => Email.Create(value)` (from database)
>    - HasIndex: Unique index on Email, database name "IX_Users_Email"
> 
> 6. **PasswordHash property**:
>    - Required
>    - MaxLength 500
> 
> 7. **IsActive property**:
>    - Required
>    - HasDefaultValue true
> 
> 8. **CreatedAt property**:
>    - Required
> 
> 9. **UpdatedAt property**:
>    - Required
> 
> 10. **FullName property**:
>     - Ignore (computed property, not stored in database)
> 
> Add XML documentation explaining this configures User entity for EF Core using Fluent API

### Expected Outcome
- ✅ ApplicationDbContext created implementing IApplicationDbContext
- ✅ UserConfiguration created with EF Core mappings
- ✅ Email value object properly mapped to/from database

---

## Task 3: Create Service Implementations

Now we implement the interfaces from Application layer.

### Task 3.1: PasswordHasher Service

**Location**: `InventoryManagement.Infrastructure/Services/PasswordHasher.cs`

**Prompt for Copilot:**

> Create PasswordHasher service class following these specifications:
> 
> **Namespace**: `InventoryManagement.Infrastructure.Services`
> 
> **Requirements**:
> - Public sealed class
> - Import: `using InventoryManagement.Application.Common.Interfaces;`
> - Implements: `IPasswordHasher`
> 
> **Methods**:
> 
> 1. **HashPassword**:
>    - Uses: `BCrypt.Net.BCrypt.HashPassword(password)` to hash password
>    - Returns hashed string
> 
> 2. **VerifyPassword**:
>    - Uses: `BCrypt.Net.BCrypt.Verify(password, passwordHash)` to verify
>    - Returns boolean
> 
> Add XML documentation explaining BCrypt is used for secure password hashing

### Task 3.2: JwtTokenGenerator Service

**Location**: `InventoryManagement.Infrastructure/Services/JwtTokenGenerator.cs`

**Prompt for Copilot:**

> Create JwtTokenGenerator service class following these specifications:
> 
> **Namespace**: `InventoryManagement.Infrastructure.Services`
> 
> **Requirements**:
> - Public sealed class
> - Imports: `using System.IdentityModel.Tokens.Jwt;`, `using System.Security.Claims;`, `using System.Text;`, Application interfaces, Domain entities, `using Microsoft.Extensions.Configuration;`, `using Microsoft.IdentityModel.Tokens;`
> - Implements: `IJwtTokenGenerator`
> 
> **Constructor**:
> - Inject: `IConfiguration configuration`
> 
> **GenerateToken Method**:
> Parameter: `User user`
> 
> Logic:
> 1. Create SymmetricSecurityKey from configuration["Jwt:Secret"] encoded as UTF8
> 2. Create SigningCredentials with key and HmacSha256 algorithm
> 3. Create array of claims:
>    - Sub: user.Id.ToString()
>    - Email: user.Email.Value
>    - GivenName: user.FirstName
>    - FamilyName: user.LastName
>    - Jti: new Guid
>    Use JwtRegisteredClaimNames constants
> 4. Create JwtSecurityToken with:
>    - issuer from configuration["Jwt:Issuer"]
>    - audience from configuration["Jwt:Audience"]
>    - claims array
>    - expires: DateTime.UtcNow.AddHours(24)
>    - signingCredentials
> 5. Return new JwtSecurityTokenHandler().WriteToken(token)
> 
> Add XML documentation explaining JWT token generation

### Task 3.3: CurrentUserService

**Location**: `InventoryManagement.Infrastructure/Services/CurrentUserService.cs`

**Prompt for Copilot:**

> Create CurrentUserService class following these specifications:
> 
> **Namespace**: `InventoryManagement.Infrastructure.Services`
> 
> **Requirements**:
> - Public sealed class
> - Imports: `using System.Security.Claims;`, Application interfaces, `using Microsoft.AspNetCore.Http;`
> - Implements: `ICurrentUserService`
> 
> **Constructor**:
> - Inject: `IHttpContextAccessor httpContextAccessor`
> 
> **Properties**:
> 
> 1. **UserId** (Guid?):
>    - Get userIdClaim from _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
>    - TryParse to Guid
>    - Return Guid if successful, null otherwise
> 
> 2. **Email** (string?):
>    - Return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email)
> 
> 3. **IsAuthenticated** (bool):
>    - Return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false
> 
> Add XML documentation explaining this gets current user from HTTP context

### Expected Outcome
- ✅ PasswordHasher implemented using BCrypt
- ✅ JwtTokenGenerator implemented
- ✅ CurrentUserService implemented

---

## Task 4: Create Database Seed Data

This creates the default admin user when database is created.

**Location**: `InventoryManagement.Infrastructure/Data/ApplicationDbContextSeed.cs`

**Prompt for Copilot:**

> Create ApplicationDbContextSeed static class following these specifications:
> 
> **Namespace**: `InventoryManagement.Infrastructure.Data`
> 
> **Requirements**:
> - Public static class
> - Imports: Application interfaces, Domain entities and value objects, Microsoft.EntityFrameworkCore
> 
> **Method** - `SeedDefaultUserAsync`:
> - Parameters: `ApplicationDbContext context`, `IPasswordHasher passwordHasher`
> - Returns: `Task`
> - Async method
> 
> Logic:
> 1. Create Email for admin: `Email.Create("admin@inventoryapp.com")`
> 2. Check if admin exists: `await context.Users.AnyAsync(u => u.Email == adminEmail)`
> 3. If admin doesn't exist:
>    a. Create admin user with User.Create:
>       - FirstName: "System"
>       - LastName: "Administrator"
>       - Email: adminEmail
>       - PasswordHash: passwordHasher.HashPassword("Admin@123")
>    b. Add admin to context.Users
>    c. Call context.SaveChangesAsync()
> 
> Add XML documentation explaining this seeds initial admin user into database

### Expected Outcome
- ✅ Seed data method created
- ✅ Default admin user will be created on first run
- ✅ Credentials: admin@inventoryapp.com / Admin@123

---

## Task 5: Create Infrastructure Dependency Injection

**Location**: `InventoryManagement.Infrastructure/DependencyInjection.cs`

**Prompt for Copilot:**

> Create Infrastructure DependencyInjection class following these specifications:
> 
> **Namespace**: `InventoryManagement.Infrastructure`
> 
> **Requirements**:
> - Public static class
> - Imports: Application interfaces, Infrastructure data and services, Microsoft.EntityFrameworkCore, Microsoft.Extensions.Configuration, Microsoft.Extensions.DependencyInjection
> 
> **Extension Method** - `AddInfrastructure`:
> - Returns: `IServiceCollection`
> - Parameters: `this IServiceCollection services`, `IConfiguration configuration`
> 
> Logic:
> 1. Register DbContext:
>    ```
>    services.AddDbContext<ApplicationDbContext>(options =>
>        options.UseSqlServer(
>            configuration.GetConnectionString("DefaultConnection"),
>            b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
>    ```
> 
> 2. Register IApplicationDbContext:
>    ```
>    services.AddScoped<IApplicationDbContext>(provider =>
>        provider.GetRequiredService<ApplicationDbContext>());
>    ```
> 
> 3. Register services as Scoped:
>    - IPasswordHasher → PasswordHasher
>    - IJwtTokenGenerator → JwtTokenGenerator
>    - ICurrentUserService → CurrentUserService
> 
> 4. Register HttpContextAccessor: `services.AddHttpContextAccessor();`
> 
> 5. Return services
> 
> Add XML documentation explaining this registers all Infrastructure services

### Expected Outcome
- ✅ Infrastructure services registered
- ✅ DbContext configured for SQL Server
- ✅ All interfaces have implementations

---

## Task 6: Configure API - appsettings.json

**Location**: `InventoryManagement.API/appsettings.json`

**Instructions**: Manually edit or prompt Copilot to create JSON configuration with:

**Connection String**:
- Name: "DefaultConnection"
- Value: `"Server=(localdb)\\mssqllocaldb;Database=InventoryManagementDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"`

**JWT Settings**:
- Secret: `"YourSuperSecretKeyThatIsAtLeast32CharactersLong!"` (must be at least 32 characters)
- Issuer: `"InventoryManagementAPI"`
- Audience: `"InventoryManagementClient"`

**Logging**:
- Default: Information
- Microsoft.AspNetCore: Warning

**AllowedHosts**: `"*"`

**IMPORTANT**: In production, store JWT Secret in Azure Key Vault or environment variables, never in source control!

---

## Task 7: Configure API - Program.cs

**Location**: `InventoryManagement.API/Program.cs`

**Prompt for Copilot:**

> Create Program.cs for ASP.NET Core Web API following these specifications:
> 
> **Setup**:
> - Create WebApplicationBuilder
> - Import namespaces: Application, Infrastructure, API Middleware, System.Text, Microsoft.AspNetCore.Authentication.JwtBearer, Microsoft.IdentityModel.Tokens, Microsoft.OpenApi.Models
> 
> **Services to Register**:
> 
> 1. **Controllers**: `builder.Services.AddControllers();`
> 
> 2. **Application services**: `builder.Services.AddApplication();`
> 
> 3. **Infrastructure services**: `builder.Services.AddInfrastructure(builder.Configuration);`
> 
> 4. **JWT Authentication**:
>    ```
>    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
>        .AddJwtBearer(options => {
>            options.TokenValidationParameters = new TokenValidationParameters {
>                ValidateIssuer = true,
>                ValidateAudience = true,
>                ValidateLifetime = true,
>                ValidateIssuerSigningKey = true,
>                ValidIssuer = builder.Configuration["Jwt:Issuer"],
>                ValidAudience = builder.Configuration["Jwt:Audience"],
>                IssuerSigningKey = new SymmetricSecurityKey(
>                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
>            };
>        });
>    ```
> 
> 5. **Authorization**: `builder.Services.AddAuthorization();`
> 
> 6. **Swagger** with JWT support:
>    - AddEndpointsApiExplorer
>    - AddSwaggerGen with:
>      - SwaggerDoc v1 with Title, Version, Description
>      - AddSecurityDefinition "Bearer" with JWT scheme
>      - AddSecurityRequirement for Bearer token
> 
> 7. **CORS Policy** named "AllowReactApp":
>    - Origins: "http://localhost:3000", "http://localhost:5173"
>    - AllowAnyHeader
>    - AllowAnyMethod
>    - AllowCredentials
> 
> **Middleware Pipeline** (after app is built):
> 
> 1. **Seed Database**:
>    - Create scope
>    - Get ApplicationDbContext and IPasswordHasher from services
>    - Call ApplicationDbContextSeed.SeedDefaultUserAsync
> 
> 2. **Development Only**:
>    - UseSwagger
>    - UseSwaggerUI
> 
> 3. **All Environments**:
>    - UseHttpsRedirection
>    - UseCors("AllowReactApp")
>    - UseMiddleware<ExceptionHandlingMiddleware>() (global exception handler)
>    - UseAuthentication
>    - UseAuthorization
>    - MapControllers
> 
> 4. **Run**: app.Run()
> 
> 5. **Add**: `public partial class Program { }` for integration tests
> 
> Add comments explaining each section

### Expected Outcome
- ✅ All services registered
- ✅ JWT authentication configured
- ✅ Swagger with JWT support
- ✅ CORS enabled for React app
- ✅ Database seeded on startup
- ✅ Global exception handling

---

## Task 8: Create Global Exception Handling Middleware

**Location**: `InventoryManagement.API/Middleware/ExceptionHandlingMiddleware.cs`

**Prompt for Copilot:**

> Create ExceptionHandlingMiddleware class following these specifications:
> 
> **Namespace**: `InventoryManagement.API.Middleware`
> 
> **Requirements**:
> - Public sealed class
> - Imports: System.Net, System.Text.Json, FluentValidation, Domain.Exceptions
> 
> **Constructor**:
> - Inject: `RequestDelegate next`, `ILogger<ExceptionHandlingMiddleware> logger`
> 
> **InvokeAsync Method**:
> - Parameter: `HttpContext context`
> - Try-catch wrapper:
>   - Try: await _next(context)
>   - Catch: Log error, call HandleExceptionAsync
> 
> **HandleExceptionAsync Method** (private static):
> - Parameter: `HttpContext context`, `Exception exception`
> - Set response content type to "application/json"
> - Use switch expression to map exceptions to status codes and messages:
>   - ValidationException → 400 BadRequest with errors array
>   - UserNotFoundException → 404 NotFound
>   - DuplicateEmailException → 409 Conflict
>   - InvalidCredentialsException → 401 Unauthorized
>   - UnauthorizedAccessException → 401 Unauthorized
>   - InvalidOperationException → 400 BadRequest
>   - DomainException → 400 BadRequest
>   - Default → 500 InternalServerError with generic message
> - Return anonymous object with: statusCode, message, errors (for validation)
> - Set response.StatusCode
> - Serialize to JSON with camelCase naming
> - Write to response
> 
> Add XML documentation explaining global exception handling for consistent error responses

### Expected Outcome
- ✅ All exceptions handled globally
- ✅ Consistent error response format
- ✅ Appropriate HTTP status codes
- ✅ Detailed validation errors

---

## Task 9: Create API Controllers

Controllers are thin - they just send commands/queries to MediatR.

### Task 9.1: AuthController

**Location**: `InventoryManagement.API/Controllers/AuthController.cs`

**Prompt for Copilot:**

> Create AuthController class following these specifications:
> 
> **Namespace**: `InventoryManagement.API.Controllers`
> 
> **Requirements**:
> - ApiController attribute
> - Route attribute: "api/[controller]"
> - Public class
> - Imports: LoginUserCommand, MediatR, Microsoft.AspNetCore.Mvc
> 
> **Constructor**:
> - Inject: `IMediator mediator`
> 
> **Endpoints**:
> 
> 1. **Login** (HttpPost "login"):
>    - Parameter: `[FromBody] LoginUserCommand command`
>    - ProducesResponseType: 200 OK with object
>    - ProducesResponseType: 401 Unauthorized
>    - Logic:
>      - var result = await _mediator.Send(command);
>      - return Ok(result);
>    - Add XML documentation comment
> 
> Add class-level XML documentation explaining these are authentication endpoints

### Task 9.2: UsersController

**Location**: `InventoryManagement.API/Controllers/UsersController.cs`

**Prompt for Copilot:**

> Create UsersController class following these specifications:
> 
> **Namespace**: `InventoryManagement.API.Controllers`
> 
> **Requirements**:
> - Authorize attribute (all endpoints require authentication)
> - ApiController attribute
> - Route attribute: "api/[controller]"
> - Public class
> - Imports: All user commands and queries, MediatR, Microsoft.AspNetCore.Authorization, Microsoft.AspNetCore.Mvc
> 
> **Constructor**:
> - Inject: `IMediator mediator`
> 
> **Endpoints**:
> 
> 1. **GetAll** (HttpGet):
>    - ProducesResponseType: 200 with List<object>
>    - Send: new GetAllUsersQuery()
>    - Return Ok(result)
> 
> 2. **GetById** (HttpGet "{id:guid}"):
>    - Parameter: Guid id
>    - ProducesResponseType: 200, 404
>    - Send: new GetUserByIdQuery(id)
>    - Return Ok(result)
> 
> 3. **GetCurrentUser** (HttpGet "me"):
>    - ProducesResponseType: 200
>    - Send: new GetCurrentUserQuery()
>    - Return Ok(result)
> 
> 4. **Create** (HttpPost):
>    - Parameter: [FromBody] CreateUserCommand command
>    - ProducesResponseType: 201 Created with Guid, 400
>    - Send command
>    - Return CreatedAtAction(nameof(GetById), new { id = userId }, userId)
> 
> 5. **Update** (HttpPut "{id:guid}"):
>    - Parameters: Guid id, [FromBody] UpdateUserCommand command
>    - ProducesResponseType: 204 NoContent, 404
>    - Validate: if (id != command.UserId) return BadRequest("ID mismatch")
>    - Send command
>    - Return NoContent()
> 
> 6. **Activate** (HttpPatch "{id:guid}/activate"):
>    - Parameter: Guid id
>    - ProducesResponseType: 204, 404
>    - Send: new ActivateUserCommand(id)
>    - Return NoContent()
> 
> 7. **Deactivate** (HttpPatch "{id:guid}/deactivate"):
>    - Parameter: Guid id
>    - ProducesResponseType: 204, 404
>    - Send: new DeactivateUserCommand(id)
>    - Return NoContent()
> 
> Add XML documentation comments for all endpoints explaining what they do
> 
> Add class-level XML documentation explaining these are user management endpoints

### Expected Outcome
- ✅ AuthController with Login endpoint
- ✅ UsersController with 7 endpoints
- ✅ All endpoints use MediatR (thin controllers)
- ✅ Proper HTTP verbs and status codes

---

## Task 10: Create and Run Database Migration

Now we create the database schema and apply it.

### Step-by-Step Instructions:

1. **Set Startup Project**:
   - Right-click `InventoryManagement.API` in Solution Explorer
   - Select "Set as Startup Project"

2. **Open Package Manager Console**:
   - Tools → NuGet Package Manager → Package Manager Console

3. **Set Default Project**:
   - In console dropdown, select `InventoryManagement.Infrastructure`

4. **Create Migration**:
   ```powershell
   Add-Migration InitialCreate
   ```

5. **Review Migration**:
   - Check the generated migration file in Infrastructure/Migrations folder
   - Should create Users table with all columns and indexes

6. **Apply Migration**:
   ```powershell
   Update-Database
   ```

7. **Verify**:
   - Database should be created
   - Users table should exist
   - Admin user should be seeded

### What Happens:
- ✅ Creates database if doesn't exist
- ✅ Creates Users table
- ✅ Creates unique index on Email
- ✅ Seeds admin user (admin@inventoryapp.com / Admin@123)

---

## Task 11: Test the API

### Run the Application:

1. Press **F5** or click "Run" in Visual Studio
2. API should start and open Swagger UI
3. URL: `https://localhost:7xxx/swagger` (port varies)

### Test Scenarios:

#### 1. Test Login (No Authentication Required):

**Swagger UI**:
- Expand `POST /api/auth/login`
- Click "Try it out"
- Request body:
  ```json
  {
    "email": "admin@inventoryapp.com",
    "password": "Admin@123"
  }
  ```
- Click "Execute"

**Expected Response** (200 OK):
```json
{
  "user": {
    "id": "guid-here",
    "firstName": "System",
    "lastName": "Administrator",
    "fullName": "System Administrator",
    "email": "admin@inventoryapp.com",
    "isActive": true,
    "createdAt": "2024-11-19T...",
    "updatedAt": "2024-11-19T..."
  },
  "token": "eyJhbGc..."
}
```

#### 2. Authorize Swagger with Token:

- Copy the `token` value from login response
- Click "Authorize" button (🔓 icon) at top right of Swagger UI
- In the dialog, enter: `Bearer your-token-here` (include "Bearer " prefix)
- Click "Authorize"
- Lock icon should now be closed (🔒)

#### 3. Test Get All Users (Requires Authentication):

- Expand `GET /api/users`
- Click "Try it out"
- Click "Execute"

**Expected Response** (200 OK):
```json
[
  {
    "id": "guid-here",
    "firstName": "System",
    "lastName": "Administrator",
    "fullName": "System Administrator",
    "email": "admin@inventoryapp.com",
    "isActive": true,
    "createdAt": "...",
    "updatedAt": "..."
  }
]
```

#### 4. Test Get Current User:

- Expand `GET /api/users/me`
- Click "Try it out"
- Click "Execute"

**Expected**: Same as above, returns currently logged-in user

#### 5. Test Create User:

- Expand `POST /api/users`
- Click "Try it out"
- Request body:
  ```json
  {
    "firstName": "John",
    "lastName": "Doe",
    "email": "john.doe@test.com",
    "password": "Password123"
  }
  ```
- Click "Execute"

**Expected Response** (201 Created):
- Returns new user's Guid
- Location header with URL to get the user

#### 6. Test Get User by ID:

- Copy the Guid from create response
- Expand `GET /api/users/{id}`
- Click "Try it out"
- Paste Guid in id field
- Click "Execute"

**Expected**: Returns John Doe's user details

#### 7. Test Update User:

- Expand `PUT /api/users/{id}`
- Click "Try it out"
- Enter John Doe's ID
- Request body:
  ```json
  {
    "userId": "paste-john-doe-guid-here",
    "firstName": "Jane",
    "lastName": "Smith",
    "email": "jane.smith@test.com"
  }
  ```
- Click "Execute"

**Expected**: 204 No Content (success)

#### 8. Test Deactivate User:

- Expand `PATCH /api/users/{id}/deactivate`
- Enter John Doe's ID
- Click "Execute"

**Expected**: 204 No Content
- Get user again - should show isActive: false

#### 9. Test Activate User:

- Expand `PATCH /api/users/{id}/activate`
- Enter John Doe's ID
- Click "Execute"

**Expected**: 204 No Content
- Get user again - should show isActive: true

### Testing Error Scenarios:

1. **Invalid Credentials**:
   - Try login with wrong password
   - Expected: 401 Unauthorized

2. **Duplicate Email**:
   - Try creating user with existing email
   - Expected: 409 Conflict

3. **Invalid Email Format**:
   - Try creating user with "invalid-email"
   - Expected: 400 Bad Request with validation errors

4. **Unauthorized Request**:
   - Click "Authorize" → Logout
   - Try GET /api/users
   - Expected: 401 Unauthorized

---

## Task 12: Create Integration Tests

Integration tests verify the entire API works end-to-end.

### Task 12.1: AuthController Integration Tests

**Location**: `InventoryManagement.API.Tests/IntegrationTests/AuthControllerTests.cs`

**Prompt for Copilot:**

> Create integration tests for AuthController using xUnit and WebApplicationFactory following these specifications:
> 
> **Namespace**: `InventoryManagement.API.Tests.IntegrationTests`
> 
> **Requirements**:
> - Public class
> - Imports: System.Net, System.Net.Http.Json, FluentAssertions, Application.Common.Models, Application.Features.Users.Commands.LoginUser, Microsoft.AspNetCore.Mvc.Testing, Xunit
> - Implements: `IClassFixture<WebApplicationFactory<Program>>`
> 
> **Constructor**:
> - Parameter: `WebApplicationFactory<Program> factory`
> - Create: `_client = factory.CreateClient();`
> 
> **Test Methods**:
> 
> 1. **Login_WithValidCredentials_ShouldReturnToken**:
>    - Arrange: LoginUserCommand with admin credentials
>    - Act: PostAsJsonAsync to "/api/auth/login"
>    - Assert: StatusCode is OK, result not null, token not empty, user email matches
> 
> 2. **Login_WithInvalidCredentials_ShouldReturnUnauthorized**:
>    - Arrange: LoginUserCommand with wrong password
>    - Act: PostAsJsonAsync
>    - Assert: StatusCode is Unauthorized
> 
> 3. **Login_WithInvalidEmail_ShouldReturnBadRequest**:
>    - Arrange: LoginUserCommand with invalid email format
>    - Assert: StatusCode is BadRequest (validation error)
> 
> Use FluentAssertions for all assertions

### Task 12.2: UsersController Integration Tests

**Location**: `InventoryManagement.API.Tests/IntegrationTests/UsersControllerTests.cs`

**Prompt for Copilot:**

> Create integration tests for UsersController with these test methods:
> 
> **Namespace**: `InventoryManagement.API.Tests.IntegrationTests`
> 
> **Setup**:
> - IClassFixture<WebApplicationFactory<Program>>
> - Helper method to get JWT token (login as admin)
> - Helper method to add Authorization header to HttpClient
> 
> **Test Methods**:
> 
> 1. **GetAll_WithAuthentication_ShouldReturnUsers**:
>    - Arrange: Get token, set authorization header
>    - Act: GET /api/users
>    - Assert: 200 OK, list not empty
> 
> 2. **GetAll_WithoutAuthentication_ShouldReturnUnauthorized**:
>    - Act: GET /api/users (no token)
>    - Assert: 401 Unauthorized
> 
> 3. **Create_WithValidData_ShouldCreateUser**:
>    - Arrange: Get token, create command
>    - Act: POST /api/users
>    - Assert: 201 Created, returns Guid
> 
> 4. **Create_WithDuplicateEmail_ShouldReturnConflict**:
>    - Arrange: Try to create user with admin email
>    - Assert: 409 Conflict
> 
> 5. **GetById_WithValidId_ShouldReturnUser**:
>    - Arrange: Create user, get ID
>    - Act: GET /api/users/{id}
>    - Assert: 200 OK, user details correct
> 
> 6. **Update_WithValidData_ShouldUpdateUser**:
>    - Arrange: Create user, prepare update command
>    - Act: PUT /api/users/{id}
>    - Assert: 204 NoContent
> 
> Use FluentAssertions

### Expected Outcome
- ✅ Integration tests verify entire API flow
- ✅ Tests authenticate and make actual HTTP requests
- ✅ All tests passing

### Run Integration Tests:
1. Build solution
2. Open Test Explorer
3. Run all tests
4. All should pass ✅ (Domain + Application + Integration)

---

## Verification Checklist

Before considering User Management complete, verify:

- [ ] All NuGet packages installed
- [ ] ApplicationDbContext created and configured
- [ ] User entity configuration with EF Core
- [ ] All service implementations created (PasswordHasher, JwtTokenGenerator, CurrentUserService)
- [ ] Database seed method created
- [ ] Infrastructure DependencyInjection configured
- [ ] appsettings.json configured (connection string, JWT settings)
- [ ] Program.cs fully configured (services, middleware, authentication)
- [ ] Exception handling middleware created
- [ ] AuthController created
- [ ] UsersController created with all 7 endpoints
- [ ] Database migration created and applied
- [ ] Admin user seeded successfully
- [ ] Can login and get JWT token
- [ ] All authenticated endpoints work with token
- [ ] All CRUD operations work
- [ ] Integration tests created and passing
- [ ] All unit tests (Domain + Application) still passing

---

## What You've Accomplished

🎉 **Complete User Management Module!**

You now have:

✅ **Clean Architecture** - Proper layer separation  
✅ **CQRS with MediatR** - Commands and queries separated  
✅ **JWT Authentication** - Secure token-based auth  
✅ **Entity Framework Core** - Database access with migrations  
✅ **RESTful API** - 8 endpoints (1 auth + 7 user management)  
✅ **Global Exception Handling** - Consistent error responses  
✅ **Validation** - FluentValidation with pipeline behavior  
✅ **Unit Tests** - Domain and Application layers  
✅ **Integration Tests** - Full API endpoints  
✅ **Swagger Documentation** - Interactive API docs with JWT support  
✅ **SOLID Principles** - Throughout all layers  

---

## API Endpoints Summary

### Authentication (No Auth Required):
- `POST /api/auth/login` - Login with email/password → Returns JWT token

### User Management (Requires JWT Token):
- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get user by ID
- `GET /api/users/me` - Get current authenticated user
- `POST /api/users` - Create new user
- `PUT /api/users/{id}` - Update user information
- `PATCH /api/users/{id}/activate` - Activate user account
- `PATCH /api/users/{id}/deactivate` - Deactivate user account

### Business Rules:
✅ Logged-in users can edit their own information  
✅ Logged-in users can edit other users' information  
✅ Logged-in users can activate/deactivate any user (including themselves)  
✅ Duplicate emails are prevented  
✅ Inactive users cannot login  
✅ All endpoints except login require authentication  

---

## Common Issues and Solutions

**Issue**: "Cannot connect to database"
- Solution: Ensure SQL Server is running, check connection string

**Issue**: "Migration fails"
- Solution: Make sure Infrastructure is default project in Package Manager Console, API is startup project

**Issue**: "401 Unauthorized on all endpoints"
- Solution: Make sure JWT token is in Authorization header with "Bearer " prefix

**Issue**: "Swagger shows lock icon"
- Solution: Click Authorize button, enter `Bearer your-token`, click Authorize

**Issue**: "Admin user not seeded"
- Solution: Check Program.cs seed section, verify it runs, check database

**Issue**: "Token validation fails"
- Solution: Verify JWT Secret is at least 32 characters, check Issuer/Audience match

**Issue**: "CORS errors from React"
- Solution: Verify CORS policy includes React's origin, UseCors before UseAuthorization

---

## Testing with Postman

If you prefer Postman over Swagger:

### 1. Login:
```
POST https://localhost:7xxx/api/auth/login
Content-Type: application/json

{
  "email": "admin@inventoryapp.com",
  "password": "Admin@123"
}
```

### 2. Save Token:
- Copy token from response
- In Postman, create environment variable: `jwt_token`

### 3. Authenticated Requests:
```
GET https://localhost:7xxx/api/users
Authorization: Bearer {{jwt_token}}
```

---

## Production Checklist

Before deploying to production:

- [ ] Move JWT Secret to Azure Key Vault / environment variable
- [ ] Use real SQL Server (not LocalDB)
- [ ] Enable HTTPS only
- [ ] Configure proper CORS (specific origins)
- [ ] Add rate limiting
- [ ] Add logging (Serilog, Application Insights)
- [ ] Add health checks
- [ ] Configure proper connection string
- [ ] Add API versioning
- [ ] Add comprehensive monitoring

---

## Next Steps: Inventory Module

Your User Management is complete! Now you can build:

1. **Product Entity** - Name, SKU, Price, Stock
2. **Category Entity** - Name, Description
3. **Product-Category Relationship** - One-to-many
4. **Product CRUD** - Commands and queries
5. **Category CRUD** - Commands and queries
6. **Stock Management** - Add/remove stock
7. **Low Stock Alerts** - Business logic
8. **Reports** - Most popular products, low stock items

Then:

9. **React Frontend** - Login page, user management UI, product management UI
10. **React Authentication** - JWT token storage, protected routes
11. **State Management** - Context API or Redux
12. **API Integration** - Axios, error handling

---

## Congratulations! 🎉

You've successfully built a production-ready User Management API using:
- Clean Architecture
- CQRS
- MediatR
- SOLID Principles
- JWT Authentication
- Entity Framework Core
- Unit Testing
- Integration Testing
- RESTful API Design

This is a strong foundation for your inventory management system!

---

## Key Learnings

### Architecture:
✅ **Separation of Concerns** - Each layer has clear responsibility  
✅ **Dependency Inversion** - Inner layers define interfaces, outer layers implement  
✅ **Testability** - Can test each layer independently  

### Patterns:
✅ **CQRS** - Commands change state, queries read state  
✅ **Mediator** - Decouples controllers from handlers  
✅ **Repository** - Abstraction over data access (DbContext)  

### Security:
✅ **Password Hashing** - BCrypt for secure storage  
✅ **JWT Tokens** - Stateless authentication  
✅ **Authorization** - Protect endpoints with [Authorize]  

### API Design:
✅ **RESTful** - Proper HTTP verbs and status codes  
✅ **Error Handling** - Consistent error responses  
✅ **Documentation** - Swagger for API discovery  

### Testing:
✅ **Unit Tests** - Fast, isolated tests  
✅ **Integration Tests** - End-to-end API tests  
✅ **High Coverage** - Confidence in code quality  

Would you like me to create guides for the Inventory module next, or do you have questions about the User Management implementation?
