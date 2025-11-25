# Step 4A: Domain Layer - Inventory Module (Aroma Business)
## Instructions for GitHub Copilot

---

## Overview
This guide provides instructions for building the **Domain layer** for the Inventory module of an aroma business. This module handles products across 5 different categories, each with specific attributes.

**Business Context**: Aroma business with products in categories: Aroma Bombel, Aroma Bottle, Aroma Device, Sanitizing Device, and Battery.

---

## Business Rules Summary

### Categories:
1. **Aroma Bombel** - Aroma bombs with taste options
2. **Aroma Bottle** - Aroma bottles with taste options
3. **Aroma Device** - Aroma devices with color, format, programs
4. **Sanitizing Device** - Sanitizing devices with color, format, programs
5. **Battery** - Batteries with size and brand options

### Key Business Rules:
- Product names must be **unique within their category** (not globally unique)
- All products must have a **price** (required)
- Categories are **fixed** (cannot be added/deleted by users)
- Photos are optional but stored as URLs
- Different product types have different required/optional attributes

---

## Architecture Decision: Product Type Hierarchy

We'll use **Table Per Hierarchy (TPH)** with a discriminator column. This means:
- One `Products` table
- `ProductType` discriminator column
- Common attributes in base entity
- Type-specific attributes stored in same table (nullable for other types)

### Why This Approach?
✅ Simpler queries (one table)  
✅ Easier to add common features (stock, price history)  
✅ Better performance (no joins)  
✅ Flexible for future expansion  

---

## Task 1: Create Enums

### Location: `InventoryManagement.Domain/Enums/ProductType.cs`

**Prompt for Copilot:**

> Create ProductType enum following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Enums`
> 
> **Enum values**:
> - AromaBombel = 1
> - AromaBottle = 2
> - AromaDevice = 3
> - SanitizingDevice = 4
> - Battery = 5
> 
> Add XML documentation explaining these are the fixed product categories for the aroma business

### Location: `InventoryManagement.Domain/Enums/TasteType.cs`

**Prompt for Copilot:**

> Create TasteType enum following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Enums`
> 
> **Enum values**:
> - Flower = 1
> - Sweet = 2
> - Fresh = 3
> - Fruit = 4
> 
> Add XML documentation explaining this is used for Aroma Bombel and Aroma Bottle products

### Location: `InventoryManagement.Domain/Enums/ColorType.cs`

**Prompt for Copilot:**

> Create ColorType enum for basic colors following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Enums`
> 
> **Enum values**:
> - Red = 1
> - Blue = 2
> - Green = 3
> - Yellow = 4
> - Orange = 5
> - Purple = 6
> - Pink = 7
> - Brown = 8
> - Black = 9
> - White = 10
> - Gray = 11
> 
> Add XML documentation explaining this is used for Aroma Device and Sanitizing Device products

### Location: `InventoryManagement.Domain/Enums/DevicePlugType.cs`

**Prompt for Copilot:**

> Create DevicePlugType enum following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Enums`
> 
> **Enum values**:
> - WithPlug = 1
> - WithoutPlug = 2
> 
> Add XML documentation explaining this is for Aroma Device and Sanitizing Device products

### Location: `InventoryManagement.Domain/Enums/BatterySize.cs`

**Prompt for Copilot:**

> Create BatterySize enum following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Enums`
> 
> **Enum values**:
> - LR6 = 1
> - LR9 = 2
> 
> Add XML documentation explaining these are battery size options

---

## Task 2: Create Value Objects

### Location: `InventoryManagement.Domain/ValueObjects/Money.cs`

**Prompt for Copilot:**

> Create Money value object following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.ValueObjects`
> 
> **Class Requirements**:
> - Sealed class implementing IEquatable<Money>
> - Immutable value object for representing monetary amounts
> 
> **Properties**:
> - Amount (decimal) - the monetary value, must be >= 0
> - Currency (string) - currency code, default "ALL" (Albanian Lek)
> 
> **Constructor** (private):
> - Takes amount and currency
> 
> **Factory Method - Create**:
> - Parameters: amount (decimal), currency (string, default "ALL")
> - Validates amount >= 0, throw ArgumentException if negative
> - Validates currency is not null/empty
> - Returns new Money instance
> 
> **Equality Methods**:
> - Implement IEquatable<Money>
> - Two Money objects equal if same Amount and Currency
> - Override Equals, GetHashCode
> - Implement == and != operators
> 
> **Additional Methods**:
> - ToString() - returns formatted string like "100.00 ALL"
> - Add(Money other) - adds two Money objects (must be same currency)
> - Subtract(Money other) - subtracts Money (must be same currency)
> 
> Add XML documentation explaining this is a value object for monetary amounts

### Location: `InventoryManagement.Domain/ValueObjects/ProductName.cs`

**Prompt for Copilot:**

> Create ProductName value object following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.ValueObjects`
> 
> **Class Requirements**:
> - Sealed class implementing IEquatable<ProductName>
> - Immutable value object for product names
> 
> **Properties**:
> - Value (string) - the product name
> 
> **Constructor** (private):
> - Takes value string
> 
> **Factory Method - Create**:
> - Parameter: name (string)
> - Validates not null/empty/whitespace
> - Trims whitespace
> - Validates length: minimum 2, maximum 200 characters
> - Returns new ProductName instance
> - Throws ArgumentException if validation fails
> 
> **Equality Methods**:
> - Case-insensitive comparison
> - Implement IEquatable<ProductName>
> - Override Equals, GetHashCode
> - Implement == and != operators
> 
> **Additional Methods**:
> - ToString() - returns Value
> - Implicit conversion to string
> 
> Add XML documentation

---

## Task 3: Create Product Entity (Base Class)

### Location: `InventoryManagement.Domain/Entities/Product.cs`

**Prompt for Copilot:**

> Create Product entity as abstract base class following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Entities`
> 
> **Class Requirements**:
> - Public abstract class
> - Base class for all product types
> 
> **Common Properties** (all products have these):
> - Id (Guid) - unique identifier, private setter
> - Name (ProductName) - product name, private setter
> - Description (string?) - optional description, private setter
> - ProductType (ProductType enum) - category, private setter
> - Price (Money) - product price, private setter
> - PhotoUrl (string?) - optional photo URL, private setter
> - StockQuantity (int) - current stock, private setter, default 0
> - IsActive (bool) - whether product is active, private setter, default true
> - CreatedAt (DateTime) - UTC timestamp, private setter
> - UpdatedAt (DateTime) - UTC timestamp, private setter
> 
> **Protected Constructor**:
> - Parameters: name, description, productType, price, photoUrl
> - Sets Id = new Guid
> - Validates all required parameters
> - Sets CreatedAt and UpdatedAt to DateTime.UtcNow
> - Sets StockQuantity = 0, IsActive = true
> 
> **Business Methods**:
> 
> 1. **UpdateBasicInfo**:
>    - Parameters: name, description, price, photoUrl
>    - Updates these properties
>    - Updates UpdatedAt
>    - Validates required fields
> 
> 2. **AddStock**:
>    - Parameter: quantity (int)
>    - Validates quantity > 0
>    - Adds to StockQuantity
>    - Updates UpdatedAt
>    - Returns new stock quantity
> 
> 3. **RemoveStock**:
>    - Parameter: quantity (int)
>    - Validates quantity > 0
>    - Validates StockQuantity >= quantity (cannot go negative)
>    - Subtracts from StockQuantity
>    - Updates UpdatedAt
>    - Returns new stock quantity
>    - Throws InvalidOperationException if insufficient stock
> 
> 4. **Activate**:
>    - Sets IsActive = true
>    - Updates UpdatedAt
> 
> 5. **Deactivate**:
>    - Sets IsActive = false
>    - Updates UpdatedAt
> 
> 5. **IsLowStock**:
>    - Parameter: threshold (int, default 10)
>    - Returns true if StockQuantity <= threshold
> 
> Add XML documentation explaining this is the base class for all products

---

## Task 4: Create Specific Product Entities

### Location: `InventoryManagement.Domain/Entities/AromaBombelProduct.cs`

**Prompt for Copilot:**

> Create AromaBombelProduct entity following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Entities`
> 
> **Class Requirements**:
> - Public sealed class
> - Inherits from Product
> 
> **Additional Properties** (specific to Aroma Bombel):
> - Taste (TasteType?) - optional taste type, private setter
> 
> **Private Constructor**:
> - Calls base constructor with ProductType.AromaBombel
> 
> **Factory Method - Create**:
> - Parameters: name, description, price, photoUrl, taste (nullable)
> - Calls base constructor
> - Sets Taste property
> - Returns new AromaBombelProduct instance
> 
> **Update Method - UpdateSpecificInfo**:
> - Parameter: taste (nullable)
> - Updates Taste property
> - Calls base.UpdatedAt
> 
> Add XML documentation

### Location: `InventoryManagement.Domain/Entities/AromaBottleProduct.cs`

**Prompt for Copilot:**

> Create AromaBottleProduct entity following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Entities`
> 
> **Class Requirements**:
> - Public sealed class
> - Inherits from Product
> 
> **Additional Properties**:
> - Taste (TasteType?) - optional, private setter
> 
> **Private Constructor**:
> - Calls base with ProductType.AromaBottle
> 
> **Factory Method - Create**:
> - Parameters: name, description, price, photoUrl, taste
> - Returns new AromaBottleProduct
> 
> **Update Method - UpdateSpecificInfo**:
> - Parameter: taste
> - Updates Taste
> 
> Add XML documentation

### Location: `InventoryManagement.Domain/Entities/AromaDeviceProduct.cs`

**Prompt for Copilot:**

> Create AromaDeviceProduct entity following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Entities`
> 
> **Class Requirements**:
> - Public sealed class
> - Inherits from Product
> 
> **Additional Properties**:
> - Color (ColorType?) - optional, private setter
> - Format (string?) - optional text, private setter
> - Programs (string?) - optional textarea content, private setter
> - PlugType (DevicePlugType) - required, private setter
> - SquareMeter (decimal?) - optional non-negative number, private setter
> 
> **Private Constructor**:
> - Calls base with ProductType.AromaDevice
> 
> **Factory Method - Create**:
> - Parameters: name, description, price, photoUrl, color, format, programs, plugType, squareMeter
> - Validates plugType is required
> - Validates squareMeter >= 0 if provided
> - Returns new AromaDeviceProduct
> 
> **Update Method - UpdateSpecificInfo**:
> - Parameters: color, format, programs, plugType, squareMeter
> - Validates squareMeter >= 0 if provided
> - Updates all properties
> 
> Add XML documentation

### Location: `InventoryManagement.Domain/Entities/SanitizingDeviceProduct.cs`

**Prompt for Copilot:**

> Create SanitizingDeviceProduct entity following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Entities`
> 
> **Class Requirements**:
> - Public sealed class
> - Inherits from Product
> 
> **Additional Properties**:
> - Color (ColorType?) - optional, private setter
> - Format (string?) - optional, private setter
> - Programs (string?) - optional, private setter
> - PlugType (DevicePlugType) - required, private setter
> 
> **Private Constructor**:
> - Calls base with ProductType.SanitizingDevice
> 
> **Factory Method - Create**:
> - Parameters: name, description, price, photoUrl, color, format, programs, plugType
> - Validates plugType is required
> - Returns new SanitizingDeviceProduct
> 
> **Update Method - UpdateSpecificInfo**:
> - Parameters: color, format, programs, plugType
> - Updates all properties
> 
> Add XML documentation

### Location: `InventoryManagement.Domain/Entities/BatteryProduct.cs`

**Prompt for Copilot:**

> Create BatteryProduct entity following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Entities`
> 
> **Class Requirements**:
> - Public sealed class
> - Inherits from Product
> 
> **Additional Properties**:
> - Type (string?) - optional text, private setter
> - Size (BatterySize?) - optional size, private setter
> - Brand (string?) - optional brand name, private setter
> 
> **Private Constructor**:
> - Calls base with ProductType.Battery
> 
> **Factory Method - Create**:
> - Parameters: name, description, price, photoUrl, type, size, brand
> - Returns new BatteryProduct
> 
> **Update Method - UpdateSpecificInfo**:
> - Parameters: type, size, brand
> - Updates all properties
> 
> Add XML documentation

---

## Task 5: Create Stock History Entity

### Location: `InventoryManagement.Domain/Entities/StockHistory.cs`

**Prompt for Copilot:**

> Create StockHistory entity for tracking stock changes following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Entities`
> 
> **Purpose**: Audit trail for all stock movements
> 
> **Properties**:
> - Id (Guid) - unique identifier, private setter
> - ProductId (Guid) - related product, private setter
> - QuantityChanged (int) - amount added or removed (negative for removal), private setter
> - QuantityAfter (int) - stock quantity after this change, private setter
> - ChangeType (string) - "Added" or "Removed", private setter
> - Reason (string?) - optional reason for change, private setter
> - ChangedBy (Guid) - user who made the change, private setter
> - ChangedAt (DateTime) - UTC timestamp, private setter
> 
> **Private Constructor**:
> - No parameters
> 
> **Factory Method - CreateAddition**:
> - Parameters: productId, quantityAdded, quantityAfter, reason, changedBy
> - Sets ChangeType = "Added"
> - Sets QuantityChanged = quantityAdded (positive)
> - Sets all other properties
> - Returns new StockHistory
> 
> **Factory Method - CreateRemoval**:
> - Parameters: productId, quantityRemoved, quantityAfter, reason, changedBy
> - Sets ChangeType = "Removed"
> - Sets QuantityChanged = -quantityRemoved (negative)
> - Sets all other properties
> - Returns new StockHistory
> 
> Add XML documentation explaining this tracks all stock movements

---

## Task 6: Create Domain Exceptions

### Location: `InventoryManagement.Domain/Exceptions/ProductNotFoundException.cs`

**Prompt for Copilot:**

> Create ProductNotFoundException following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Exceptions`
> 
> **Class**: Sealed class inheriting from DomainException
> 
> **Properties**:
> - ProductId (Guid?) - optional
> - ProductName (string?) - optional
> 
> **Constructors**:
> - Constructor with productId (Guid) - message: "Product with ID '{productId}' was not found."
> - Constructor with productName (string) - message: "Product with name '{productName}' was not found."
> 
> Add XML documentation

### Location: `InventoryManagement.Domain/Exceptions/DuplicateProductNameException.cs`

**Prompt for Copilot:**

> Create DuplicateProductNameException following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Exceptions`
> 
> **Class**: Sealed class inheriting from DomainException
> 
> **Properties**:
> - ProductName (string)
> - ProductType (ProductType)
> 
> **Constructor**:
> - Parameters: productName, productType
> - Message: "A product with name '{productName}' already exists in category '{productType}'."
> 
> Add XML documentation explaining names must be unique within category

### Location: `InventoryManagement.Domain/Exceptions/InsufficientStockException.cs`

**Prompt for Copilot:**

> Create InsufficientStockException following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Exceptions`
> 
> **Class**: Sealed class inheriting from DomainException
> 
> **Properties**:
> - ProductId (Guid)
> - RequestedQuantity (int)
> - AvailableQuantity (int)
> 
> **Constructor**:
> - Parameters: productId, requestedQuantity, availableQuantity
> - Message: "Insufficient stock for product '{productId}'. Requested: {requestedQuantity}, Available: {availableQuantity}."
> 
> Add XML documentation

---

## Task 7: Create Domain Events

### Location: `InventoryManagement.Domain/Events/ProductCreatedEvent.cs`

**Prompt for Copilot:**

> Create ProductCreatedEvent following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Events`
> 
> **Class**: Sealed class implementing IEvent
> 
> **Properties**:
> - ProductId (Guid)
> - ProductName (string)
> - ProductType (ProductType)
> - OccurredOn (DateTime)
> 
> **Constructor**:
> - Parameters: productId, productName, productType
> - Sets OccurredOn to DateTime.UtcNow
> 
> Add XML documentation

### Location: `InventoryManagement.Domain/Events/StockAddedEvent.cs`

**Prompt for Copilot:**

> Create StockAddedEvent following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Events`
> 
> **Class**: Sealed class implementing IEvent
> 
> **Properties**:
> - ProductId (Guid)
> - QuantityAdded (int)
> - NewStockLevel (int)
> - OccurredOn (DateTime)
> 
> **Constructor**:
> - Parameters: productId, quantityAdded, newStockLevel
> - Sets OccurredOn to DateTime.UtcNow
> 
> Add XML documentation

### Location: `InventoryManagement.Domain/Events/StockRemovedEvent.cs`

**Prompt for Copilot:**

> Create StockRemovedEvent following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Events`
> 
> **Class**: Sealed class implementing IEvent
> 
> **Properties**:
> - ProductId (Guid)
> - QuantityRemoved (int)
> - NewStockLevel (int)
> - OccurredOn (DateTime)
> 
> **Constructor**:
> - Parameters: productId, quantityRemoved, newStockLevel
> - Sets OccurredOn to DateTime.UtcNow
> 
> Add XML documentation

### Location: `InventoryManagement.Domain/Events/LowStockAlertEvent.cs`

**Prompt for Copilot:**

> Create LowStockAlertEvent following these specifications:
> 
> **Namespace**: `InventoryManagement.Domain.Events`
> 
> **Class**: Sealed class implementing IEvent
> 
> **Properties**:
> - ProductId (Guid)
> - ProductName (string)
> - CurrentStock (int)
> - Threshold (int)
> - OccurredOn (DateTime)
> 
> **Constructor**:
> - Parameters: productId, productName, currentStock, threshold
> - Sets OccurredOn to DateTime.UtcNow
> 
> Add XML documentation explaining this is raised when stock falls below threshold

---

## Task 8: Unit Tests for Domain Layer

### Location: `InventoryManagement.Domain.Tests/ValueObjects/MoneyTests.cs`

**Prompt for Copilot:**

> Create unit tests for Money value object using xUnit and FluentAssertions with these test methods:
> 
> 1. **Create_WithValidAmount_ShouldCreateMoney**
> 2. **Create_WithNegativeAmount_ShouldThrowException**
> 3. **Add_SameCurrency_ShouldAddAmounts**
> 4. **Add_DifferentCurrency_ShouldThrowException**
> 5. **Subtract_ValidAmount_ShouldSubtractAmounts**
> 6. **Equals_SameAmountAndCurrency_ShouldReturnTrue**
> 7. **ToString_ShouldReturnFormattedString**

### Location: `InventoryManagement.Domain.Tests/Entities/ProductTests.cs`

**Prompt for Copilot:**

> Create unit tests for Product entities using xUnit and FluentAssertions with these test methods:
> 
> **For AromaBombelProduct**:
> 1. **Create_WithValidData_ShouldCreateProduct**
> 2. **Create_WithoutName_ShouldThrowException**
> 3. **Create_WithoutPrice_ShouldThrowException**
> 4. **AddStock_ValidQuantity_ShouldIncreaseStock**
> 5. **RemoveStock_ValidQuantity_ShouldDecreaseStock**
> 6. **RemoveStock_InsufficientStock_ShouldThrowException**
> 7. **IsLowStock_BelowThreshold_ShouldReturnTrue**
> 8. **UpdateSpecificInfo_ShouldUpdateTaste**
> 
> **For AromaDeviceProduct**:
> 1. **Create_WithValidData_ShouldCreateProduct**
> 2. **Create_WithoutPlugType_ShouldThrowException**
> 3. **Create_WithNegativeSquareMeter_ShouldThrowException**
> 4. **UpdateSpecificInfo_ShouldUpdateAllFields**
> 
> **For BatteryProduct**:
> 1. **Create_WithValidData_ShouldCreateProduct**
> 2. **UpdateSpecificInfo_ShouldUpdateFields**

### Location: `InventoryManagement.Domain.Tests/Entities/StockHistoryTests.cs`

**Prompt for Copilot:**

> Create unit tests for StockHistory entity:
> 
> 1. **CreateAddition_ShouldCreateWithPositiveQuantity**
> 2. **CreateRemoval_ShouldCreateWithNegativeQuantity**
> 3. **CreateAddition_ShouldSetChangeTypeToAdded**
> 4. **CreateRemoval_ShouldSetChangeTypeToRemoved**

---

## Verification Checklist

Before moving to Step 4B, verify:

- [ ] All enums created (ProductType, TasteType, ColorType, DevicePlugType, BatterySize)
- [ ] Money value object created with operations
- [ ] ProductName value object created
- [ ] Product base class created with common properties and methods
- [ ] All 5 specific product entities created (AromaBombel, AromaBottle, AromaDevice, SanitizingDevice, Battery)
- [ ] StockHistory entity created
- [ ] Domain exceptions created (ProductNotFound, DuplicateProductName, InsufficientStock)
- [ ] Domain events created (ProductCreated, StockAdded, StockRemoved, LowStockAlert)
- [ ] Unit tests created and passing
- [ ] Solution builds without errors

---

## What You've Accomplished

✅ **Domain model** for multi-category product system  
✅ **Value objects** for Money and ProductName  
✅ **Product hierarchy** with base class and 5 specific types  
✅ **Stock management** logic in domain  
✅ **Audit trail** with StockHistory  
✅ **Business rules** enforced in domain  
✅ **Domain events** for important actions  
✅ **Comprehensive tests** for domain logic  

---

## Key Design Decisions

### 1. Table Per Hierarchy (TPH)
- All products in one table
- Discriminator column for ProductType
- Type-specific columns nullable
- Better performance, simpler queries

### 2. Product Name Uniqueness
- Names must be unique **within** category
- Not globally unique
- Enforced in application layer

### 3. Stock Management
- Stock tracked in Product entity
- History tracked in separate StockHistory entity
- Cannot go negative (validation)
- Events raised for stock changes

### 4. Money Value Object
- Encapsulates amount and currency
- Prevents negative amounts
- Supports arithmetic operations
- Type-safe monetary calculations

---

## Next Steps

Ready for **Step 4B: Application Layer** where you'll create:
- CQRS Commands for product operations
- CQRS Queries for retrieving products
- DTOs for each product type
- Validators
- MediatR handlers
- Stock management use cases

---

## Tips

1. **Test each entity** as you create it
2. **Run tests frequently** to catch errors early
3. **Use Copilot** for repetitive code (5 product types)
4. **Follow the pattern** from User Management
5. **Build incrementally** - one product type at a time

Good luck! 🚀
