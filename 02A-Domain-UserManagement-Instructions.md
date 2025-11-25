# Step 2A: Domain Layer - User Management Module
## Instructions for GitHub Copilot

---

## Overview
This document provides instructions for building the **Domain layer** of the User Management module. The Domain layer is the core of Clean Architecture and contains business entities, value objects, domain events, and exceptions with **zero external dependencies**.

**Important**: This file contains instructions for GitHub Copilot to generate code. Read each section carefully and use Copilot to implement the specifications.

---

## Understanding Clean Architecture - Domain Layer

### What is the Domain Layer?
- **Core of your application** - Contains business logic and rules
- **No dependencies** - Pure C# code, no frameworks, no libraries
- **Framework agnostic** - Can be used with any UI or database
- **Testable** - Easy to unit test without external dependencies

### Key Concepts:

**Entity**:
- Has a unique identity (ID) that persists over time
- Example: User, Product, Order
- Two entities with the same data but different IDs are different
- Mutable - can change state over time through methods

**Value Object**:
- Defined entirely by its values, no unique identity
- Example: Email, Money, Address
- Two value objects with the same values are considered equal
- Immutable - cannot change after creation
- Should validate itself on creation

**Domain Event**:
- Represents something important that happened in the domain
- Example: UserCreated, UserDeactivated
- Used for decoupling and side effects

**Domain Exception**:
- Custom exceptions for business rule violations
- Example: UserNotFoundException, DuplicateEmailException

---

## Project Structure for Domain Layer

Your Domain project should have this folder structure:
```
InventoryManagement.Domain/
├── Entities/
│   └── (User entity will go here)
├── ValueObjects/
│   └── (Email value object will go here)
├── Exceptions/
│   └── (Custom exceptions will go here)
└── Events/
    └── (Domain events will go here)
```

Create these folders in the `InventoryManagement.Domain` project if they don't exist.

---

## Task 1: Create the User Entity

### Location
Create file: `InventoryManagement.Domain/Entities/User.cs`

### Instructions for Copilot

**Prompt to use with Copilot:**

> Create a User entity class in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Entities`
> 
> **Class Requirements**:
> - Make the class `sealed` and `public`
> - This is a DDD Entity with a unique identity
> - Should follow encapsulation principles (private setters)
> 
> **Properties**:
> - `Id` (Guid) - Unique identifier, private setter
> - `FirstName` (string) - Required, max 100 chars, private setter
> - `LastName` (string) - Required, max 100 chars, private setter
> - `Email` (Email value object type) - Required, private setter
> - `PasswordHash` (string) - Required for storing hashed password, private setter
> - `IsActive` (bool) - Indicates if user account is active, private setter
> - `CreatedAt` (DateTime) - UTC timestamp when created, private setter
> - `UpdatedAt` (DateTime) - UTC timestamp when last modified, private setter
> - `FullName` (string) - Read-only computed property that returns "FirstName LastName"
> 
> **Constructor**:
> - Create a private parameterless constructor (required for EF Core)
> 
> **Factory Method** - `Create`:
> - Public static method to create new User instances
> - Parameters: firstName, lastName, email (Email type), passwordHash
> - Should validate all required parameters (not null/empty/whitespace)
> - Throw ArgumentException with descriptive message if validation fails
> - Set Id to new Guid
> - Set IsActive to true by default
> - Set CreatedAt and UpdatedAt to DateTime.UtcNow
> - Return the created User instance
> 
> **Business Methods**:
> 
> 1. **UpdateInformation** method:
>    - Parameters: firstName, lastName, email
>    - Validates all parameters are not null/empty
>    - Updates FirstName, LastName, Email properties
>    - Updates UpdatedAt to current UTC time
>    - Throws ArgumentException if validation fails
> 
> 2. **ChangePassword** method:
>    - Parameter: newPasswordHash (string)
>    - Validates parameter is not null/empty
>    - Updates PasswordHash property
>    - Updates UpdatedAt to current UTC time
>    - Throws ArgumentException if validation fails
> 
> 3. **Activate** method:
>    - No parameters
>    - Sets IsActive to true
>    - Updates UpdatedAt to current UTC time
>    - Should be idempotent (if already active, just return)
> 
> 4. **Deactivate** method:
>    - No parameters
>    - Sets IsActive to false
>    - Updates UpdatedAt to current UTC time
>    - Should be idempotent (if already inactive, just return)
> 
> **SOLID Principles Applied**:
> - Single Responsibility: User class only manages user data and behavior
> - Encapsulation: Private setters, modifications only through methods
> - Factory Pattern: Create() method ensures valid object creation

### Expected Outcome
After Copilot generates the code, you should have a User entity with:
- ✅ All properties with private setters
- ✅ Factory method for creation
- ✅ Business methods for state changes
- ✅ Input validation
- ✅ No external dependencies

---

## Task 2: Create the Email Value Object

### Location
Create file: `InventoryManagement.Domain/ValueObjects/Email.cs`

### Instructions for Copilot

**Prompt to use with Copilot:**

> Create an Email value object class in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.ValueObjects`
> 
> **Class Requirements**:
> - Make the class `sealed`, `public`, and implement `IEquatable<Email>`
> - This is a DDD Value Object (immutable, defined by its value)
> - Should validate email format on creation
> 
> **Properties**:
> - `Value` (string) - The email address, public getter, private setter (or init)
> 
> **Regular Expression for Validation**:
> - Use regex pattern: `@"^[^@\s]+@[^@\s]+\.[^@\s]+$"`
> - Make it static, compiled, and case-insensitive for performance
> 
> **Constructor**:
> - Private constructor that takes the email value
> - Stores the value without validation (validation is done in factory)
> 
> **Factory Method** - `Create`:
> - Public static method to create Email instances
> - Parameter: email (string)
> - Should trim the input and convert to lowercase
> - Validates email is not null, empty, or whitespace
> - Validates email format using regex
> - Throw ArgumentException with descriptive message if validation fails
> - Return new Email instance
> 
> **Equality Methods** (Value Object Pattern):
> - Implement `Equals(Email? other)` - compares Value case-insensitively
> - Override `Equals(object? obj)` - calls Equals(Email? other)
> - Override `GetHashCode()` - uses Value.GetHashCode with case-insensitive comparison
> - Implement `==` operator
> - Implement `!=` operator
> 
> **Additional Methods**:
> - Override `ToString()` - returns Value
> - Implicit operator to convert Email to string for database storage
> 
> **Why Value Object?**
> - Type safety: Can't accidentally use wrong string as email
> - Always valid: Impossible to create invalid email
> - Immutable: Can't be changed after creation
> - Self-documenting: Code clearly shows "this is an email"

### Expected Outcome
After Copilot generates the code, you should have an Email value object with:
- ✅ Email format validation
- ✅ Immutability
- ✅ Value-based equality
- ✅ Implicit string conversion
- ✅ Type safety

---

## Task 3: Create Domain Exceptions

### Task 3.1: Base Domain Exception

**Location**: Create file `InventoryManagement.Domain/Exceptions/DomainException.cs`

**Prompt for Copilot:**

> Create an abstract base DomainException class in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Exceptions`
> 
> **Class Requirements**:
> - Abstract base class for all domain exceptions
> - Inherits from System.Exception
> 
> **Constructors**:
> - Protected constructor accepting message (string)
> - Protected constructor accepting message and innerException

### Task 3.2: UserNotFoundException

**Location**: Create file `InventoryManagement.Domain/Exceptions/UserNotFoundException.cs`

**Prompt for Copilot:**

> Create UserNotFoundException class in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Exceptions`
> 
> **Class Requirements**:
> - Sealed class inheriting from DomainException
> - Thrown when a user is not found in the system
> 
> **Properties**:
> - `UserId` (Guid?) - optional, public getter
> - `Email` (string?) - optional, public getter
> 
> **Constructors**:
> - Constructor accepting userId (Guid) - sets message "User with ID '{userId}' was not found."
> - Constructor accepting email (string) - sets message "User with email '{email}' was not found."

### Task 3.3: DuplicateEmailException

**Location**: Create file `InventoryManagement.Domain/Exceptions/DuplicateEmailException.cs`

**Prompt for Copilot:**

> Create DuplicateEmailException class in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Exceptions`
> 
> **Class Requirements**:
> - Sealed class inheriting from DomainException
> - Thrown when trying to create a user with an email that already exists
> 
> **Properties**:
> - `Email` (string) - public getter
> 
> **Constructor**:
> - Constructor accepting email (string) - sets message "A user with email '{email}' already exists."

### Task 3.4: InvalidCredentialsException

**Location**: Create file `InventoryManagement.Domain/Exceptions/InvalidCredentialsException.cs`

**Prompt for Copilot:**

> Create InvalidCredentialsException class in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Exceptions`
> 
> **Class Requirements**:
> - Sealed class inheriting from DomainException
> - Thrown when login credentials are invalid
> 
> **Constructor**:
> - Parameterless constructor - sets message "Invalid email or password."

### Expected Outcome
After creating all exceptions, you should have:
- ✅ Base DomainException class
- ✅ UserNotFoundException (by ID or email)
- ✅ DuplicateEmailException
- ✅ InvalidCredentialsException

---

## Task 4: Create Domain Events (Optional but Recommended)

Domain events represent important things that happened in the domain. They're useful for:
- Decoupling business logic
- Side effects (sending emails, logging, etc.)
- Audit trails
- Event sourcing (advanced)

### Task 4.1: Event Interface

**Location**: Create file `InventoryManagement.Domain/Events/IEvent.cs`

**Prompt for Copilot:**

> Create an IEvent marker interface in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Events`
> 
> **Interface Requirements**:
> - Public interface for domain events
> - Property: `OccurredOn` (DateTime) - when the event occurred

### Task 4.2: UserCreatedEvent

**Location**: Create file `InventoryManagement.Domain/Events/UserCreatedEvent.cs`

**Prompt for Copilot:**

> Create UserCreatedEvent class in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Events`
> 
> **Class Requirements**:
> - Sealed class implementing IEvent
> - Raised when a new user is created
> 
> **Properties**:
> - `UserId` (Guid) - public getter
> - `Email` (string) - public getter
> - `OccurredOn` (DateTime) - public getter
> 
> **Constructor**:
> - Accepts userId and email
> - Sets OccurredOn to DateTime.UtcNow

### Task 4.3: UserDeactivatedEvent

**Location**: Create file `InventoryManagement.Domain/Events/UserDeactivatedEvent.cs`

**Prompt for Copilot:**

> Create UserDeactivatedEvent class in C# following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Events`
> 
> **Class Requirements**:
> - Sealed class implementing IEvent
> - Raised when a user is deactivated
> 
> **Properties**:
> - `UserId` (Guid) - public getter
> - `Email` (string) - public getter
> - `OccurredOn` (DateTime) - public getter
> 
> **Constructor**:
> - Accepts userId and email
> - Sets OccurredOn to DateTime.UtcNow

### Expected Outcome
After creating events, you should have:
- ✅ IEvent interface
- ✅ UserCreatedEvent
- ✅ UserDeactivatedEvent

---

## Task 5: Create Unit Tests for Domain Layer

Unit tests verify that your domain logic works correctly without any external dependencies.

### Setup
The `InventoryManagement.Domain.Tests` project should already have:
- xUnit framework
- FluentAssertions (for better test syntax)

If not, install these packages via NuGet Package Manager.

### Task 5.1: User Entity Tests

**Location**: Create file `InventoryManagement.Domain.Tests/Entities/UserTests.cs`

**Prompt for Copilot:**

> Create unit tests for the User entity using xUnit and FluentAssertions. Create a test class named UserTests with the following test methods:
> 
> **Namespace**: `InventoryManagement.Domain.Tests.Entities`
> 
> **Test Methods to Create**:
> 
> 1. **Create_WithValidData_ShouldCreateUser**:
>    - Arrange: Valid firstName, lastName, email (use Email.Create), passwordHash
>    - Act: Call User.Create with valid data
>    - Assert: User is not null, all properties set correctly, IsActive is true, Id is not empty, FullName is "FirstName LastName", CreatedAt is close to now
> 
> 2. **Create_WithInvalidFirstName_ShouldThrowException** (Theory with InlineData):
>    - Test with: null, empty string, whitespace
>    - Act: Attempt to create User with invalid firstName
>    - Assert: Should throw ArgumentException with message containing "First name"
> 
> 3. **Create_WithInvalidLastName_ShouldThrowException** (Theory with InlineData):
>    - Test with: null, empty string, whitespace
>    - Act: Attempt to create User with invalid lastName
>    - Assert: Should throw ArgumentException with message containing "Last name"
> 
> 4. **Create_WithInvalidPasswordHash_ShouldThrowException** (Theory with InlineData):
>    - Test with: null, empty string, whitespace
>    - Act: Attempt to create User with invalid passwordHash
>    - Assert: Should throw ArgumentException
> 
> 5. **Deactivate_WhenUserIsActive_ShouldDeactivateUser**:
>    - Arrange: Create an active user
>    - Act: Call Deactivate()
>    - Assert: IsActive should be false
> 
> 6. **Activate_WhenUserIsInactive_ShouldActivateUser**:
>    - Arrange: Create user and deactivate it
>    - Act: Call Activate()
>    - Assert: IsActive should be true
> 
> 7. **UpdateInformation_WithValidData_ShouldUpdateUser**:
>    - Arrange: Create user, note original UpdatedAt
>    - Act: Call UpdateInformation with new firstName, lastName, email
>    - Assert: Properties updated correctly, UpdatedAt is after original, FullName updated
> 
> 8. **UpdateInformation_WithInvalidFirstName_ShouldThrowException**:
>    - Act: Call UpdateInformation with null firstName
>    - Assert: Should throw ArgumentException
> 
> 9. **ChangePassword_WithValidHash_ShouldUpdatePassword**:
>    - Arrange: Create user
>    - Act: Call ChangePassword with new hash
>    - Assert: PasswordHash updated, UpdatedAt changed
> 
> 10. **ChangePassword_WithInvalidHash_ShouldThrowException** (Theory):
>     - Test with: null, empty string, whitespace
>     - Assert: Should throw ArgumentException
> 
> Use FluentAssertions for assertions (Should().Be(), Should().NotBeNull(), Should().Throw<>(), etc.)

### Task 5.2: Email Value Object Tests

**Location**: Create file `InventoryManagement.Domain.Tests/ValueObjects/EmailTests.cs`

**Prompt for Copilot:**

> Create unit tests for the Email value object using xUnit and FluentAssertions. Create a test class named EmailTests with the following test methods:
> 
> **Namespace**: `InventoryManagement.Domain.Tests.ValueObjects`
> 
> **Test Methods to Create**:
> 
> 1. **Create_WithValidEmail_ShouldCreateEmail** (Theory with InlineData):
>    - Test with: "test@example.com", "user.name@example.co.uk", "user+tag@example.com"
>    - Act: Call Email.Create with valid email
>    - Assert: Email not null, Value equals input (lowercase)
> 
> 2. **Create_WithInvalidEmail_ShouldThrowException** (Theory with InlineData):
>    - Test with: empty string, whitespace, "invalid", "@example.com", "user@", "user @example.com"
>    - Act: Attempt to create Email with invalid format
>    - Assert: Should throw ArgumentException
> 
> 3. **Create_WithUppercaseEmail_ShouldConvertToLowercase**:
>    - Act: Create Email with "TEST@EXAMPLE.COM"
>    - Assert: Value should be "test@example.com"
> 
> 4. **Equals_WithSameValue_ShouldReturnTrue**:
>    - Arrange: Create two Email objects with same value (different case)
>    - Act & Assert: email1.Equals(email2) and email1 == email2 should be true
> 
> 5. **Equals_WithDifferentValue_ShouldReturnFalse**:
>    - Arrange: Create two Email objects with different values
>    - Act & Assert: email1.Equals(email2) and email1 != email2 should work correctly
> 
> 6. **ToString_ShouldReturnEmailValue**:
>    - Arrange: Create Email
>    - Act: Call ToString()
>    - Assert: Should return the email value string
> 
> 7. **GetHashCode_WithSameValue_ShouldReturnSameHashCode**:
>    - Arrange: Create two Email objects with same value
>    - Assert: GetHashCode should return same value for both
> 
> Use FluentAssertions for all assertions

### Expected Outcome
After running tests, you should have:
- ✅ All domain tests passing
- ✅ Code coverage for User entity
- ✅ Code coverage for Email value object
- ✅ Confidence in domain logic

### Running Tests in Visual Studio
1. Build the solution (Ctrl+Shift+B)
2. Open Test Explorer (Test → Test Explorer)
3. Click "Run All Tests"
4. All tests should be green ✅

---

## Verification Checklist

Before moving to Step 2B (Application Layer), verify:

- [ ] `InventoryManagement.Domain` project compiles without errors
- [ ] User entity created with all required properties and methods
- [ ] Email value object created with validation
- [ ] All domain exceptions created
- [ ] Domain events created (IEvent, UserCreatedEvent, UserDeactivatedEvent)
- [ ] All unit tests created and passing
- [ ] No external dependencies in Domain project (only System namespaces)
- [ ] Domain project has NO NuGet packages (pure C#)

---

## What You've Learned

### Domain-Driven Design Concepts:
✅ **Entities** - Objects with identity that can change state  
✅ **Value Objects** - Immutable objects defined by their values  
✅ **Domain Events** - Important occurrences in the domain  
✅ **Domain Exceptions** - Business rule violations  

### Design Patterns:
✅ **Factory Pattern** - Create() methods for object creation  
✅ **Encapsulation** - Private setters, public methods  
✅ **Value Object Pattern** - Immutable, value-based equality  

### SOLID Principles:
✅ **Single Responsibility** - Each class has one reason to change  
✅ **Open/Closed** - Open for extension, closed for modification  
✅ **Dependency Inversion** - Domain has no dependencies  

### Testing:
✅ **Unit Testing** - Testing without external dependencies  
✅ **Arrange-Act-Assert** pattern  
✅ **FluentAssertions** - Readable test assertions  

---

## Common Issues and Solutions

**Issue**: "Email type not recognized in User.Create"
- Make sure you've created the Email value object first
- Add `using InventoryManagement.Domain.ValueObjects;` to User.cs

**Issue**: "Tests not showing in Test Explorer"
- Rebuild the solution
- Make sure test methods are public and have [Fact] or [Theory] attribute

**Issue**: "xUnit or FluentAssertions not found"
- Install packages via NuGet Package Manager
- Right-click Domain.Tests project → Manage NuGet Packages → Browse → Search and install

**Issue**: "Email validation not working correctly"
- Check regex pattern is correct
- Ensure regex options include IgnoreCase
- Test with the provided test cases

---

## Next Steps

Once all domain tests are passing, you're ready for:

**Step 2B: Application Layer**
- CQRS Commands and Queries
- MediatR Handlers
- FluentValidation
- Application Interfaces
- DTOs
- Application layer unit tests

The Application layer will use the domain entities and value objects you just created to implement business use cases!

---

## Tips for Working with Copilot

1. **Be specific**: The more detailed your prompt, the better code Copilot generates
2. **One class at a time**: Don't try to generate everything at once
3. **Review the code**: Copilot is a tool, not a replacement for understanding
4. **Run tests frequently**: Verify each class works before moving to the next
5. **Ask Copilot to explain**: Use Copilot Chat to understand the generated code
6. **Iterate**: If code isn't right, refine your prompt and try again

---

## Summary

In this step, you've instructed Copilot to create:
- ✅ User entity with business methods
- ✅ Email value object with validation  
- ✅ Domain exceptions for error handling
- ✅ Domain events for important occurrences
- ✅ Comprehensive unit tests

Your Domain layer is now complete, framework-independent, and fully tested!
