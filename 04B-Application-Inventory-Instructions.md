# Step 4B: Application Layer - Inventory Module CQRS with MediatR
## Instructions for GitHub Copilot

---

## Overview
This guide provides instructions for building the **Application layer** for the Inventory module. We'll implement CQRS Commands and Queries for managing products across 5 categories: Aroma Bombel, Aroma Bottle, Aroma Device, Sanitizing Device, and Battery.

---

## Folder Structure

Create this folder structure in `InventoryManagement.Application`:

```
Application/
├── Features/
│   ├── Users/                    # (already exists)
│   └── Products/
│       ├── Commands/
│       │   ├── CreateProduct/
│       │   ├── UpdateProduct/
│       │   ├── DeleteProduct/
│       │   ├── AddStock/
│       │   └── RemoveStock/
│       └── Queries/
│           ├── GetAllProducts/
│           ├── GetProductById/
│           ├── GetProductsByType/
│           ├── GetLowStockProducts/
│           └── GetStockHistory/
├── Common/
│   ├── Interfaces/               # Add IApplicationDbContext updates
│   └── Models/                   # Add Product DTOs
```

---

## Task 1: Update IApplicationDbContext Interface

### Location: `InventoryManagement.Application/Common/Interfaces/IApplicationDbContext.cs`

**Prompt for Copilot:**

> Update IApplicationDbContext interface to add Product and StockHistory DbSets:
> 
> **Add these new DbSet properties**:
> - `DbSet<Product> Products { get; }` - for all products (base type)
> - `DbSet<StockHistory> StockHistories { get; }` - for stock history
> 
> **Add imports**:
> - using InventoryManagement.Domain.Entities;
> 
> Keep existing Users DbSet and SaveChangesAsync method

---

## Task 2: Create DTOs (Data Transfer Objects)

### Task 2.1: Base Product DTO

**Location**: `InventoryManagement.Application/Common/Models/ProductDto.cs`

**Prompt for Copilot:**

> Create ProductDto class following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Common.Models`
> 
> **Purpose**: Base DTO for returning product information
> 
> **Properties**:
> - Id (Guid)
> - Name (string)
> - Description (string?)
> - ProductType (string) - enum name as string
> - ProductTypeId (int) - enum value
> - Price (decimal)
> - Currency (string)
> - PhotoUrl (string?)
> - StockQuantity (int)
> - IsActive (bool)
> - IsLowStock (bool)
> - CreatedAt (DateTime)
> - UpdatedAt (DateTime)
> 
> All properties with public get/set and default empty strings where applicable
> 
> Add XML documentation

### Task 2.2: Aroma Bombel Product DTO

**Location**: `InventoryManagement.Application/Common/Models/AromaBombelProductDto.cs`

**Prompt for Copilot:**

> Create AromaBombelProductDto class following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Common.Models`
> 
> **Class**: Public sealed class inheriting from ProductDto
> 
> **Additional Properties**:
> - Taste (string?) - TasteType enum name or null
> - TasteId (int?) - TasteType enum value or null
> 
> Add XML documentation

### Task 2.3: Aroma Bottle Product DTO

**Location**: `InventoryManagement.Application/Common/Models/AromaBottleProductDto.cs`

**Prompt for Copilot:**

> Create AromaBottleProductDto class following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Common.Models`
> 
> **Class**: Public sealed class inheriting from ProductDto
> 
> **Additional Properties**:
> - Taste (string?)
> - TasteId (int?)
> 
> Add XML documentation

### Task 2.4: Aroma Device Product DTO

**Location**: `InventoryManagement.Application/Common/Models/AromaDeviceProductDto.cs`

**Prompt for Copilot:**

> Create AromaDeviceProductDto class following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Common.Models`
> 
> **Class**: Public sealed class inheriting from ProductDto
> 
> **Additional Properties**:
> - Color (string?)
> - ColorId (int?)
> - Format (string?)
> - Programs (string?)
> - PlugType (string) - required
> - PlugTypeId (int)
> - SquareMeter (decimal?)
> 
> Add XML documentation

### Task 2.5: Sanitizing Device Product DTO

**Location**: `InventoryManagement.Application/Common/Models/SanitizingDeviceProductDto.cs`

**Prompt for Copilot:**

> Create SanitizingDeviceProductDto class following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Common.Models`
> 
> **Class**: Public sealed class inheriting from ProductDto
> 
> **Additional Properties**:
> - Color (string?)
> - ColorId (int?)
> - Format (string?)
> - Programs (string?)
> - PlugType (string)
> - PlugTypeId (int)
> 
> Add XML documentation

### Task 2.6: Battery Product DTO

**Location**: `InventoryManagement.Application/Common/Models/BatteryProductDto.cs`

**Prompt for Copilot:**

> Create BatteryProductDto class following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Common.Models`
> 
> **Class**: Public sealed class inheriting from ProductDto
> 
> **Additional Properties**:
> - Type (string?) - battery type text
> - Size (string?) - BatterySize enum name
> - SizeId (int?)
> - Brand (string?)
> 
> Add XML documentation

### Task 2.7: Stock History DTO

**Location**: `InventoryManagement.Application/Common/Models/StockHistoryDto.cs`

**Prompt for Copilot:**

> Create StockHistoryDto class following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Common.Models`
> 
> **Properties**:
> - Id (Guid)
> - ProductId (Guid)
> - ProductName (string)
> - QuantityChanged (int)
> - QuantityAfter (int)
> - ChangeType (string) - "Added" or "Removed"
> - Reason (string?)
> - ChangedBy (Guid)
> - ChangedByName (string)
> - ChangedAt (DateTime)
> 
> Add XML documentation

---

## Task 3: Create Commands - Create Product

### Task 3.1: Create Aroma Bombel Product Command

**Folder**: Create `Application/Features/Products/Commands/CreateProduct/`

**Location**: `CreateAromaBombelCommand.cs`

**Prompt for Copilot:**

> Create CreateAromaBombelCommand record following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Features.Products.Commands.CreateProduct`
> 
> **Record**: Public sealed record implementing `IRequest<Guid>`
> 
> **Properties**:
> - Name (string)
> - Description (string?)
> - Price (decimal)
> - Currency (string) - default "ALL"
> - PhotoUrl (string?)
> - TasteId (int?) - TasteType enum value
> 
> Add XML documentation

**Location**: `CreateAromaBombelCommandValidator.cs`

**Prompt for Copilot:**

> Create CreateAromaBombelCommandValidator following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Features.Products.Commands.CreateProduct`
> 
> **Class**: Inherits AbstractValidator<CreateAromaBombelCommand>
> 
> **Validation Rules**:
> - Name: NotEmpty, MaxLength(200), MinLength(2)
> - Description: MaxLength(1000) when not null
> - Price: GreaterThan(0)
> - Currency: NotEmpty, Length(3) - currency codes are 3 chars
> - PhotoUrl: Must be valid URL format when not null/empty
> - TasteId: Must be valid TasteType enum value when provided (1-4)
> 
> Add XML documentation

**Location**: `CreateAromaBombelCommandHandler.cs`

**Prompt for Copilot:**

> Create CreateAromaBombelCommandHandler following these specifications:
> 
> **Namespace**: `InventoryManagement.Application.Features.Products.Commands.CreateProduct`
> 
> **Class**: Implements `IRequestHandler<CreateAromaBombelCommand, Guid>`
> 
> **Dependencies** (inject via constructor):
> - IApplicationDbContext context
> 
> **Handle method logic**:
> 1. Create ProductName value object from request.Name
> 2. Check if product name already exists in AromaBombel category:
>    - Query: context.Products.OfType<AromaBombelProduct>().AnyAsync(p => p.Name == productName)
>    - If exists, throw DuplicateProductNameException
> 3. Create Money value object from request.Price and request.Currency
> 4. Convert TasteId to TasteType? enum (null if not provided)
> 5. Create product using AromaBombelProduct.Create()
> 6. Add to context.Products
> 7. SaveChangesAsync
> 8. Return product.Id
> 
> Add XML documentation

### Task 3.2: Create Aroma Bottle Product Command

**Location**: `CreateAromaBottleCommand.cs`

**Prompt for Copilot:**

> Create CreateAromaBottleCommand record (similar to AromaBombel):
> 
> **Properties**:
> - Name (string)
> - Description (string?)
> - Price (decimal)
> - Currency (string)
> - PhotoUrl (string?)
> - TasteId (int?)
> 
> Implements IRequest<Guid>

**Location**: `CreateAromaBottleCommandValidator.cs`

**Prompt for Copilot:**

> Create validator for CreateAromaBottleCommand with same rules as AromaBombel

**Location**: `CreateAromaBottleCommandHandler.cs`

**Prompt for Copilot:**

> Create handler for CreateAromaBottleCommand:
> - Check name uniqueness within AromaBottle category
> - Create AromaBottleProduct
> - Similar logic to AromaBombel handler

### Task 3.3: Create Aroma Device Product Command

**Location**: `CreateAromaDeviceCommand.cs`

**Prompt for Copilot:**

> Create CreateAromaDeviceCommand record:
> 
> **Properties**:
> - Name (string)
> - Description (string?)
> - Price (decimal)
> - Currency (string)
> - PhotoUrl (string?)
> - ColorId (int?)
> - Format (string?)
> - Programs (string?)
> - PlugTypeId (int) - required
> - SquareMeter (decimal?)
> 
> Implements IRequest<Guid>

**Location**: `CreateAromaDeviceCommandValidator.cs`

**Prompt for Copilot:**

> Create validator for CreateAromaDeviceCommand:
> 
> **Validation Rules**:
> - Name: NotEmpty, MaxLength(200), MinLength(2)
> - Description: MaxLength(1000)
> - Price: GreaterThan(0)
> - Currency: NotEmpty, Length(3)
> - PhotoUrl: Valid URL when provided
> - ColorId: Valid ColorType enum (1-11) when provided
> - Format: MaxLength(200)
> - Programs: MaxLength(2000)
> - PlugTypeId: NotEmpty, Must be valid DevicePlugType enum (1-2)
> - SquareMeter: GreaterThanOrEqualTo(0) when provided

**Location**: `CreateAromaDeviceCommandHandler.cs`

**Prompt for Copilot:**

> Create handler for CreateAromaDeviceCommand:
> - Check name uniqueness within AromaDevice category
> - Convert enums from int to enum types
> - Validate PlugTypeId is provided and valid
> - Create AromaDeviceProduct
> - Save and return Id

### Task 3.4: Create Sanitizing Device Product Command

**Location**: `CreateSanitizingDeviceCommand.cs`

**Prompt for Copilot:**

> Create CreateSanitizingDeviceCommand record:
> 
> **Properties**:
> - Name (string)
> - Description (string?)
> - Price (decimal)
> - Currency (string)
> - PhotoUrl (string?)
> - ColorId (int?)
> - Format (string?)
> - Programs (string?)
> - PlugTypeId (int) - required
> 
> Implements IRequest<Guid>

**Location**: `CreateSanitizingDeviceCommandValidator.cs`

**Prompt for Copilot:**

> Create validator for CreateSanitizingDeviceCommand (similar to AromaDevice but without SquareMeter)

**Location**: `CreateSanitizingDeviceCommandHandler.cs`

**Prompt for Copilot:**

> Create handler for CreateSanitizingDeviceCommand:
> - Check name uniqueness within SanitizingDevice category
> - Create SanitizingDeviceProduct
> - Save and return Id

### Task 3.5: Create Battery Product Command

**Location**: `CreateBatteryCommand.cs`

**Prompt for Copilot:**

> Create CreateBatteryCommand record:
> 
> **Properties**:
> - Name (string)
> - Description (string?)
> - Price (decimal)
> - Currency (string)
> - PhotoUrl (string?)
> - Type (string?) - battery type text
> - SizeId (int?) - BatterySize enum value
> - Brand (string?)
> 
> Implements IRequest<Guid>

**Location**: `CreateBatteryCommandValidator.cs`

**Prompt for Copilot:**

> Create validator for CreateBatteryCommand:
> 
> **Validation Rules**:
> - Name: NotEmpty, MaxLength(200), MinLength(2)
> - Description: MaxLength(1000)
> - Price: GreaterThan(0)
> - Currency: NotEmpty, Length(3)
> - Type: MaxLength(100)
> - SizeId: Must be valid BatterySize enum (1-2) when provided
> - Brand: MaxLength(100)

**Location**: `CreateBatteryCommandHandler.cs`

**Prompt for Copilot:**

> Create handler for CreateBatteryCommand:
> - Check name uniqueness within Battery category
> - Create BatteryProduct
> - Save and return Id

---

## Task 4: Create Commands - Update Product

### Task 4.1: Update Aroma Bombel Product Command

**Folder**: Create `Application/Features/Products/Commands/UpdateProduct/`

**Location**: `UpdateAromaBombelCommand.cs`

**Prompt for Copilot:**

> Create UpdateAromaBombelCommand record:
> 
> **Properties**:
> - ProductId (Guid)
> - Name (string)
> - Description (string?)
> - Price (decimal)
> - Currency (string)
> - PhotoUrl (string?)
> - TasteId (int?)
> 
> Implements IRequest<Unit>

**Location**: `UpdateAromaBombelCommandValidator.cs`

**Prompt for Copilot:**

> Create validator with same rules as Create + ProductId NotEmpty

**Location**: `UpdateAromaBombelCommandHandler.cs`

**Prompt for Copilot:**

> Create UpdateAromaBombelCommandHandler:
> 
> **Handle logic**:
> 1. Find product by Id using OfType<AromaBombelProduct>().FirstOrDefaultAsync()
> 2. If not found, throw ProductNotFoundException
> 3. Create new ProductName from request.Name
> 4. Check if new name is already taken by another product in same category (exclude current product)
> 5. If duplicate, throw DuplicateProductNameException
> 6. Create Money from price
> 7. Call product.UpdateBasicInfo(name, description, price, photoUrl)
> 8. Convert TasteId to TasteType? and call product.UpdateSpecificInfo(taste)
> 9. SaveChangesAsync
> 10. Return Unit.Value

### Task 4.2-4.5: Update Commands for Other Product Types

**Prompt for Copilot:**

> Create similar Update commands, validators, and handlers for:
> - UpdateAromaBottleCommand (similar to AromaBombel)
> - UpdateAromaDeviceCommand (with Color, Format, Programs, PlugType, SquareMeter)
> - UpdateSanitizingDeviceCommand (with Color, Format, Programs, PlugType)
> - UpdateBatteryCommand (with Type, Size, Brand)
> 
> Each should:
> - Include ProductId
> - Validate same rules as Create
> - Check name uniqueness excluding current product
> - Update basic info and specific info

---

## Task 5: Create Commands - Delete Product

**Folder**: Create `Application/Features/Products/Commands/DeleteProduct/`

**Location**: `DeleteProductCommand.cs`

**Prompt for Copilot:**

> Create DeleteProductCommand record:
> 
> **Namespace**: `InventoryManagement.Application.Features.Products.Commands.DeleteProduct`
> 
> **Properties**:
> - ProductId (Guid)
> 
> Implements IRequest<Unit>
> 
> Note: This works for any product type

**Location**: `DeleteProductCommandHandler.cs`

**Prompt for Copilot:**

> Create DeleteProductCommandHandler:
> 
> **Handle logic**:
> 1. Find product by Id from context.Products
> 2. If not found, throw ProductNotFoundException
> 3. Instead of deleting, call product.Deactivate() (soft delete)
> 4. SaveChangesAsync
> 5. Return Unit.Value
> 
> Add XML comment explaining this is a soft delete that deactivates the product

---

## Task 6: Create Commands - Stock Management

### Task 6.1: Add Stock Command

**Folder**: Create `Application/Features/Products/Commands/AddStock/`

**Location**: `AddStockCommand.cs`

**Prompt for Copilot:**

> Create AddStockCommand record:
> 
> **Namespace**: `InventoryManagement.Application.Features.Products.Commands.AddStock`
> 
> **Properties**:
> - ProductId (Guid)
> - Quantity (int)
> - Reason (string?) - optional reason for adding stock
> 
> Implements IRequest<int> - returns new stock quantity

**Location**: `AddStockCommandValidator.cs`

**Prompt for Copilot:**

> Create validator:
> - ProductId: NotEmpty
> - Quantity: GreaterThan(0) with message "Quantity must be greater than 0"
> - Reason: MaxLength(500)

**Location**: `AddStockCommandHandler.cs`

**Prompt for Copilot:**

> Create AddStockCommandHandler:
> 
> **Dependencies**:
> - IApplicationDbContext context
> - ICurrentUserService currentUserService
> 
> **Handle logic**:
> 1. Find product by Id
> 2. If not found, throw ProductNotFoundException
> 3. Call product.AddStock(request.Quantity)
> 4. Create StockHistory.CreateAddition() with:
>    - productId
>    - quantityAdded: request.Quantity
>    - quantityAfter: product.StockQuantity
>    - reason: request.Reason
>    - changedBy: currentUserService.UserId.Value
> 5. Add StockHistory to context.StockHistories
> 6. SaveChangesAsync
> 7. Return product.StockQuantity

### Task 6.2: Remove Stock Command

**Folder**: Create `Application/Features/Products/Commands/RemoveStock/`

**Location**: `RemoveStockCommand.cs`

**Prompt for Copilot:**

> Create RemoveStockCommand record:
> 
> **Properties**:
> - ProductId (Guid)
> - Quantity (int)
> - Reason (string?)
> 
> Implements IRequest<int>

**Location**: `RemoveStockCommandValidator.cs`

**Prompt for Copilot:**

> Create validator:
> - ProductId: NotEmpty
> - Quantity: GreaterThan(0)
> - Reason: MaxLength(500)

**Location**: `RemoveStockCommandHandler.cs`

**Prompt for Copilot:**

> Create RemoveStockCommandHandler:
> 
> **Handle logic**:
> 1. Find product by Id
> 2. If not found, throw ProductNotFoundException
> 3. Check if product.StockQuantity >= request.Quantity
> 4. If insufficient, throw InsufficientStockException
> 5. Call product.RemoveStock(request.Quantity)
> 6. Create StockHistory.CreateRemoval()
> 7. Check if product.IsLowStock(threshold: 10) - if true, could raise event
> 8. Add StockHistory to context
> 9. SaveChangesAsync
> 10. Return product.StockQuantity

---

## Task 7: Create Queries

### Task 7.1: Get All Products Query

**Folder**: Create `Application/Features/Products/Queries/GetAllProducts/`

**Location**: `GetAllProductsQuery.cs`

**Prompt for Copilot:**

> Create GetAllProductsQuery record:
> 
> **Namespace**: `InventoryManagement.Application.Features.Products.Queries.GetAllProducts`
> 
> **Properties**:
> - IncludeInactive (bool) - default false, whether to include deactivated products
> 
> Implements IRequest<List<ProductDto>>

**Location**: `GetAllProductsQueryHandler.cs`

**Prompt for Copilot:**

> Create GetAllProductsQueryHandler:
> 
> **Handle logic**:
> 1. Query context.Products
> 2. If !request.IncludeInactive, filter where IsActive == true
> 3. Order by ProductType, then by Name
> 4. Project to appropriate DTO based on ProductType:
>    - For AromaBombelProduct → AromaBombelProductDto
>    - For AromaBottleProduct → AromaBottleProductDto
>    - For AromaDeviceProduct → AromaDeviceProductDto
>    - For SanitizingDeviceProduct → SanitizingDeviceProductDto
>    - For BatteryProduct → BatteryProductDto
> 5. Map all common properties from Product base
> 6. Map specific properties based on type
> 7. Return List<ProductDto>
> 
> Note: Use switch pattern matching or visitor pattern for type-specific mapping

### Task 7.2: Get Products By Type Query

**Folder**: Create `Application/Features/Products/Queries/GetProductsByType/`

**Location**: `GetProductsByTypeQuery.cs`

**Prompt for Copilot:**

> Create GetProductsByTypeQuery record:
> 
> **Properties**:
> - ProductTypeId (int) - ProductType enum value
> - IncludeInactive (bool) - default false
> 
> Implements IRequest<List<ProductDto>>

**Location**: `GetProductsByTypeQueryHandler.cs`

**Prompt for Copilot:**

> Create handler that:
> 1. Convert ProductTypeId to ProductType enum
> 2. Filter products by ProductType
> 3. Filter by IsActive if needed
> 4. Map to appropriate DTO based on type
> 5. Return list

### Task 7.3: Get Product By Id Query

**Folder**: Create `Application/Features/Products/Queries/GetProductById/`

**Location**: `GetProductByIdQuery.cs`

**Prompt for Copilot:**

> Create GetProductByIdQuery record:
> 
> **Properties**:
> - ProductId (Guid)
> 
> Implements IRequest<ProductDto>

**Location**: `GetProductByIdQueryHandler.cs`

**Prompt for Copilot:**

> Create handler that:
> 1. Find product by Id
> 2. If not found, throw ProductNotFoundException
> 3. Determine actual type (AromaBombel, AromaDevice, etc.)
> 4. Map to appropriate specific DTO
> 5. Return DTO

### Task 7.4: Get Low Stock Products Query

**Folder**: Create `Application/Features/Products/Queries/GetLowStockProducts/`

**Location**: `GetLowStockProductsQuery.cs`

**Prompt for Copilot:**

> Create GetLowStockProductsQuery record:
> 
> **Properties**:
> - Threshold (int) - default 10
> 
> Implements IRequest<List<ProductDto>>

**Location**: `GetLowStockProductsQueryHandler.cs`

**Prompt for Copilot:**

> Create handler that:
> 1. Query active products where StockQuantity <= threshold
> 2. Order by StockQuantity ascending (lowest first)
> 3. Map to appropriate DTOs
> 4. Return list

### Task 7.5: Get Stock History Query

**Folder**: Create `Application/Features/Products/Queries/GetStockHistory/`

**Location**: `GetStockHistoryQuery.cs`

**Prompt for Copilot:**

> Create GetStockHistoryQuery record:
> 
> **Properties**:
> - ProductId (Guid?) - optional, if null returns all history
> - FromDate (DateTime?) - optional filter
> - ToDate (DateTime?) - optional filter
> - Take (int) - default 50, max records to return
> 
> Implements IRequest<List<StockHistoryDto>>

**Location**: `GetStockHistoryQueryHandler.cs`

**Prompt for Copilot:**

> Create handler that:
> 1. Query StockHistories
> 2. Filter by ProductId if provided
> 3. Filter by date range if provided
> 4. Order by ChangedAt descending (newest first)
> 5. Take specified number of records
> 6. Join with Products to get ProductName
> 7. Join with Users to get ChangedByName
> 8. Map to StockHistoryDto
> 9. Return list

---

## Task 8: Create Mapping Helper

**Location**: `Application/Features/Products/ProductMapper.cs`

**Prompt for Copilot:**

> Create ProductMapper static class to centralize product-to-DTO mapping:
> 
> **Namespace**: `InventoryManagement.Application.Features.Products`
> 
> **Methods**:
> 
> 1. **MapToDto(Product product)** → ProductDto:
>    - Use pattern matching (switch expression) to determine product type
>    - Call appropriate specific mapper
>    - Return base ProductDto if unknown type
> 
> 2. **MapBaseProperties(Product product, ProductDto dto)**:
>    - Map all common properties: Id, Name.Value, Description, ProductType, Price, etc.
>    - Calculate IsLowStock
> 
> 3. **MapAromaBombel(AromaBombelProduct product)** → AromaBombelProductDto:
>    - Map base properties
>    - Map Taste and TasteId
> 
> 4. **MapAromaBottle(AromaBottleProduct product)** → AromaBottleProductDto
> 
> 5. **MapAromaDevice(AromaDeviceProduct product)** → AromaDeviceProductDto:
>    - Map base properties
>    - Map Color, ColorId, Format, Programs, PlugType, PlugTypeId, SquareMeter
> 
> 6. **MapSanitizingDevice(SanitizingDeviceProduct product)** → SanitizingDeviceProductDto
> 
> 7. **MapBattery(BatteryProduct product)** → BatteryProductDto
> 
> Add XML documentation

---

## Task 9: Unit Tests for Application Layer

### Location: `Application.Tests/Features/Products/Commands/CreateAromaBombelCommandHandlerTests.cs`

**Prompt for Copilot:**

> Create unit tests for CreateAromaBombelCommandHandler:
> 
> 1. **Handle_WithValidCommand_ShouldCreateProduct**
> 2. **Handle_WithDuplicateName_ShouldThrowDuplicateProductNameException**
> 3. **Handle_WithInvalidPrice_ShouldThrowException** (via validator)
> 4. **Handle_WithValidTaste_ShouldSetTasteProperty**

### Location: `Application.Tests/Features/Products/Commands/AddStockCommandHandlerTests.cs`

**Prompt for Copilot:**

> Create unit tests for AddStockCommandHandler:
> 
> 1. **Handle_WithValidQuantity_ShouldIncreaseStock**
> 2. **Handle_WithInvalidProductId_ShouldThrowProductNotFoundException**
> 3. **Handle_ShouldCreateStockHistoryRecord**

### Location: `Application.Tests/Features/Products/Commands/RemoveStockCommandHandlerTests.cs`

**Prompt for Copilot:**

> Create unit tests for RemoveStockCommandHandler:
> 
> 1. **Handle_WithValidQuantity_ShouldDecreaseStock**
> 2. **Handle_WithInsufficientStock_ShouldThrowInsufficientStockException**
> 3. **Handle_ShouldCreateStockHistoryRecord**

---

## Verification Checklist

Before moving to Step 4C, verify:

- [ ] IApplicationDbContext updated with Products and StockHistories
- [ ] All DTOs created (ProductDto base + 5 specific + StockHistoryDto)
- [ ] All Create commands created (5 product types)
- [ ] All Create validators created
- [ ] All Create handlers created
- [ ] All Update commands created (5 product types)
- [ ] All Update validators and handlers created
- [ ] Delete command created (soft delete)
- [ ] AddStock command, validator, handler created
- [ ] RemoveStock command, validator, handler created
- [ ] All queries created (GetAll, GetByType, GetById, GetLowStock, GetStockHistory)
- [ ] ProductMapper helper created
- [ ] Unit tests created
- [ ] Solution builds without errors
- [ ] All tests pass

---

## What You've Accomplished

✅ **DTOs** for all product types  
✅ **Create Commands** for 5 product categories  
✅ **Update Commands** for all products  
✅ **Delete Command** (soft delete)  
✅ **Stock Management** commands with history tracking  
✅ **Queries** for retrieving products and stock history  
✅ **Product Mapper** for DTO conversion  
✅ **Validators** with business rules  
✅ **Unit Tests** for handlers  

---

## Key Patterns Used

### 1. Type-Specific Commands
Each product type has its own Create/Update command to handle specific properties.

### 2. Shared Delete Command
Delete works on any product type via base Product class.

### 3. Stock Management
- AddStock and RemoveStock track history
- Current user recorded for audit
- InsufficientStock validation

### 4. Product Mapper
Centralizes DTO mapping logic using pattern matching.

### 5. Soft Delete
Products are deactivated, not deleted, preserving data integrity.

---

## Next Steps

Ready for **Step 4C: Infrastructure & API Layer** where you'll create:
- EF Core configurations for Product hierarchy (TPH)
- StockHistory configuration
- Database migration
- Seed initial categories (optional)
- API Controllers for Products
- API endpoints for stock management
- Integration tests

---

## Tips

1. **Start with one product type** - Get AromaBombel working end-to-end, then copy for others
2. **Reuse patterns** - Copy-paste and modify for similar product types
3. **Test validators** - Use Theory with InlineData for validation tests
4. **Use pattern matching** - For type-specific logic in handlers and mapper
5. **Keep DTOs simple** - Only include what the frontend needs

Good luck! 🚀
