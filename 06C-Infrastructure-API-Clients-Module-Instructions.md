# Step 6C: Infrastructure & API Layer - Clients Module
## Instructions for GitHub Copilot

---

## Overview
This guide provides instructions for building the **Infrastructure and API layers** for the Clients module. We'll implement EF Core configurations, database migrations, API controllers, and integration tests.

**What we'll build:**
- EF Core entity configurations (TPH strategy)
- Database migration for Clients table
- API Controllers with CRUD endpoints
- Search endpoint
- Integration tests
- Swagger documentation

**Time estimate:** 2-3 hours

---

## Part 1: Infrastructure Layer

### Folder Structure

Create this folder structure in `InventoryManagement.Infrastructure`:

```
Infrastructure/
├── Persistence/
│   ├── Configurations/
│   │   ├── ClientConfiguration.cs
│   │   ├── IndividualClientConfiguration.cs
│   │   └── BusinessClientConfiguration.cs
│   └── ApplicationDbContext.cs  # Update this
└── Migrations/
    └── [timestamp]_AddClients.cs  # Generated
```

---

## Task 1: Create Client Base Configuration

### Location: `Infrastructure/Persistence/Configurations/ClientConfiguration.cs`

**Prompt for Copilot:**

> Create ClientConfiguration for EF Core following these specifications:
> 
> **Namespace**: `InventoryManagement.Infrastructure.Persistence.Configurations`
> 
> **Class**: Implements `IEntityTypeConfiguration<Client>`
> 
> **Configure method requirements**:
> - Table name: "Clients"
> - Primary key: Id (string, max 36 characters)
> - ClientType: Store as int, required
> - Address: max 500 characters, optional
> - Email: max 100 characters, optional
> - PhoneNumber: max 20 characters, optional
> - Notes: nvarchar(max), optional
> - CreatedAt, UpdatedAt: required DateTime
> - CreatedBy: max 36 characters, required
> - UpdatedBy: max 36 characters, optional
> - IsActive: required bool, default value true
> 
> **TPH (Table Per Hierarchy) strategy**:
> - Use HasDiscriminator on ClientType
> - IndividualClient = 1
> - BusinessClient = 2
> 
> **Indexes**:
> - ClientType
> - IsActive
> - Email
> - CreatedAt
> 
> Add XML documentation

---

## Task 2: Create IndividualClient Configuration

### Location: `Infrastructure/Persistence/Configurations/IndividualClientConfiguration.cs`

**Prompt for Copilot:**

> Create IndividualClientConfiguration for EF Core following these specifications:
> 
> **Namespace**: `InventoryManagement.Infrastructure.Persistence.Configurations`
> 
> **Class**: Implements `IEntityTypeConfiguration<IndividualClient>`
> 
> **Configure method requirements**:
> - FirstName: max 50 characters, required
> - LastName: max 50 characters, required
> - FullName: Ignore this property (computed in entity)
> 
> **Indexes**:
> - FirstName
> - LastName
> - Composite index on (FirstName, LastName)
> 
> Add XML documentation

---

## Task 3: Create BusinessClient Configuration

### Location: `Infrastructure/Persistence/Configurations/BusinessClientConfiguration.cs`

**Prompt for Copilot:**

> Create BusinessClientConfiguration for EF Core following these specifications:
> 
> **Namespace**: `InventoryManagement.Infrastructure.Persistence.Configurations`
> 
> **Class**: Implements `IEntityTypeConfiguration<BusinessClient>`
> 
> **Configure method requirements**:
> 
> **NIPT Value Object**:
> - Use OwnsOne for NIPT property
> - Map NIPT.Value to column name "NIPT"
> - Max 10 characters, required
> 
> **Owner properties**:
> - OwnerFirstName: max 50 characters, optional
> - OwnerLastName: max 50 characters, optional
> - OwnerPhoneNumber: max 20 characters, optional
> 
> **Contact person properties**:
> - ContactPersonFirstName: max 50 characters, required
> - ContactPersonLastName: max 50 characters, required
> - ContactPersonPhoneNumber: max 20 characters, optional
> 
> **Computed properties** (ignore these):
> - OwnerFullName
> - ContactPersonFullName
> 
> **Unique constraint**:
> - Create unique index on NIPT column
> - Filter: Only for active clients ([IsActive] = 1)
> 
> **Indexes**:
> - ContactPersonFirstName
> - ContactPersonLastName
> 
> Add XML documentation

---

## Task 4: Update ApplicationDbContext

### Location: `Infrastructure/Persistence/ApplicationDbContext.cs`

**Prompt for Copilot:**

> Update ApplicationDbContext to include Clients:
> 
> **Add DbSet property**:
> - `public DbSet<Client> Clients => Set<Client>();`
> 
> **In OnModelCreating method**:
> - Apply ClientConfiguration
> - Apply IndividualClientConfiguration
> - Apply BusinessClientConfiguration
> 
> Keep existing configurations for Users, Products, StockHistories

---

## Task 5: Create Database Migration

### Location: Use Package Manager Console or .NET CLI

**Prompt for Copilot:**

> Create a migration for the Clients table:
> 
> **Using Package Manager Console**:
> ```
> Add-Migration AddClients -Project InventoryManagement.Infrastructure -StartupProject InventoryManagement.WebAPI
> ```
> 
> **Using .NET CLI**:
> ```
> dotnet ef migrations add AddClients --project Infrastructure --startup-project WebAPI
> ```
> 
> **Migration should create**:
> - Clients table with all columns
> - ClientType discriminator column (int)
> - Columns for IndividualClient: FirstName, LastName
> - Columns for BusinessClient: NIPT, Owner fields, ContactPerson fields
> - All indexes defined in configurations
> - Unique constraint on NIPT with filter
> 
> **Review the generated migration** before applying
> 
> **Apply migration**:
> ```
> Update-Database
> ```
> or
> ```
> dotnet ef database update
> ```

---

## Part 2: API Layer

### Folder Structure

Create this folder structure in `InventoryManagement.WebAPI`:

```
WebAPI/
├── Controllers/
│   └── ClientsController.cs
└── Program.cs  # May need to register services
```

---

## Task 6: Create Clients Controller

### Location: `WebAPI/Controllers/ClientsController.cs`

**Prompt for Copilot:**

> Create ClientsController following these specifications:
> 
> **Namespace**: `InventoryManagement.WebAPI.Controllers`
> 
> **Class**: 
> - Inherits: `ApiControllerBase`
> - Attributes: `[ApiController]`, `[Route("api/[controller]")]`, `[Authorize]`
> 
> **Dependencies** (inject via constructor):
> - IMediator mediator
> 
> **Endpoints**:
> 
> ### 1. Get All Clients
> **Route**: `GET api/clients`
> **Query parameters**: includeInactive (bool, default false)
> **Action**:
> - Send GetAllClientsQuery with includeInactive
> - Return Ok with List<ClientDto>
> 
> ### 2. Get Client By Id
> **Route**: `GET api/clients/{id}`
> **Parameters**: id (string)
> **Action**:
> - Send GetClientByIdQuery with id
> - Return Ok with ClientDto
> - Return NotFound if client doesn't exist
> 
> ### 3. Get Clients By Type
> **Route**: `GET api/clients/type/{clientTypeId}`
> **Parameters**: clientTypeId (int - 1 or 2)
> **Query parameters**: includeInactive (bool, default false)
> **Action**:
> - Send GetClientsByTypeQuery
> - Return Ok with List<ClientDto>
> 
> ### 4. Search Clients
> **Route**: `GET api/clients/search`
> **Query parameters**: 
> - searchTerm (string, optional)
> - clientTypeId (int, optional)
> - includeInactive (bool, default false)
> **Action**:
> - Send SearchClientsQuery
> - Return Ok with List<ClientDto>
> 
> ### 5. Create Individual Client
> **Route**: `POST api/clients/individual`
> **Body**: CreateIndividualClientCommand
> **Action**:
> - Send CreateIndividualClientCommand
> - Return CreatedAtAction with new client ID
> - Location header points to GetById endpoint
> 
> ### 6. Create Business Client
> **Route**: `POST api/clients/business`
> **Body**: CreateBusinessClientCommand
> **Action**:
> - Send CreateBusinessClientCommand
> - Return CreatedAtAction with new client ID
> - Location header points to GetById endpoint
> 
> ### 7. Update Individual Client
> **Route**: `PUT api/clients/individual/{id}`
> **Parameters**: id (string)
> **Body**: UpdateIndividualClientCommand (without clientId)
> **Action**:
> - Create command with id from route
> - Send UpdateIndividualClientCommand
> - Return NoContent (204)
> 
> ### 8. Update Business Client
> **Route**: `PUT api/clients/business/{id}`
> **Parameters**: id (string)
> **Body**: UpdateBusinessClientCommand (without clientId)
> **Action**:
> - Create command with id from route
> - Send UpdateBusinessClientCommand
> - Return NoContent (204)
> 
> ### 9. Delete Client
> **Route**: `DELETE api/clients/{id}`
> **Parameters**: id (string)
> **Action**:
> - Send DeleteClientCommand
> - Return NoContent (204)
> - This is a soft delete
> 
> **Swagger documentation**: Add XML comments for all endpoints
> 
> **Error handling**: Exception filter will handle domain exceptions
> 
> Add XML documentation for the class and all methods

---

## Task 7: Update Exception Handling (if needed)

### Location: `WebAPI/Filters/ApiExceptionFilterAttribute.cs` or equivalent

**Prompt for Copilot:**

> Ensure exception handling covers Client-specific exceptions:
> 
> **Exceptions to handle**:
> - **DuplicateNIPTException**: Return 400 Bad Request with error message
> - **InvalidClientDataException**: Return 400 Bad Request with error message
> - **NotFoundException**: Return 404 Not Found
> 
> If these are already handled by DomainException handling, no changes needed.
> Otherwise, add specific handlers for these exceptions.

---

## Part 3: Integration Tests

### Folder Structure

Create this folder structure in `InventoryManagement.Application.IntegrationTests`:

```
Application.IntegrationTests/
├── Clients/
│   ├── Commands/
│   │   ├── CreateIndividualClientCommandTests.cs
│   │   ├── CreateBusinessClientCommandTests.cs
│   │   ├── UpdateIndividualClientCommandTests.cs
│   │   ├── UpdateBusinessClientCommandTests.cs
│   │   └── DeleteClientCommandTests.cs
│   └── Queries/
│       ├── GetAllClientsQueryTests.cs
│       ├── GetClientByIdQueryTests.cs
│       ├── GetClientsByTypeQueryTests.cs
│       └── SearchClientsQueryTests.cs
```

---

## Task 8: Create Integration Tests - Create Commands

### Location: `Application.IntegrationTests/Clients/Commands/CreateIndividualClientCommandTests.cs`

**Prompt for Copilot:**

> Create integration tests for CreateIndividualClientCommand:
> 
> **Test class**: CreateIndividualClientCommandTests : TestBase
> 
> **Tests**:
> 
> 1. **CreateIndividualClient_WithValidData_ShouldSucceed**
>    - Arrange: Create valid command with firstName, lastName, email, phone
>    - Act: Send command via mediator
>    - Assert: 
>      - Client ID returned
>      - Client exists in database
>      - All properties saved correctly
>      - CreatedAt is set
>      - IsActive is true
> 
> 2. **CreateIndividualClient_WithMinimalData_ShouldSucceed**
>    - Arrange: Command with only firstName and lastName (required fields)
>    - Act: Send command
>    - Assert: Client created successfully
> 
> 3. **CreateIndividualClient_WithInvalidEmail_ShouldFail**
>    - Arrange: Command with invalid email format
>    - Act: Send command
>    - Assert: ValidationException thrown
> 
> 4. **CreateIndividualClient_WithEmptyFirstName_ShouldFail**
>    - Arrange: Command with empty firstName
>    - Act: Send command
>    - Assert: ValidationException thrown
> 
> 5. **CreateIndividualClient_WithInvalidPhoneNumber_ShouldFail**
>    - Arrange: Command with invalid phone format
>    - Act: Send command
>    - Assert: ValidationException or InvalidClientDataException thrown

### Location: `Application.IntegrationTests/Clients/Commands/CreateBusinessClientCommandTests.cs`

**Prompt for Copilot:**

> Create integration tests for CreateBusinessClientCommand:
> 
> **Test class**: CreateBusinessClientCommandTests : TestBase
> 
> **Tests**:
> 
> 1. **CreateBusinessClient_WithValidData_ShouldSucceed**
>    - Arrange: Valid command with NIPT, contact person, owner
>    - Act: Send command
>    - Assert: Client created with all data
> 
> 2. **CreateBusinessClient_WithDuplicateNIPT_ShouldFail**
>    - Arrange: 
>      - Create first business client with NIPT "K123456789"
>      - Create command with same NIPT
>    - Act: Send second command
>    - Assert: DuplicateNIPTException thrown
> 
> 3. **CreateBusinessClient_WithInvalidNIPT_ShouldFail**
>    - Arrange: Command with NIPT not 10 characters
>    - Act: Send command
>    - Assert: ValidationException thrown
> 
> 4. **CreateBusinessClient_WithoutContactPerson_ShouldFail**
>    - Arrange: Command without contact person first or last name
>    - Act: Send command
>    - Assert: ValidationException thrown
> 
> 5. **CreateBusinessClient_WithoutOwner_ShouldSucceed**
>    - Arrange: Command without owner information (optional)
>    - Act: Send command
>    - Assert: Client created successfully

---

## Task 9: Create Integration Tests - Update Commands

### Location: `Application.IntegrationTests/Clients/Commands/UpdateIndividualClientCommandTests.cs`

**Prompt for Copilot:**

> Create integration tests for UpdateIndividualClientCommand:
> 
> **Tests**:
> 
> 1. **UpdateIndividualClient_WithValidData_ShouldSucceed**
>    - Arrange: Create client, then create update command with new data
>    - Act: Send update command
>    - Assert: 
>      - Client updated in database
>      - UpdatedAt changed
>      - All fields updated correctly
> 
> 2. **UpdateIndividualClient_NonExistent_ShouldFail**
>    - Arrange: Update command with non-existent ID
>    - Act: Send command
>    - Assert: NotFoundException thrown
> 
> 3. **UpdateIndividualClient_WithInvalidData_ShouldFail**
>    - Arrange: Update command with invalid email
>    - Act: Send command
>    - Assert: ValidationException thrown

### Location: `Application.IntegrationTests/Clients/Commands/UpdateBusinessClientCommandTests.cs`

**Prompt for Copilot:**

> Create integration tests for UpdateBusinessClientCommand:
> 
> **Tests**:
> 
> 1. **UpdateBusinessClient_WithValidData_ShouldSucceed**
>    - Similar to individual update test
> 
> 2. **UpdateBusinessClient_ChangingNIPTToDuplicate_ShouldFail**
>    - Arrange:
>      - Create business client 1 with NIPT "K111111111"
>      - Create business client 2 with NIPT "K222222222"
>      - Update command for client 2 with NIPT "K111111111"
>    - Act: Send update command
>    - Assert: DuplicateNIPTException thrown
> 
> 3. **UpdateBusinessClient_ChangingNIPTToNew_ShouldSucceed**
>    - Arrange: Create client, update with new unique NIPT
>    - Act: Send command
>    - Assert: NIPT updated successfully

---

## Task 10: Create Integration Tests - Delete Command

### Location: `Application.IntegrationTests/Clients/Commands/DeleteClientCommandTests.cs`

**Prompt for Copilot:**

> Create integration tests for DeleteClientCommand:
> 
> **Tests**:
> 
> 1. **DeleteClient_ExistingClient_ShouldSoftDelete**
>    - Arrange: Create client
>    - Act: Send delete command
>    - Assert:
>      - Client still exists in database
>      - IsActive is false
>      - UpdatedAt changed
> 
> 2. **DeleteClient_NonExistent_ShouldFail**
>    - Arrange: Delete command with non-existent ID
>    - Act: Send command
>    - Assert: NotFoundException thrown
> 
> 3. **DeleteClient_AlreadyDeleted_ShouldFail**
>    - Arrange: Create and delete client
>    - Act: Try to delete again
>    - Assert: NotFoundException thrown (validator checks IsActive)

---

## Task 11: Create Integration Tests - Query Handlers

### Location: `Application.IntegrationTests/Clients/Queries/GetAllClientsQueryTests.cs`

**Prompt for Copilot:**

> Create integration tests for GetAllClientsQuery:
> 
> **Tests**:
> 
> 1. **GetAllClients_ReturnsAllActiveClients**
>    - Arrange: Create 2 active and 1 inactive client
>    - Act: Send query with includeInactive = false
>    - Assert: Returns 2 clients
> 
> 2. **GetAllClients_WithIncludeInactive_ReturnsAll**
>    - Arrange: Same setup
>    - Act: Send query with includeInactive = true
>    - Assert: Returns 3 clients
> 
> 3. **GetAllClients_EmptyDatabase_ReturnsEmptyList**
>    - Arrange: No clients
>    - Act: Send query
>    - Assert: Returns empty list

### Location: `Application.IntegrationTests/Clients/Queries/GetClientByIdQueryTests.cs`

**Prompt for Copilot:**

> Create integration tests for GetClientByIdQuery:
> 
> **Tests**:
> 
> 1. **GetClientById_IndividualClient_ReturnsCorrectDto**
>    - Arrange: Create individual client
>    - Act: Query by ID
>    - Assert: 
>      - Returns IndividualClientDto
>      - All properties correct
> 
> 2. **GetClientById_BusinessClient_ReturnsCorrectDto**
>    - Arrange: Create business client
>    - Act: Query by ID
>    - Assert:
>      - Returns BusinessClientDto
>      - NIPT and all fields correct
> 
> 3. **GetClientById_NonExistent_ShouldFail**
>    - Act: Query with non-existent ID
>    - Assert: NotFoundException thrown

### Location: `Application.IntegrationTests/Clients/Queries/GetClientsByTypeQueryTests.cs`

**Prompt for Copilot:**

> Create integration tests for GetClientsByTypeQuery:
> 
> **Tests**:
> 
> 1. **GetClientsByType_Individual_ReturnsOnlyIndividual**
>    - Arrange: Create 2 individual, 2 business clients
>    - Act: Query with clientTypeId = 1
>    - Assert: Returns 2 individual clients only
> 
> 2. **GetClientsByType_Business_ReturnsOnlyBusiness**
>    - Similar test for business clients

### Location: `Application.IntegrationTests/Clients/Queries/SearchClientsQueryTests.cs`

**Prompt for Copilot:**

> Create integration tests for SearchClientsQuery:
> 
> **Tests**:
> 
> 1. **SearchClients_ByFirstName_ReturnsMatches**
>    - Arrange: Create clients with various names
>    - Act: Search by first name
>    - Assert: Returns matching clients
> 
> 2. **SearchClients_ByNIPT_ReturnsBusinessClient**
>    - Arrange: Create business client with specific NIPT
>    - Act: Search by NIPT
>    - Assert: Returns that business client
> 
> 3. **SearchClients_ByEmail_ReturnsMatches**
>    - Arrange: Create clients with emails
>    - Act: Search by email
>    - Assert: Returns clients with matching email
> 
> 4. **SearchClients_WithTypeFilter_ReturnsFilteredResults**
>    - Arrange: Create both types
>    - Act: Search with clientTypeId filter
>    - Assert: Returns only matching type
> 
> 5. **SearchClients_NoResults_ReturnsEmptyList**
>    - Act: Search with non-matching term
>    - Assert: Returns empty list

---

## Verification Checklist

Before considering Infrastructure & API complete, verify:

### Infrastructure:
- [ ] ClientConfiguration created with TPH discriminator
- [ ] IndividualClientConfiguration created
- [ ] BusinessClientConfiguration created with NIPT value object
- [ ] ApplicationDbContext updated with Clients DbSet
- [ ] Configurations applied in OnModelCreating
- [ ] Migration created successfully
- [ ] Migration applied to database
- [ ] Clients table exists with correct columns
- [ ] ClientType discriminator column exists
- [ ] NIPT unique constraint with filter created
- [ ] All indexes created

### API:
- [ ] ClientsController created
- [ ] GET all clients endpoint works
- [ ] GET client by ID endpoint works
- [ ] GET clients by type endpoint works
- [ ] GET search endpoint works
- [ ] POST individual client endpoint works
- [ ] POST business client endpoint works
- [ ] PUT individual client endpoint works
- [ ] PUT business client endpoint works
- [ ] DELETE client endpoint works (soft delete)
- [ ] All endpoints have proper HTTP status codes
- [ ] Swagger documentation generated
- [ ] Exception handling covers client exceptions

### Integration Tests:
- [ ] Create individual client tests pass
- [ ] Create business client tests pass
- [ ] NIPT uniqueness test passes
- [ ] Update individual client tests pass
- [ ] Update business client tests pass
- [ ] NIPT duplicate on update test passes
- [ ] Delete client tests pass (soft delete)
- [ ] Get all clients tests pass
- [ ] Get by ID tests pass
- [ ] Get by type tests pass
- [ ] Search clients tests pass
- [ ] All integration tests pass

---

## What You've Accomplished

✅ **EF Core Configurations** with TPH strategy  
✅ **NIPT Value Object** mapping  
✅ **Database Migration** for Clients table  
✅ **Unique Constraint** on NIPT with active filter  
✅ **RESTful API** with 10 endpoints  
✅ **Separate endpoints** for Individual and Business  
✅ **Search functionality** across multiple fields  
✅ **Soft Delete** implementation  
✅ **Comprehensive Integration Tests** (20+ tests)  
✅ **Swagger Documentation** for all endpoints  

---

## Key Patterns Used

### 1. TPH (Table Per Hierarchy)
All client types in single table with discriminator column.

### 2. Value Object Mapping
NIPT mapped using OwnsOne with custom column name.

### 3. Filtered Unique Index
NIPT unique only for active clients.

### 4. Type-Specific Endpoints
Separate Create/Update endpoints for Individual and Business.

### 5. Soft Delete
IsActive flag prevents permanent data loss.

### 6. RESTful Design
Proper HTTP verbs, status codes, and resource naming.

---

## API Endpoint Summary

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/clients | Get all clients |
| GET | /api/clients/{id} | Get client by ID |
| GET | /api/clients/type/{typeId} | Get clients by type |
| GET | /api/clients/search | Search clients |
| POST | /api/clients/individual | Create individual client |
| POST | /api/clients/business | Create business client |
| PUT | /api/clients/individual/{id} | Update individual client |
| PUT | /api/clients/business/{id} | Update business client |
| DELETE | /api/clients/{id} | Soft delete client |

---

## Database Schema

**Clients Table** (TPH):
```
Columns:
- Id (string, PK)
- ClientType (int, discriminator: 1=Individual, 2=Business)
- Address (string, 500)
- Email (string, 100)
- PhoneNumber (string, 20)
- Notes (nvarchar(max))
- CreatedAt (datetime)
- UpdatedAt (datetime)
- CreatedBy (string, 36)
- UpdatedBy (string, 36)
- IsActive (bit, default 1)

IndividualClient columns:
- FirstName (string, 50)
- LastName (string, 50)

BusinessClient columns:
- NIPT (string, 10, unique when IsActive=1)
- OwnerFirstName (string, 50)
- OwnerLastName (string, 50)
- OwnerPhoneNumber (string, 20)
- ContactPersonFirstName (string, 50)
- ContactPersonLastName (string, 50)
- ContactPersonPhoneNumber (string, 20)

Indexes:
- IX_Clients_ClientType
- IX_Clients_IsActive
- IX_Clients_Email
- IX_Clients_CreatedAt
- IX_Clients_FirstName (Individual)
- IX_Clients_LastName (Individual)
- IX_Clients_FirstName_LastName (Individual, composite)
- IX_Clients_NIPT (Business, unique with filter)
- IX_Clients_ContactPersonFirstName (Business)
- IX_Clients_ContactPersonLastName (Business)
```

---

## Testing Tips

1. **Test NIPT uniqueness** - Critical business rule
2. **Test soft delete** - Verify IsActive flag behavior
3. **Test type-specific endpoints** - Individual vs Business
4. **Test search** - Multiple field combinations
5. **Test validation** - Email, phone, NIPT format
6. **Test updates** - Especially NIPT changes
7. **Use Postman/Swagger** - For manual API testing
8. **Check migration** - Review generated SQL before applying

---

## Common Issues & Solutions

### Issue 1: "NIPT value object not saving"
**Solution**: Ensure OwnsOne configuration maps to column "NIPT" not "NIPT_Value"

### Issue 2: "Duplicate NIPT allowed"
**Solution**: Check unique index has filter [IsActive] = 1

### Issue 3: "FullName not accessible in queries"
**Solution**: These are computed properties, use FirstName + LastName in projections

### Issue 4: "Soft delete not working"
**Solution**: Ensure validator checks IsActive, handler sets IsActive = false

### Issue 5: "Migration fails"
**Solution**: Check all configurations are applied in OnModelCreating

---

## Next Steps

After completing this step, you can:
1. **Test the complete flow** - Create, Read, Update, Delete clients
2. **Move to Frontend** - Implement React UI (Step 6B if not done)
3. **Add features**:
   - Client statistics report
   - Export clients to CSV/Excel
   - Client activity history
   - Link clients to orders/invoices

---

## Tips

1. **Review migration SQL** before applying
2. **Test NIPT uniqueness** thoroughly
3. **Use Swagger** for API documentation
4. **Run integration tests** frequently
5. **Check indexes** are created correctly
6. **Test soft delete** behavior
7. **Validate value objects** work as expected

Good luck! 🚀