# Step 2B: Application Layer - User Management CQRS with MediatR
## Instructions for GitHub Copilot

---

## Overview
This document provides instructions for building the **Application layer** which contains business use cases using **CQRS** (Command Query Responsibility Segregation) and the **Mediator pattern** with MediatR.

**Important**: This file contains instructions for GitHub Copilot to generate code. The Application layer orchestrates domain logic and prepares it for use by the API.

---

## Understanding the Application Layer

### What is the Application Layer?
- **Orchestrates business logic** - Coordinates domain objects to fulfill use cases
- **Defines interfaces** - Declares what infrastructure must implement
- **CQRS implementation** - Separates commands (write) from queries (read)
- **Depends only on Domain** - No dependencies on Infrastructure or API

### Key Concepts:

**CQRS (Command Query Responsibility Segregation)**:
- **Commands**: Change state (Create, Update, Delete) - return void, ID, or success indicator
- **Queries**: Read data (Get, List, Search) - return DTOs, never modify state
- Why? Clearer intent, separate optimization paths, easier testing

**MediatR Pattern**:
- **Mediator**: Decouples request senders from handlers
- **Handlers**: Each use case has one handler (Single Responsibility)
- **Pipeline Behaviors**: Add cross-cutting concerns (validation, logging)
- Controllers don't call services directly - they send requests through mediator

**DTOs (Data Transfer Objects)**:
- Objects for transferring data between layers
- Never return domain entities from API
- Why? Decouples API from domain, controls what data is exposed

**Interfaces**:
- Application defines what it needs (IApplicationDbContext, IJwtTokenGenerator)
- Infrastructure implements these interfaces (Dependency Inversion Principle)

---

## Project Structure for Application Layer

Your Application project should have this folder structure:
```
InventoryManagement.Application/
├── Common/
│   ├── Interfaces/
│   │   ├── IApplicationDbContext.cs
│   │   ├── IPasswordHasher.cs
│   │   ├── IJwtTokenGenerator.cs
│   │   └── ICurrentUserService.cs
│   ├── Models/
│   │   ├── UserDto.cs
│   │   └── AuthenticationResult.cs
│   └── Behaviors/
│       └── ValidationBehavior.cs
├── Features/
│   └── Users/
│       ├── Commands/
│       │   ├── LoginUser/
│       │   ├── CreateUser/
│       │   ├── UpdateUser/
│       │   ├── ActivateUser/
│       │   └── DeactivateUser/
│       └── Queries/
│           ├── GetAllUsers/
│           ├── GetUserById/
│           └── GetCurrentUser/
└── DependencyInjection.cs
```

Create these folders in the `InventoryManagement.Application` project.

---

## Task 1: Create Application Interfaces

These interfaces define what the Application layer needs from Infrastructure. This follows the **Dependency Inversion Principle**.

### Task 1.1: Database Context Interface

**Location**: `InventoryManagement.Application/Common/Interfaces/IApplicationDbContext.cs`

**Prompt for Copilot:**

> Create IApplicationDbContext interface in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Common.Interfaces`
> 
> **Purpose**: Interface for database context - Application defines what it needs, Infrastructure implements
> 
> **Requirements**:
> - Public interface
> - Import: `using InventoryManagement.Domain.Entities;`
> - Import: `using Microsoft.EntityFrameworkCore;`
> 
> **Members**:
> - Property: `DbSet<User> Users { get; }` - for accessing users table
> - Method: `Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);` - for saving changes
> 
> Add XML documentation comment explaining this is the database context interface following Dependency Inversion Principle

### Task 1.2: Password Hasher Interface

**Location**: `InventoryManagement.Application/Common/Interfaces/IPasswordHasher.cs`

**Prompt for Copilot:**

> Create IPasswordHasher interface in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Common.Interfaces`
> 
> **Purpose**: Interface for password hashing operations
> 
> **Methods**:
> - `string HashPassword(string password);` - Hashes a plain text password
> - `bool VerifyPassword(string password, string passwordHash);` - Verifies password against hash
> 
> Add XML documentation comments for each method

### Task 1.3: JWT Token Generator Interface

**Location**: `InventoryManagement.Application/Common/Interfaces/IJwtTokenGenerator.cs`

**Prompt for Copilot:**

> Create IJwtTokenGenerator interface in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Common.Interfaces`
> 
> **Purpose**: Interface for JWT token generation
> 
> **Requirements**:
> - Import: `using InventoryManagement.Domain.Entities;`
> 
> **Method**:
> - `string GenerateToken(User user);` - Generates JWT token for authenticated user
> 
> Add XML documentation comment

### Task 1.4: Current User Service Interface

**Location**: `InventoryManagement.Application/Common/Interfaces/ICurrentUserService.cs`

**Prompt for Copilot:**

> Create ICurrentUserService interface in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Common.Interfaces`
> 
> **Purpose**: Interface to get current authenticated user information from HTTP context
> 
> **Properties** (all with get only):
> - `Guid? UserId` - ID of current user, nullable
> - `string? Email` - Email of current user, nullable
> - `bool IsAuthenticated` - Whether user is authenticated
> 
> Add XML documentation comment

### Expected Outcome
After creating interfaces, you should have:
- ✅ IApplicationDbContext
- ✅ IPasswordHasher
- ✅ IJwtTokenGenerator
- ✅ ICurrentUserService

---

## Task 2: Create DTOs (Data Transfer Objects)

DTOs are used to transfer data to/from the API. Never expose domain entities directly!

### Task 2.1: UserDto

**Location**: `InventoryManagement.Application/Common/Models/UserDto.cs`

**Prompt for Copilot:**

> Create UserDto class in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Common.Models`
> 
> **Purpose**: DTO for returning user information to API clients
> 
> **Requirements**:
> - Sealed public class
> - Properties with public getters and setters
> 
> **Properties**:
> - `Guid Id`
> - `string FirstName` - initialize to empty string
> - `string LastName` - initialize to empty string
> - `string FullName` - initialize to empty string
> - `string Email` - initialize to empty string
> - `bool IsActive`
> - `DateTime CreatedAt`
> - `DateTime UpdatedAt`
> 
> Add XML documentation comment explaining this is a DTO and should never return domain entities directly

### Task 2.2: AuthenticationResult

**Location**: `InventoryManagement.Application/Common/Models/AuthenticationResult.cs`

**Prompt for Copilot:**

> Create AuthenticationResult class in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Common.Models`
> 
> **Purpose**: DTO for authentication response containing user info and JWT token
> 
> **Requirements**:
> - Sealed public class
> 
> **Properties**:
> - `UserDto User` - with null-forgiving operator (= null!)
> - `string Token` - initialize to empty string
> 
> Add XML documentation comment

### Expected Outcome
- ✅ UserDto created
- ✅ AuthenticationResult created

---

## Task 3: Create CQRS Commands and Queries - Login User

Now we'll implement our first use case: User Login with JWT authentication.

### Folder Structure
Create folder: `InventoryManagement.Application/Features/Users/Commands/LoginUser/`

### Task 3.1: LoginUserCommand

**Location**: `InventoryManagement.Application/Features/Users/Commands/LoginUser/LoginUserCommand.cs`

**Prompt for Copilot:**

> Create LoginUserCommand record in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Features.Users.Commands.LoginUser`
> 
> **Requirements**:
> - Public sealed record
> - Import: `using InventoryManagement.Application.Common.Models;`
> - Import: `using MediatR;`
> - Implements: `IRequest<AuthenticationResult>`
> 
> **Parameters** (record properties):
> - `string Email`
> - `string Password`
> 
> Add XML documentation comment explaining this is a command to authenticate a user

### Task 3.2: LoginUserCommandValidator

**Location**: `InventoryManagement.Application/Features/Users/Commands/LoginUser/LoginUserCommandValidator.cs`

**Prompt for Copilot:**

> Create LoginUserCommandValidator class in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Features.Users.Commands.LoginUser`
> 
> **Requirements**:
> - Public sealed class
> - Import: `using FluentValidation;`
> - Inherits: `AbstractValidator<LoginUserCommand>`
> 
> **Validation Rules** (in constructor):
> - Email: NotEmpty with message "Email is required", EmailAddress with message "Invalid email format"
> - Password: NotEmpty with message "Password is required", MinimumLength(6) with message "Password must be at least 6 characters"
> 
> Use FluentValidation's fluent API: `RuleFor(x => x.Property).NotEmpty().WithMessage("...")`
> 
> Add XML documentation comment explaining this validates LoginUserCommand using FluentValidation

### Task 3.3: LoginUserCommandHandler

**Location**: `InventoryManagement.Application/Features/Users/Commands/LoginUser/LoginUserCommandHandler.cs`

**Prompt for Copilot:**

> Create LoginUserCommandHandler class in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Features.Users.Commands.LoginUser`
> 
> **Requirements**:
> - Public sealed class
> - Imports needed: Common.Interfaces, Common.Models, Domain.Exceptions, Domain.ValueObjects, MediatR, Microsoft.EntityFrameworkCore
> - Implements: `IRequestHandler<LoginUserCommand, AuthenticationResult>`
> 
> **Constructor Dependencies**:
> - `IApplicationDbContext context`
> - `IPasswordHasher passwordHasher`
> - `IJwtTokenGenerator jwtTokenGenerator`
> 
> **Handle Method Logic**:
> 1. Create Email value object from request.Email using Email.Create
> 2. Find user by email using _context.Users.FirstOrDefaultAsync
> 3. If user is null, throw InvalidCredentialsException
> 4. Verify password using _passwordHasher.VerifyPassword
> 5. If password incorrect, throw InvalidCredentialsException
> 6. Check if user.IsActive is true, if not throw InvalidCredentialsException
> 7. Generate JWT token using _jwtTokenGenerator.GenerateToken(user)
> 8. Return new AuthenticationResult with UserDto mapped from user entity and token
> 
> **UserDto Mapping**:
> Map all properties from user entity to UserDto (Id, FirstName, LastName, FullName, Email.Value, IsActive, CreatedAt, UpdatedAt)
> 
> Add XML documentation comments explaining this handles login and returns JWT token

### Expected Outcome
- ✅ LoginUserCommand created
- ✅ LoginUserCommandValidator created
- ✅ LoginUserCommandHandler created

---

## Task 4: Create User Command - Create User

### Folder Structure
Create folder: `InventoryManagement.Application/Features/Users/Commands/CreateUser/`

### Task 4.1: CreateUserCommand

**Location**: `InventoryManagement.Application/Features/Users/Commands/CreateUser/CreateUserCommand.cs`

**Prompt for Copilot:**

> Create CreateUserCommand record in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Features.Users.Commands.CreateUser`
> 
> **Requirements**:
> - Public sealed record
> - Import: `using MediatR;`
> - Implements: `IRequest<Guid>` (returns the ID of created user)
> 
> **Parameters**:
> - `string FirstName`
> - `string LastName`
> - `string Email`
> - `string Password`
> 
> Add XML documentation comment

### Task 4.2: CreateUserCommandValidator

**Location**: `InventoryManagement.Application/Features/Users/Commands/CreateUser/CreateUserCommandValidator.cs`

**Prompt for Copilot:**

> Create CreateUserCommandValidator class in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Features.Users.Commands.CreateUser`
> 
> **Requirements**:
> - Public sealed class inheriting from `AbstractValidator<CreateUserCommand>`
> - Import: `using FluentValidation;`
> 
> **Validation Rules**:
> - FirstName: NotEmpty("First name is required"), MaximumLength(100)
> - LastName: NotEmpty("Last name is required"), MaximumLength(100)
> - Email: NotEmpty("Email is required"), EmailAddress("Invalid email format")
> - Password: NotEmpty("Password is required"), MinimumLength(6), MaximumLength(100)

### Task 4.3: CreateUserCommandHandler

**Location**: `InventoryManagement.Application/Features/Users/Commands/CreateUser/CreateUserCommandHandler.cs`

**Prompt for Copilot:**

> Create CreateUserCommandHandler class in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Features.Users.Commands.CreateUser`
> 
> **Requirements**:
> - Public sealed class implementing `IRequestHandler<CreateUserCommand, Guid>`
> - Import necessary namespaces
> 
> **Constructor Dependencies**:
> - `IApplicationDbContext context`
> - `IPasswordHasher passwordHasher`
> 
> **Handle Method Logic**:
> 1. Create Email value object from request.Email
> 2. Check if email already exists using _context.Users.AnyAsync(u => u.Email == email)
> 3. If exists, throw DuplicateEmailException
> 4. Hash password using _passwordHasher.HashPassword(request.Password)
> 5. Create user entity using User.Create(firstName, lastName, email, passwordHash)
> 6. Add user to _context.Users
> 7. Call _context.SaveChangesAsync
> 8. Return user.Id
> 
> Add XML documentation comment

### Expected Outcome
- ✅ CreateUserCommand created
- ✅ CreateUserCommandValidator created  
- ✅ CreateUserCommandHandler created

---

## Task 5: Create User Command - Update User

**IMPORTANT BUSINESS RULE**: A logged-in user CAN edit their own information AND other users' information.

### Folder Structure
Create folder: `InventoryManagement.Application/Features/Users/Commands/UpdateUser/`

### Task 5.1: UpdateUserCommand

**Location**: `InventoryManagement.Application/Features/Users/Commands/UpdateUser/UpdateUserCommand.cs`

**Prompt for Copilot:**

> Create UpdateUserCommand record in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Features.Users.Commands.UpdateUser`
> 
> **Requirements**:
> - Public sealed record
> - Import: `using MediatR;`
> - Implements: `IRequest<Unit>` (Unit means void in MediatR)
> 
> **Parameters**:
> - `Guid UserId`
> - `string FirstName`
> - `string LastName`
> - `string Email`
> 
> Add XML documentation comment explaining logged-in users can update their own or other users' information

### Task 5.2: UpdateUserCommandValidator

**Location**: `InventoryManagement.Application/Features/Users/Commands/UpdateUser/UpdateUserCommandValidator.cs`

**Prompt for Copilot:**

> Create UpdateUserCommandValidator class in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Features.Users.Commands.UpdateUser`
> 
> **Requirements**:
> - Public sealed class inheriting from `AbstractValidator<UpdateUserCommand>`
> 
> **Validation Rules**:
> - UserId: NotEmpty("User ID is required")
> - FirstName: NotEmpty("First name is required"), MaximumLength(100)
> - LastName: NotEmpty("Last name is required"), MaximumLength(100)
> - Email: NotEmpty("Email is required"), EmailAddress("Invalid email format")

### Task 5.3: UpdateUserCommandHandler

**Location**: `InventoryManagement.Application/Features/Users/Commands/UpdateUser/UpdateUserCommandHandler.cs`

**Prompt for Copilot:**

> Create UpdateUserCommandHandler class in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Features.Users.Commands.UpdateUser`
> 
> **Requirements**:
> - Public sealed class implementing `IRequestHandler<UpdateUserCommand, Unit>`
> - Import necessary namespaces
> 
> **Constructor Dependencies**:
> - `IApplicationDbContext context`
> 
> **Handle Method Logic**:
> 1. Find user by request.UserId using FirstOrDefaultAsync
> 2. If user is null, throw UserNotFoundException(request.UserId)
> 3. Create new Email value object from request.Email
> 4. Check if new email is already taken by another user (query: email == email && u.Id != request.UserId)
> 5. If email taken, throw DuplicateEmailException
> 6. Call user.UpdateInformation(request.FirstName, request.LastName, email)
> 7. Call _context.SaveChangesAsync
> 8. Return Unit.Value
> 
> Add XML documentation comment explaining any logged-in user can update any user's information

### Expected Outcome
- ✅ UpdateUserCommand created
- ✅ UpdateUserCommandValidator created
- ✅ UpdateUserCommandHandler created (no self-update restriction)

---

## Task 6: Create User Commands - Activate/Deactivate

### Task 6.1: ActivateUserCommand and Handler

**Folder**: Create `InventoryManagement.Application/Features/Users/Commands/ActivateUser/`

**Location 1**: `ActivateUserCommand.cs`

**Prompt for Copilot:**

> Create ActivateUserCommand record with:
> - Namespace: `InventoryManagement.Application.Features.Users.Commands.ActivateUser`
> - Public sealed record implementing `IRequest<Unit>`
> - Single parameter: `Guid UserId`
> - Add XML documentation comment

**Location 2**: `ActivateUserCommandHandler.cs`

**Prompt for Copilot:**

> Create ActivateUserCommandHandler class with:
> - Namespace: `InventoryManagement.Application.Features.Users.Commands.ActivateUser`
> - Public sealed class implementing `IRequestHandler<ActivateUserCommand, Unit>`
> - Constructor dependency: `IApplicationDbContext context`
> - Handle method logic:
>   1. Find user by request.UserId
>   2. If not found, throw UserNotFoundException
>   3. Call user.Activate()
>   4. Call SaveChangesAsync
>   5. Return Unit.Value
> - Add XML documentation comment

### Task 6.2: DeactivateUserCommand and Handler

**Folder**: Create `InventoryManagement.Application/Features/Users/Commands/DeactivateUser/`

**Location 1**: `DeactivateUserCommand.cs`

**Prompt for Copilot:**

> Create DeactivateUserCommand record with:
> - Namespace: `InventoryManagement.Application.Features.Users.Commands.DeactivateUser`
> - Public sealed record implementing `IRequest<Unit>`
> - Single parameter: `Guid UserId`
> - Add XML documentation comment

**Location 2**: `DeactivateUserCommandHandler.cs`

**Prompt for Copilot:**

> Create DeactivateUserCommandHandler class with:
> - Namespace: `InventoryManagement.Application.Features.Users.Commands.DeactivateUser`
> - Public sealed class implementing `IRequestHandler<DeactivateUserCommand, Unit>`
> - Constructor dependency: `IApplicationDbContext context`
> - Handle method logic:
>   1. Find user by request.UserId
>   2. If not found, throw UserNotFoundException
>   3. Call user.Deactivate()
>   4. Call SaveChangesAsync
>   5. Return Unit.Value
> - Add XML documentation comment
> - Note: Any logged-in admin can deactivate any user (including themselves if they want)

### Expected Outcome
- ✅ ActivateUserCommand and Handler created
- ✅ DeactivateUserCommand and Handler created (no self-deactivate restriction)

---

## Task 7: Create CQRS Queries

Queries read data but never modify state. They return DTOs, not domain entities.

### Task 7.1: GetAllUsersQuery

**Folder**: Create `InventoryManagement.Application/Features/Users/Queries/GetAllUsers/`

**Location 1**: `GetAllUsersQuery.cs`

**Prompt for Copilot:**

> Create GetAllUsersQuery record with:
> - Namespace: `InventoryManagement.Application.Features.Users.Queries.GetAllUsers`
> - Public sealed record implementing `IRequest<List<UserDto>>`
> - No parameters (empty record)
> - Add XML documentation comment explaining this query returns all users

**Location 2**: `GetAllUsersQueryHandler.cs`

**Prompt for Copilot:**

> Create GetAllUsersQueryHandler class with:
> - Namespace: `InventoryManagement.Application.Features.Users.Queries.GetAllUsers`
> - Public sealed class implementing `IRequestHandler<GetAllUsersQuery, List<UserDto>>`
> - Constructor dependency: `IApplicationDbContext context`
> - Handle method logic:
>   1. Query _context.Users
>   2. Order by FirstName, then by LastName
>   3. Use Select to project to UserDto (map all properties including Email.Value)
>   4. Call ToListAsync
>   5. Return the list
> - Add XML documentation comment

### Task 7.2: GetUserByIdQuery

**Folder**: Create `InventoryManagement.Application/Features/Users/Queries/GetUserById/`

**Location 1**: `GetUserByIdQuery.cs`

**Prompt for Copilot:**

> Create GetUserByIdQuery record with:
> - Namespace: `InventoryManagement.Application.Features.Users.Queries.GetUserById`
> - Public sealed record implementing `IRequest<UserDto>`
> - Parameter: `Guid UserId`
> - Add XML documentation comment

**Location 2**: `GetUserByIdQueryHandler.cs`

**Prompt for Copilot:**

> Create GetUserByIdQueryHandler class with:
> - Namespace: `InventoryManagement.Application.Features.Users.Queries.GetUserById`
> - Public sealed class implementing `IRequestHandler<GetUserByIdQuery, UserDto>`
> - Constructor dependency: `IApplicationDbContext context`
> - Handle method logic:
>   1. Query _context.Users where Id == request.UserId
>   2. Use Select to project to UserDto
>   3. Call FirstOrDefaultAsync
>   4. If null, throw UserNotFoundException
>   5. Return the UserDto
> - Add XML documentation comment

### Task 7.3: GetCurrentUserQuery

**Folder**: Create `InventoryManagement.Application/Features/Users/Queries/GetCurrentUser/`

**Location 1**: `GetCurrentUserQuery.cs`

**Prompt for Copilot:**

> Create GetCurrentUserQuery record with:
> - Namespace: `InventoryManagement.Application.Features.Users.Queries.GetCurrentUser`
> - Public sealed record implementing `IRequest<UserDto>`
> - No parameters
> - Add XML documentation comment

**Location 2**: `GetCurrentUserQueryHandler.cs`

**Prompt for Copilot:**

> Create GetCurrentUserQueryHandler class with:
> - Namespace: `InventoryManagement.Application.Features.Users.Queries.GetCurrentUser`
> - Public sealed class implementing `IRequestHandler<GetCurrentUserQuery, UserDto>`
> - Constructor dependencies: `IApplicationDbContext context`, `ICurrentUserService currentUserService`
> - Handle method logic:
>   1. Check if currentUserService.IsAuthenticated, if not throw UnauthorizedAccessException
>   2. Check if currentUserService.UserId is null, if so throw UnauthorizedAccessException
>   3. Query user by currentUserService.UserId.Value
>   4. Project to UserDto
>   5. If null, throw UserNotFoundException
>   6. Return UserDto
> - Add XML documentation comment

### Expected Outcome
- ✅ GetAllUsersQuery and Handler created
- ✅ GetUserByIdQuery and Handler created
- ✅ GetCurrentUserQuery and Handler created

---

## Task 8: Create Validation Pipeline Behavior

This is MediatR "magic" - it automatically validates all requests before they reach handlers!

**Location**: `InventoryManagement.Application/Common/Behaviors/ValidationBehavior.cs`

**Prompt for Copilot:**

> Create ValidationBehavior class in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Common.Behaviors`
> 
> **Requirements**:
> - Public sealed generic class
> - Imports: `using FluentValidation;`, `using MediatR;`
> - Generic parameters: `<TRequest, TResponse> where TRequest : IRequest<TResponse>`
> - Implements: `IPipelineBehavior<TRequest, TResponse>`
> 
> **Constructor**:
> - Inject: `IEnumerable<IValidator<TRequest>> validators`
> 
> **Handle Method**:
> - Parameters: TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken
> - Logic:
>   1. If no validators, call next() and return
>   2. Create ValidationContext<TRequest> from request
>   3. Validate all validators in parallel using Task.WhenAll
>   4. Collect all validation failures
>   5. If any failures exist, throw ValidationException with failures
>   6. Otherwise call next() and return result
> 
> Add XML documentation explaining this is a MediatR pipeline behavior that validates requests before handlers execute, demonstrating Open/Closed Principle

### Expected Outcome
- ✅ ValidationBehavior created
- ✅ Will automatically validate ALL commands and queries

---

## Task 9: Create Dependency Injection Registration

This registers all Application services with the DI container.

**Location**: `InventoryManagement.Application/DependencyInjection.cs`

**Prompt for Copilot:**

> Create DependencyInjection static class in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Application`
> 
> **Requirements**:
> - Public static class
> - Imports: `using System.Reflection;`, `using FluentValidation;`, `using MediatR;`, `using Microsoft.Extensions.DependencyInjection;`
> 
> **Extension Method** - `AddApplication`:
> - Returns: `IServiceCollection`
> - Parameter: `this IServiceCollection services`
> - Logic:
>   1. Get current assembly: `var assembly = Assembly.GetExecutingAssembly();`
>   2. Register MediatR: `services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(assembly); cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>)); });`
>   3. Register FluentValidation validators: `services.AddValidatorsFromAssembly(assembly);`
>   4. Return services
> 
> Add XML documentation comment explaining this registers all Application layer services following Dependency Inversion Principle

### Expected Outcome
- ✅ DependencyInjection.cs created
- ✅ All MediatR handlers registered automatically
- ✅ All validators registered automatically
- ✅ Validation pipeline behavior registered

---

## Task 10: Create Unit Tests for Application Layer

Unit tests verify handlers work correctly with mocked dependencies.

### Setup Required Packages
Make sure `InventoryManagement.Application.Tests` has these packages:
- xUnit
- FluentAssertions
- Moq (for mocking)
- Microsoft.NET.Test.Sdk

### Task 10.1: CreateUserCommandHandler Tests

**Location**: `InventoryManagement.Application.Tests/Features/Users/Commands/CreateUserCommandHandlerTests.cs`

**Prompt for Copilot:**

> Create unit tests for CreateUserCommandHandler using xUnit, Moq, and FluentAssertions with these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Tests.Features.Users.Commands`
> 
> **Test Class**: `CreateUserCommandHandlerTests`
> 
> **Setup** (in constructor):
> - Create Mock<IApplicationDbContext> _contextMock
> - Create Mock<IPasswordHasher> _passwordHasherMock
> - Create Mock<DbSet<User>> _usersDbSetMock
> - Setup _contextMock.Setup(x => x.Users).Returns(_usersDbSetMock.Object)
> - Create handler instance with mocked dependencies
> 
> **Test Methods**:
> 
> 1. **Handle_WithValidCommand_ShouldCreateUser**:
>    - Arrange: Valid CreateUserCommand, mock password hasher to return "hashed_password", mock empty users list (email doesn't exist)
>    - Act: Call handler.Handle
>    - Assert: Result is not empty Guid, Verify _usersDbSetMock.Add called once with User matching command data, Verify SaveChangesAsync called once
> 
> 2. **Handle_WithDuplicateEmail_ShouldThrowDuplicateEmailException**:
>    - Arrange: Command with existing email, mock users list with existing user
>    - Act: Call handler.Handle
>    - Assert: Should throw DuplicateEmailException with matching email in message
> 
> 3. **Handle_WithInvalidEmail_ShouldThrowArgumentException** (Theory with InlineData):
>    - Test invalid email formats: "invalid-email", "@test.com", "test@"
>    - Assert: Should throw ArgumentException
> 
> You'll need a helper method to mock DbSet properly - create MockDbSet method that sets up Provider, Expression, ElementType, and GetEnumerator
> 
> Use FluentAssertions: result.Should().NotBeEmpty(), act.Should().ThrowAsync<Exception>()

### Task 10.2: LoginUserCommandHandler Tests

**Location**: `InventoryManagement.Application.Tests/Features/Users/Commands/LoginUserCommandHandlerTests.cs`

**Prompt for Copilot:**

> Create unit tests for LoginUserCommandHandler with these test methods:
> 
> **Namespace**: `InventoryManagement.Application.Tests.Features.Users.Commands`
> 
> **Test Class**: `LoginUserCommandHandlerTests`
> 
> **Test Methods**:
> 
> 1. **Handle_WithValidCredentials_ShouldReturnAuthenticationResult**:
>    - Arrange: Valid LoginUserCommand, mock user exists, mock password verification returns true, mock JWT token generation returns "test_token"
>    - Act: Call handler.Handle
>    - Assert: Result not null, User properties match, Token equals "test_token"
> 
> 2. **Handle_WithInvalidEmail_ShouldThrowInvalidCredentialsException**:
>    - Arrange: Command with non-existent email, mock empty users list
>    - Act: Call handler
>    - Assert: Should throw InvalidCredentialsException
> 
> 3. **Handle_WithInvalidPassword_ShouldThrowInvalidCredentialsException**:
>    - Arrange: Valid email but mock password verification returns false
>    - Assert: Should throw InvalidCredentialsException
> 
> 4. **Handle_WithInactiveUser_ShouldThrowInvalidCredentialsException**:
>    - Arrange: Valid credentials but user.IsActive is false
>    - Assert: Should throw InvalidCredentialsException

### Task 10.3: GetAllUsersQueryHandler Tests

**Location**: `InventoryManagement.Application.Tests/Features/Users/Queries/GetAllUsersQueryHandlerTests.cs`

**Prompt for Copilot:**

> Create unit tests for GetAllUsersQueryHandler with these test methods:
> 
> **Namespace**: `InventoryManagement.Application.Tests.Features.Users.Queries`
> 
> **Test Class**: `GetAllUsersQueryHandlerTests`
> 
> **Test Methods**:
> 
> 1. **Handle_WithUsers_ShouldReturnAllUsers**:
>    - Arrange: Mock users list with 3 users (Alice, Bob, Charlie), mock DbSet
>    - Act: Call handler.Handle with GetAllUsersQuery
>    - Assert: Result should have count 3, should contain users with correct emails, should be ordered by FirstName
> 
> 2. **Handle_WithNoUsers_ShouldReturnEmptyList**:
>    - Arrange: Mock empty users list
>    - Act: Call handler
>    - Assert: Result should be empty

### Expected Outcome
After running tests:
- ✅ All Application layer unit tests passing
- ✅ Code coverage for commands and queries
- ✅ Mocking infrastructure dependencies works correctly

---

## Verification Checklist

Before moving to Step 2C, verify:

- [ ] All application interfaces created (IApplicationDbContext, IPasswordHasher, IJwtTokenGenerator, ICurrentUserService)
- [ ] All DTOs created (UserDto, AuthenticationResult)
- [ ] All commands created with validators and handlers (Login, Create, Update, Activate, Deactivate)
- [ ] All queries created with handlers (GetAll, GetById, GetCurrent)
- [ ] ValidationBehavior created
- [ ] DependencyInjection.cs created
- [ ] All application unit tests created and passing
- [ ] Solution builds without errors
- [ ] Updated business rule: Users CAN edit their own information

---

## What You've Learned

### CQRS Pattern:
✅ **Commands** - Change state, validated, return void/ID  
✅ **Queries** - Read data, return DTOs  
✅ **Separation** - Clear intent, separate optimization  

### MediatR Pattern:
✅ **Mediator** - Decouples senders from handlers  
✅ **Handlers** - One handler per use case (SRP)  
✅ **Behaviors** - Cross-cutting concerns (validation)  

### Validation:
✅ **FluentValidation** - Declarative validation rules  
✅ **Pipeline Behavior** - Automatic validation before handlers  
✅ **Fail Fast** - Invalid requests never reach handlers  

### Testing:
✅ **Mocking** - Test handlers without real database  
✅ **Arrange-Act-Assert** - Clear test structure  
✅ **Unit Tests** - Fast, isolated, repeatable  

### SOLID Principles:
✅ **Single Responsibility** - Each handler does one thing  
✅ **Open/Closed** - ValidationBehavior extends without modifying  
✅ **Dependency Inversion** - Depend on abstractions (interfaces)  

---

## Common Issues and Solutions

**Issue**: "IRequest not found"
- Install MediatR package in Application project
- Add `using MediatR;` to file

**Issue**: "AbstractValidator not found"
- Install FluentValidation package
- Add `using FluentValidation;`

**Issue**: "Cannot mock DbSet"
- DbSet is difficult to mock - use helper method to setup Provider, Expression, ElementType, GetEnumerator
- Or consider using in-memory database for integration tests

**Issue**: "Validators not running"
- Make sure ValidationBehavior is registered in DependencyInjection
- Verify validators inherit from AbstractValidator<T>

---

## Next Steps

You're ready for **Step 2C: Infrastructure & API Layer**!

In Step 2C, you'll:
1. Implement the interfaces you just defined (DbContext, PasswordHasher, JwtTokenGenerator)
2. Configure Entity Framework Core
3. Create database migration
4. Seed default admin user
5. Build API controllers
6. Configure JWT authentication
7. Add global exception handling
8. Create integration tests
9. Test the entire application end-to-end!

---

## Tips for Working with Copilot

1. **Copy entire prompt sections** - Copilot works best with detailed specifications
2. **One file at a time** - Don't rush, verify each file compiles
3. **Use Copilot Chat** - Ask it to explain generated code
4. **Build frequently** - Catch errors early
5. **Run tests after each handler** - Ensure each piece works before moving on

---

## Summary

Application layer complete! You've instructed Copilot to create:
- ✅ 4 Application interfaces
- ✅ 2 DTOs
- ✅ 5 Commands with validators and handlers
- ✅ 3 Queries with handlers
- ✅ Validation pipeline behavior
- ✅ Dependency injection setup
- ✅ Comprehensive unit tests

Your Application layer now orchestrates business logic using CQRS and MediatR!
