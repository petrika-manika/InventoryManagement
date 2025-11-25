# Step 6A: Backend - Clients Module (Domain & Application Layers)
## Instructions for GitHub Copilot

---

## Overview
This guide provides instructions for building the **Clients Module** backend layers. We'll implement Domain entities, Application layer CQRS commands and queries for managing two types of clients: Individual and Business.

**Client Types:**
- **Individual** - Personal clients with basic contact information
- **Business** - Company clients with NIPT, owner, and contact person details

**Time estimate:** 3-4 hours

---

## Part 1: Domain Layer

### Folder Structure

Create this folder structure in `InventoryManagement.Domain`:

```
Domain/
├── Entities/
│   ├── Client.cs              # Abstract base class
│   ├── IndividualClient.cs    # Individual client entity
│   └── BusinessClient.cs      # Business client entity
├── Enums/
│   └── ClientType.cs          # Individual = 1, Business = 2
├── ValueObjects/
│   ├── PersonName.cs          # Value object for names
│   └── NIPT.cs                # Value object for business ID
└── Exceptions/
    ├── DuplicateNIPTException.cs
    └── InvalidClientDataException.cs
```

---

## Task 1: Create ClientType Enum

### Location: `Domain/Enums/ClientType.cs`

**Prompt for Copilot:**

> Create ClientType enum following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Enums`
> 
> **Enum values**:
> - Individual = 1
> - Business = 2
> 
> Add XML documentation explaining each type

---

## Task 2: Create PersonName Value Object

### Location: `Domain/ValueObjects/PersonName.cs`

**Prompt for Copilot:**

> Create PersonName value object following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.ValueObjects`
> 
> **Purpose**: Encapsulate first and last name validation
> 
> **Properties**:
> - FirstName (string) - required, 1-50 characters
> - LastName (string) - required, 1-50 characters
> - FullName (computed property) - returns "FirstName LastName"
> 
> **Constructor**:
> - Takes firstName and lastName parameters
> - Validates both are not null, empty, or whitespace
> - Validates length constraints
> - Throws ArgumentException with clear message on validation failure
> 
> **Methods**:
> - Implement Equals, GetHashCode for value equality
> - Implement ToString() returning FullName
> 
> Add XML documentation

---

## Task 3: Create NIPT Value Object

### Location: `Domain/ValueObjects/NIPT.cs`

**Prompt for Copilot:**

> Create NIPT (Business ID) value object following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.ValueObjects`
> 
> **Purpose**: Encapsulate NIPT (Albanian business tax ID) validation
> 
> **Properties**:
> - Value (string) - the NIPT number
> 
> **Constructor**:
> - Takes string value parameter
> - Validates not null, empty, or whitespace
> - Validates format: 10 alphanumeric characters
> - Throws ArgumentException with clear message on validation failure
> 
> **Methods**:
> - Implement Equals, GetHashCode for value equality
> - Implement ToString() returning Value
> - Static method IsValid(string? value) - returns bool without throwing
> 
> Add XML documentation

---

## Task 4: Create Domain Exceptions

### Location: `Domain/Exceptions/DuplicateNIPTException.cs`

**Prompt for Copilot:**

> Create DuplicateNIPTException following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Exceptions`
> 
> **Inherits from**: `DomainException`
> 
> **Constructor parameters**:
> - nipt (string) - the duplicate NIPT value
> 
> **Properties**:
> - NIPT (string) - public get
> 
> **Message format**: "A business client with NIPT '{NIPT}' already exists."
> 
> Add XML documentation

### Location: `Domain/Exceptions/InvalidClientDataException.cs`

**Prompt for Copilot:**

> Create InvalidClientDataException following these specifications:
> 
> **Inherits from**: `DomainException`
> 
> **Constructor**:
> - Takes string message parameter
> 
> **Use cases**:
> - Invalid email format
> - Invalid phone number format
> - Missing required fields
> 
> Add XML documentation

---

## Task 5: Create Client Base Entity

### Location: `Domain/Entities/Client.cs`

**Prompt for Copilot:**

> Create abstract Client base entity following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Entities`
> 
> **Class**: Public abstract class
> 
> **Common Properties**:
> - Id (string) - primary key, generated GUID
> - ClientType (ClientType enum) - Individual or Business
> - Address (string?) - optional, max 500 characters
> - Email (string?) - optional, validated email format
> - PhoneNumber (string?) - optional, max 20 characters
> - Notes (string?) - optional, no length limit
> - CreatedAt (DateTime) - set on creation
> - UpdatedAt (DateTime) - updated on modification
> - CreatedBy (string) - user ID who created
> - UpdatedBy (string?) - user ID who last updated
> - IsActive (bool) - default true, for soft delete
> 
> **Methods**:
> - **ValidateEmail()** - protected method
>   - If Email is not null/empty, validate format using regex or EmailAddressAttribute
>   - Throw InvalidClientDataException if invalid
> 
> - **ValidatePhoneNumber()** - protected method
>   - If PhoneNumber is not null/empty, validate format (digits, spaces, +, -, (, ) only)
>   - Throw InvalidClientDataException if invalid
> 
> - **UpdateCommonFields(address, email, phoneNumber, notes)** - public method
>   - Update common fields
>   - Validate email and phone
>   - Set UpdatedAt to DateTime.UtcNow
> 
> - **Deactivate()** - public method
>   - Set IsActive = false
>   - Set UpdatedAt = DateTime.UtcNow
> 
> Add XML documentation

---

## Task 6: Create IndividualClient Entity

### Location: `Domain/Entities/IndividualClient.cs`

**Prompt for Copilot:**

> Create IndividualClient entity following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Entities`
> 
> **Class**: Public sealed class inheriting from Client
> 
> **Properties**:
> - FirstName (string) - required, 1-50 characters
> - LastName (string) - required, 1-50 characters
> - FullName (computed property) - returns "FirstName LastName"
> 
> **Constructor**:
> - Parameters: firstName, lastName, address?, email?, phoneNumber?, notes?, createdBy
> - Validate firstName and lastName (not null/empty, length 1-50)
> - Throw InvalidClientDataException if validation fails
> - Call ValidateEmail() and ValidatePhoneNumber() from base
> - Set ClientType = ClientType.Individual
> - Set CreatedAt = DateTime.UtcNow
> - Set IsActive = true
> - Set all properties
> 
> **Methods**:
> - **UpdatePersonalInfo(firstName, lastName, address, email, phoneNumber, notes)** - public
>   - Validate firstName and lastName
>   - Call base.UpdateCommonFields for common fields
>   - Update FirstName and LastName
>   - Set UpdatedAt = DateTime.UtcNow
> 
> Add XML documentation

---

## Task 7: Create BusinessClient Entity

### Location: `Domain/Entities/BusinessClient.cs`

**Prompt for Copilot:**

> Create BusinessClient entity following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Entities`
> 
> **Class**: Public sealed class inheriting from Client
> 
> **Properties**:
> - NIPT (NIPT value object) - required, unique
> - OwnerFirstName (string?) - optional, max 50 characters
> - OwnerLastName (string?) - optional, max 50 characters
> - OwnerPhoneNumber (string?) - optional, max 20 characters
> - ContactPersonFirstName (string) - required, 1-50 characters
> - ContactPersonLastName (string) - required, 1-50 characters
> - ContactPersonPhoneNumber (string?) - optional, max 20 characters
> - OwnerFullName (computed property) - returns "OwnerFirstName OwnerLastName" or null
> - ContactPersonFullName (computed property) - returns "ContactPersonFirstName ContactPersonLastName"
> 
> **Constructor**:
> - Parameters: nipt (NIPT), contactPersonFirstName, contactPersonLastName, 
>   ownerFirstName?, ownerLastName?, ownerPhoneNumber?, contactPersonPhoneNumber?,
>   address?, email?, phoneNumber?, notes?, createdBy
> - Validate NIPT is not null (throw ArgumentNullException)
> - Validate contactPersonFirstName and contactPersonLastName (required, length 1-50)
> - Validate owner names if provided (length 1-50)
> - Validate all phone numbers if provided (same as base class)
> - Throw InvalidClientDataException for validation failures
> - Call ValidateEmail() and ValidatePhoneNumber() from base
> - Set ClientType = ClientType.Business
> - Set CreatedAt = DateTime.UtcNow
> - Set IsActive = true
> - Set all properties
> 
> **Methods**:
> - **UpdateBusinessInfo(...)** - public
>   - Parameters: nipt, ownerFirstName?, ownerLastName?, ownerPhoneNumber?,
>     contactPersonFirstName, contactPersonLastName, contactPersonPhoneNumber?,
>     address, email, phoneNumber, notes
>   - Validate NIPT (throw if null)
>   - Validate contact person names (required)
>   - Validate owner names if provided
>   - Validate all phone numbers
>   - Call base.UpdateCommonFields for common fields
>   - Update all business-specific properties
>   - Set UpdatedAt = DateTime.UtcNow
> 
> Add XML documentation

---

## Part 2: Application Layer

### Folder Structure

Create this folder structure in `InventoryManagement.Application`:

```
Application/
├── Features/
│   └── Clients/
│       ├── Commands/
│       │   ├── CreateIndividualClient/
│       │   ├── CreateBusinessClient/
│       │   ├── UpdateIndividualClient/
│       │   ├── UpdateBusinessClient/
│       │   └── DeleteClient/
│       └── Queries/
│           ├── GetAllClients/
│           ├── GetClientById/
│           ├── GetClientsByType/
│           └── SearchClients/
├── Common/
│   ├── Interfaces/
│   │   └── IApplicationDbContext.cs  # Update this
│   └── Models/
│       ├── ClientDto.cs
│       ├── IndividualClientDto.cs
│       └── BusinessClientDto.cs
```

---

## Task 8: Update IApplicationDbContext Interface

### Location: `Application/Common/Interfaces/IApplicationDbContext.cs`

**Prompt for Copilot:**

> Update IApplicationDbContext interface to add Clients DbSet:
> 
> **Add this new DbSet property**:
> - `DbSet<Client> Clients { get; }` - for all clients (base type)
> 
> **Add import**:
> - using InventoryManagement.Domain.Entities;
> 
> Keep existing DbSets (Users, Products, StockHistories) and SaveChangesAsync method

---

## Task 9: Create DTOs (Data Transfer Objects)

### Task 9.1: Base Client DTO

**Location**: `Application/Common/Models/ClientDto.cs`

**Prompt for Copilot:**

> Create ClientDto class following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Common.Models`
> 
> **Purpose**: Base DTO for returning client information
> 
> **Properties**:
> - Id (string)
> - ClientType (string) - enum name as string ("Individual" or "Business")
> - ClientTypeId (int) - enum value (1 or 2)
> - Address (string?)
> - Email (string?)
> - PhoneNumber (string?)
> - Notes (string?)
> - CreatedAt (DateTime)
> - UpdatedAt (DateTime)
> - CreatedBy (string)
> - UpdatedBy (string?)
> - IsActive (bool)
> 
> All properties with public get/set
> 
> Add XML documentation

### Task 9.2: Individual Client DTO

**Location**: `Application/Common/Models/IndividualClientDto.cs`

**Prompt for Copilot:**

> Create IndividualClientDto class following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Common.Models`
> 
> **Class**: Public sealed class inheriting from ClientDto
> 
> **Additional Properties**:
> - FirstName (string)
> - LastName (string)
> - FullName (string) - computed or set from entity
> 
> Add XML documentation

### Task 9.3: Business Client DTO

**Location**: `Application/Common/Models/BusinessClientDto.cs`

**Prompt for Copilot:**

> Create BusinessClientDto class following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Common.Models`
> 
> **Class**: Public sealed class inheriting from ClientDto
> 
> **Additional Properties**:
> - NIPT (string)
> - OwnerFirstName (string?)
> - OwnerLastName (string?)
> - OwnerPhoneNumber (string?)
> - OwnerFullName (string?)
> - ContactPersonFirstName (string)
> - ContactPersonLastName (string)
> - ContactPersonPhoneNumber (string?)
> - ContactPersonFullName (string)
> 
> Add XML documentation

---

## Task 10: Create Individual Client Commands

### Task 10.1: Create Individual Client Command

**Folder**: Create `Application/Features/Clients/Commands/CreateIndividualClient/`

**Location**: `CreateIndividualClientCommand.cs`

**Prompt for Copilot:**

> Create CreateIndividualClientCommand record following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Features.Clients.Commands.CreateIndividualClient`
> 
> **Record**: Public sealed record implementing `IRequest<string>` (returns client ID)
> 
> **Properties**:
> - FirstName (string)
> - LastName (string)
> - Address (string?)
> - Email (string?)
> - PhoneNumber (string?)
> - Notes (string?)
> 
> Add XML documentation

**Location**: `CreateIndividualClientCommandValidator.cs`

**Prompt for Copilot:**

> Create CreateIndividualClientCommandValidator following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Features.Clients.Commands.CreateIndividualClient`
> 
> **Class**: Inherits `AbstractValidator<CreateIndividualClientCommand>`
> 
> **Validation Rules**:
> - FirstName: NotEmpty, MaxLength(50), MinLength(1)
> - LastName: NotEmpty, MaxLength(50), MinLength(1)
> - Address: MaxLength(500) when not null
> - Email: Must be valid email format when not null/empty (use EmailAddress())
> - PhoneNumber: MaxLength(20), match regex pattern for phone when not null
>   - Pattern: `^[\d\s\+\-\(\)]+$` (digits, spaces, +, -, (, ) only)
> - Notes: No max length
> 
> Add XML documentation

**Location**: `CreateIndividualClientCommandHandler.cs`

**Prompt for Copilot:**

> Create CreateIndividualClientCommandHandler following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Features.Clients.Commands.CreateIndividualClient`
> 
> **Class**: Implements `IRequestHandler<CreateIndividualClientCommand, string>`
> 
> **Dependencies** (inject via constructor):
> - IApplicationDbContext context
> - ICurrentUserService currentUserService (to get current user ID)
> 
> **Handle method logic**:
> 1. Get current user ID from currentUserService
> 2. Create new IndividualClient entity:
>    - Pass firstName, lastName, address, email, phoneNumber, notes, createdBy
>    - Entity constructor will validate and throw exceptions if needed
> 3. Add to context.Clients
> 4. Save changes
> 5. Return client.Id
> 
> **Exception handling**:
> - Let domain exceptions bubble up (InvalidClientDataException, etc.)
> - Validation is done by entity constructor
> 
> Add XML documentation

---

### Task 10.2: Update Individual Client Command

**Folder**: Create `Application/Features/Clients/Commands/UpdateIndividualClient/`

**Location**: `UpdateIndividualClientCommand.cs`

**Prompt for Copilot:**

> Create UpdateIndividualClientCommand record following these specifications:
> 
> **Record**: Public sealed record implementing `IRequest<Unit>`
> 
> **Properties**:
> - ClientId (string) - the client to update
> - FirstName (string)
> - LastName (string)
> - Address (string?)
> - Email (string?)
> - PhoneNumber (string?)
> - Notes (string?)
> 
> Add XML documentation

**Location**: `UpdateIndividualClientCommandValidator.cs`

**Prompt for Copilot:**

> Create validator with same rules as Create command, plus:
> - ClientId: NotEmpty
> 
> Add XML documentation

**Location**: `UpdateIndividualClientCommandHandler.cs`

**Prompt for Copilot:**

> Create UpdateIndividualClientCommandHandler following these specifications:
> 
> **Class**: Implements `IRequestHandler<UpdateIndividualClientCommand, Unit>`
> 
> **Dependencies**:
> - IApplicationDbContext context
> - ICurrentUserService currentUserService
> 
> **Handle method logic**:
> 1. Get current user ID
> 2. Find client by ID:
>    - Query: context.Clients.OfType<IndividualClient>().FirstOrDefaultAsync(c => c.Id == request.ClientId)
> 3. If not found, throw NotFoundException("Client", request.ClientId)
> 4. Call client.UpdatePersonalInfo(...) with all request properties
> 5. Set client.UpdatedBy = current user ID
> 6. Save changes
> 7. Return Unit.Value
> 
> Add XML documentation

---

## Task 11: Create Business Client Commands

### Task 11.1: Create Business Client Command

**Folder**: Create `Application/Features/Clients/Commands/CreateBusinessClient/`

**Location**: `CreateBusinessClientCommand.cs`

**Prompt for Copilot:**

> Create CreateBusinessClientCommand record following these specifications:
> 
> **Record**: Public sealed record implementing `IRequest<string>`
> 
> **Properties**:
> - NIPT (string)
> - OwnerFirstName (string?)
> - OwnerLastName (string?)
> - OwnerPhoneNumber (string?)
> - ContactPersonFirstName (string)
> - ContactPersonLastName (string)
> - ContactPersonPhoneNumber (string?)
> - Address (string?)
> - Email (string?)
> - PhoneNumber (string?)
> - Notes (string?)
> 
> Add XML documentation

**Location**: `CreateBusinessClientCommandValidator.cs`

**Prompt for Copilot:**

> Create CreateBusinessClientCommandValidator following these specifications:
> 
> **Class**: Inherits `AbstractValidator<CreateBusinessClientCommand>`
> 
> **Validation Rules**:
> - NIPT: NotEmpty, Length(10), must be alphanumeric
>   - Regex: `^[A-Za-z0-9]{10}$`
> - OwnerFirstName: MaxLength(50) when not null
> - OwnerLastName: MaxLength(50) when not null
> - OwnerPhoneNumber: MaxLength(20), phone regex when not null
> - ContactPersonFirstName: NotEmpty, MaxLength(50), MinLength(1)
> - ContactPersonLastName: NotEmpty, MaxLength(50), MinLength(1)
> - ContactPersonPhoneNumber: MaxLength(20), phone regex when not null
> - Address: MaxLength(500) when not null
> - Email: Valid email when not null
> - PhoneNumber: MaxLength(20), phone regex when not null
> - Notes: No max length
> 
> Add XML documentation

**Location**: `CreateBusinessClientCommandHandler.cs`

**Prompt for Copilot:**

> Create CreateBusinessClientCommandHandler following these specifications:
> 
> **Class**: Implements `IRequestHandler<CreateBusinessClientCommand, string>`
> 
> **Dependencies**:
> - IApplicationDbContext context
> - ICurrentUserService currentUserService
> 
> **Handle method logic**:
> 1. Get current user ID
> 2. Check if NIPT already exists:
>    - Query: await context.Clients.OfType<BusinessClient>().AnyAsync(c => c.NIPT.Value == request.NIPT)
>    - If exists, throw DuplicateNIPTException(request.NIPT)
> 3. Create NIPT value object: new NIPT(request.NIPT)
> 4. Create new BusinessClient entity with all properties
> 5. Add to context.Clients
> 6. Save changes
> 7. Return client.Id
> 
> Add XML documentation

---

### Task 11.2: Update Business Client Command

**Folder**: Create `Application/Features/Clients/Commands/UpdateBusinessClient/`

**Location**: `UpdateBusinessClientCommand.cs`

**Prompt for Copilot:**

> Create UpdateBusinessClientCommand record following these specifications:
> 
> **Record**: Public sealed record implementing `IRequest<Unit>`
> 
> **Properties**:
> - ClientId (string)
> - NIPT (string)
> - OwnerFirstName (string?)
> - OwnerLastName (string?)
> - OwnerPhoneNumber (string?)
> - ContactPersonFirstName (string)
> - ContactPersonLastName (string)
> - ContactPersonPhoneNumber (string?)
> - Address (string?)
> - Email (string?)
> - PhoneNumber (string?)
> - Notes (string?)
> 
> Add XML documentation

**Location**: `UpdateBusinessClientCommandValidator.cs`

**Prompt for Copilot:**

> Create validator with same rules as Create command, plus:
> - ClientId: NotEmpty
> 
> Add XML documentation

**Location**: `UpdateBusinessClientCommandHandler.cs`

**Prompt for Copilot:**

> Create UpdateBusinessClientCommandHandler following these specifications:
> 
> **Class**: Implements `IRequestHandler<UpdateBusinessClientCommand, Unit>`
> 
> **Dependencies**:
> - IApplicationDbContext context
> - ICurrentUserService currentUserService
> 
> **Handle method logic**:
> 1. Get current user ID
> 2. Find client by ID:
>    - Query: context.Clients.OfType<BusinessClient>().FirstOrDefaultAsync(c => c.Id == request.ClientId)
> 3. If not found, throw NotFoundException("Client", request.ClientId)
> 4. If NIPT is changing, check for duplicates:
>    - If request.NIPT != client.NIPT.Value:
>      - Check if new NIPT exists on different client
>      - Throw DuplicateNIPTException if found
> 5. Create new NIPT value object if changed
> 6. Call client.UpdateBusinessInfo(...) with all properties
> 7. Set client.UpdatedBy = current user ID
> 8. Save changes
> 9. Return Unit.Value
> 
> Add XML documentation

---

## Task 12: Create Delete Client Command

**Folder**: Create `Application/Features/Clients/Commands/DeleteClient/`

**Location**: `DeleteClientCommand.cs`

**Prompt for Copilot:**

> Create DeleteClientCommand record following these specifications:
> 
> **Record**: Public sealed record implementing `IRequest<Unit>`
> 
> **Properties**:
> - ClientId (string)
> 
> Add XML documentation

**Location**: `DeleteClientCommandValidator.cs`

**Prompt for Copilot:**

> Create DeleteClientCommandValidator following these specifications:
> 
> **Class**: Inherits `AbstractValidator<DeleteClientCommand>`
> 
> **Dependencies**:
> - IApplicationDbContext context (injected)
> 
> **Validation Rules**:
> - ClientId: NotEmpty
> - ClientId: MustAsync(ExistAsync) - "Client not found"
> 
> **Method**:
> - **ExistAsync**: Check if client exists and is active
> 
> Add XML documentation

**Location**: `DeleteClientCommandHandler.cs`

**Prompt for Copilot:**

> Create DeleteClientCommandHandler following these specifications:
> 
> **Class**: Implements `IRequestHandler<DeleteClientCommand, Unit>`
> 
> **Dependencies**:
> - IApplicationDbContext context
> - ICurrentUserService currentUserService
> 
> **Handle method logic**:
> 1. Get current user ID
> 2. Find client: context.Clients.FirstOrDefaultAsync(c => c.Id == request.ClientId)
> 3. If not found, throw NotFoundException("Client", request.ClientId)
> 4. Call client.Deactivate() (soft delete)
> 5. Set client.UpdatedBy = current user ID
> 6. Save changes
> 7. Return Unit.Value
> 
> **Note**: This is a soft delete (sets IsActive = false)
> 
> Add XML documentation

---

## Task 13: Create Query Handlers

### Task 13.1: Get All Clients Query

**Folder**: Create `Application/Features/Clients/Queries/GetAllClients/`

**Location**: `GetAllClientsQuery.cs`

**Prompt for Copilot:**

> Create GetAllClientsQuery record following these specifications:
> 
> **Record**: Public sealed record implementing `IRequest<List<ClientDto>>`
> 
> **Properties**:
> - IncludeInactive (bool) - default false
> 
> Add XML documentation

**Location**: `GetAllClientsQueryHandler.cs`

**Prompt for Copilot:**

> Create GetAllClientsQueryHandler following these specifications:
> 
> **Class**: Implements `IRequestHandler<GetAllClientsQuery, List<ClientDto>>`
> 
> **Dependencies**:
> - IApplicationDbContext context
> 
> **Handle method logic**:
> 1. Query all clients: context.Clients.AsQueryable()
> 2. If !request.IncludeInactive, filter where IsActive == true
> 3. Order by ClientType, then by CreatedAt descending
> 4. For each client, determine type (Individual or Business)
> 5. Project to appropriate DTO:
>    - If IndividualClient → IndividualClientDto
>    - If BusinessClient → BusinessClientDto
> 6. Map all properties including common properties from base
> 7. Return List<ClientDto>
> 
> **Use pattern matching** for type-specific mapping
> 
> Add XML documentation

---

### Task 13.2: Get Client By Id Query

**Folder**: Create `Application/Features/Clients/Queries/GetClientById/`

**Location**: `GetClientByIdQuery.cs`

**Prompt for Copilot:**

> Create GetClientByIdQuery record following these specifications:
> 
> **Record**: Public sealed record implementing `IRequest<ClientDto>`
> 
> **Properties**:
> - ClientId (string)
> 
> Add XML documentation

**Location**: `GetClientByIdQueryHandler.cs`

**Prompt for Copilot:**

> Create GetClientByIdQueryHandler following these specifications:
> 
> **Class**: Implements `IRequestHandler<GetClientByIdQuery, ClientDto>`
> 
> **Dependencies**:
> - IApplicationDbContext context
> 
> **Handle method logic**:
> 1. Find client: context.Clients.FirstOrDefaultAsync(c => c.Id == request.ClientId)
> 2. If not found, throw NotFoundException("Client", request.ClientId)
> 3. Determine actual type (Individual or Business)
> 4. Map to appropriate DTO (IndividualClientDto or BusinessClientDto)
> 5. Return DTO
> 
> Add XML documentation

---

### Task 13.3: Get Clients By Type Query

**Folder**: Create `Application/Features/Clients/Queries/GetClientsByType/`

**Location**: `GetClientsByTypeQuery.cs`

**Prompt for Copilot:**

> Create GetClientsByTypeQuery record following these specifications:
> 
> **Record**: Public sealed record implementing `IRequest<List<ClientDto>>`
> 
> **Properties**:
> - ClientTypeId (int) - 1 for Individual, 2 for Business
> - IncludeInactive (bool) - default false
> 
> Add XML documentation

**Location**: `GetClientsByTypeQueryHandler.cs`

**Prompt for Copilot:**

> Create GetClientsByTypeQueryHandler following these specifications:
> 
> **Class**: Implements `IRequestHandler<GetClientsByTypeQuery, List<ClientDto>>`
> 
> **Handle method logic**:
> 1. Convert ClientTypeId to ClientType enum
> 2. Query clients filtered by ClientType
> 3. Filter by IsActive if needed
> 4. Order by CreatedAt descending
> 5. Map to appropriate DTOs
> 6. Return list
> 
> Add XML documentation

---

### Task 13.4: Search Clients Query

**Folder**: Create `Application/Features/Clients/Queries/SearchClients/`

**Location**: `SearchClientsQuery.cs`

**Prompt for Copilot:**

> Create SearchClientsQuery record following these specifications:
> 
> **Record**: Public sealed record implementing `IRequest<List<ClientDto>>`
> 
> **Properties**:
> - SearchTerm (string?) - search in names, NIPT, email, phone
> - ClientTypeId (int?) - optional filter by type
> - IncludeInactive (bool) - default false
> 
> Add XML documentation

**Location**: `SearchClientsQueryHandler.cs`

**Prompt for Copilot:**

> Create SearchClientsQueryHandler following these specifications:
> 
> **Class**: Implements `IRequestHandler<SearchClientsQuery, List<ClientDto>>`
> 
> **Handle method logic**:
> 1. Start with all clients query
> 2. Filter by IsActive if needed
> 3. Filter by ClientType if provided
> 4. If SearchTerm is provided and not empty:
>    - Search in IndividualClient: FirstName, LastName, Email, PhoneNumber
>    - Search in BusinessClient: NIPT.Value, ContactPersonFirstName, ContactPersonLastName, 
>      OwnerFirstName, OwnerLastName, Email, PhoneNumber
>    - Use case-insensitive contains
> 5. Order by relevance (clients where search term appears in name first)
> 6. Map to appropriate DTOs
> 7. Return list
> 
> Add XML documentation

---

## Task 14: Create Client Mapper Helper

**Location**: `Application/Features/Clients/ClientMapper.cs`

**Prompt for Copilot:**

> Create ClientMapper static class to centralize client-to-DTO mapping:
> 
> **Namespace**: `InventoryManagement.Application.Features.Clients`
> 
> **Methods**:
> 
> 1. **MapToDto(Client client)** → ClientDto:
>    - Use pattern matching (switch expression) to determine client type
>    - Call MapIndividual or MapBusiness accordingly
>    - Return base ClientDto if unknown type (shouldn't happen)
> 
> 2. **MapIndividual(IndividualClient client)** → IndividualClientDto:
>    - Create new IndividualClientDto
>    - Map all common properties from Client base
>    - Map FirstName, LastName, FullName
>    - Return DTO
> 
> 3. **MapBusiness(BusinessClient client)** → BusinessClientDto:
>    - Create new BusinessClientDto
>    - Map all common properties from Client base
>    - Map NIPT.Value → NIPT property
>    - Map all owner properties
>    - Map all contact person properties
>    - Return DTO
> 
> Add XML documentation

---

## Task 15: Unit Tests for Application Layer

### Location: `Application.Tests/Features/Clients/Commands/CreateIndividualClientCommandHandlerTests.cs`

**Prompt for Copilot:**

> Create unit tests for CreateIndividualClientCommandHandler:
> 
> 1. **Handle_WithValidCommand_ShouldCreateClient**
>    - Arrange: Valid command with firstName, lastName
>    - Act: Call handler
>    - Assert: Client created, ID returned
> 
> 2. **Handle_WithInvalidEmail_ShouldThrowException**
>    - Arrange: Command with invalid email format
>    - Act: Call handler
>    - Assert: InvalidClientDataException thrown
> 
> 3. **Handle_WithEmptyFirstName_ShouldThrowException**
>    - Test through validator or entity constructor
> 
> 4. **Handle_ShouldSetCreatedAtAndCreatedBy**
>    - Verify audit fields are set correctly

### Location: `Application.Tests/Features/Clients/Commands/CreateBusinessClientCommandHandlerTests.cs`

**Prompt for Copilot:**

> Create unit tests for CreateBusinessClientCommandHandler:
> 
> 1. **Handle_WithValidCommand_ShouldCreateClient**
> 2. **Handle_WithDuplicateNIPT_ShouldThrowDuplicateNIPTException**
> 3. **Handle_WithInvalidNIPT_ShouldThrowException**
> 4. **Handle_ShouldValidateContactPersonNames**

### Location: `Application.Tests/Features/Clients/Commands/UpdateIndividualClientCommandHandlerTests.cs`

**Prompt for Copilot:**

> Create unit tests for UpdateIndividualClientCommandHandler:
> 
> 1. **Handle_WithValidCommand_ShouldUpdateClient**
> 2. **Handle_WithNonExistentClient_ShouldThrowNotFoundException**
> 3. **Handle_ShouldUpdateUpdatedAtAndUpdatedBy**

### Location: `Application.Tests/Features/Clients/Commands/DeleteClientCommandHandlerTests.cs`

**Prompt for Copilot:**

> Create unit tests for DeleteClientCommandHandler:
> 
> 1. **Handle_WithValidId_ShouldSoftDeleteClient**
>    - Assert: IsActive becomes false
> 2. **Handle_WithNonExistentClient_ShouldThrowNotFoundException**

---

## Verification Checklist

Before moving to infrastructure and API layers, verify:

**Domain Layer:**
- [ ] ClientType enum created
- [ ] PersonName value object created with validation
- [ ] NIPT value object created with validation
- [ ] DuplicateNIPTException created
- [ ] InvalidClientDataException created
- [ ] Client base entity created with common fields
- [ ] IndividualClient entity created
- [ ] BusinessClient entity created
- [ ] Email validation method works
- [ ] Phone validation method works

**Application Layer:**
- [ ] IApplicationDbContext updated with Clients DbSet
- [ ] ClientDto base class created
- [ ] IndividualClientDto created
- [ ] BusinessClientDto created
- [ ] CreateIndividualClient command/validator/handler created
- [ ] CreateBusinessClient command/validator/handler created
- [ ] UpdateIndividualClient command/validator/handler created
- [ ] UpdateBusinessClient command/validator/handler created
- [ ] DeleteClient command/validator/handler created (soft delete)
- [ ] GetAllClients query/handler created
- [ ] GetClientById query/handler created
- [ ] GetClientsByType query/handler created
- [ ] SearchClients query/handler created
- [ ] ClientMapper helper created
- [ ] Unit tests created for all handlers
- [ ] Solution builds without errors
- [ ] All tests pass

---

## What You've Accomplished

✅ **Domain Layer** with proper entities and value objects  
✅ **ClientType enum** for Individual and Business  
✅ **Value Objects** (PersonName, NIPT) with validation  
✅ **Domain Exceptions** for business rule violations  
✅ **DTOs** for all client types  
✅ **Create Commands** for both client types  
✅ **Update Commands** for both client types  
✅ **Delete Command** with soft delete  
✅ **Query Handlers** for retrieving and searching clients  
✅ **Client Mapper** for DTO conversion  
✅ **Validators** with comprehensive rules  
✅ **Unit Tests** for handlers  

---

## Key Patterns Used

### 1. Type-Specific Entities
Separate entities for Individual and Business clients with specific properties.

### 2. Value Objects
NIPT and PersonName encapsulate validation logic and ensure correctness.

### 3. Soft Delete
Clients are deactivated (IsActive = false) instead of deleted, preserving data.

### 4. NIPT Uniqueness
Business clients have unique NIPT validation to prevent duplicates.

### 5. Comprehensive Validation
- Entity-level validation in constructors
- FluentValidation in command validators
- Email and phone format validation

### 6. Audit Trail
CreatedAt, UpdatedAt, CreatedBy, UpdatedBy track all changes.

---

## Next Steps

Ready for **Step 6B: Infrastructure & API Layer** where you'll create:
- EF Core configurations for Client hierarchy (TPH)
- Database migration
- API Controllers for Clients
- CRUD endpoints
- Search endpoint
- Integration tests
- Swagger documentation

---

## Tips

1. **Start with Individual clients** - Simpler structure, test end-to-end first
2. **Test NIPT uniqueness** - Critical business rule for Business clients
3. **Validate emails and phones** - Use regex or built-in validators
4. **Test soft delete** - Ensure IsActive flag works correctly
5. **Use value objects** - NIPT and PersonName encapsulate validation
6. **Search functionality** - Test with various search terms
7. **Type safety** - Use OfType<IndividualClient>() and OfType<BusinessClient>()

Good luck! 🚀
