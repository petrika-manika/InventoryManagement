# Step 4C: Infrastructure & API Layer - Inventory Module Implementation
## Instructions for GitHub Copilot

---

## Overview
This guide provides instructions for building the **Infrastructure** and **API/Presentation** layers for the Inventory module. You'll implement Entity Framework Core configurations for the product hierarchy, database migrations, and REST API controllers.

---

## Task 1: Create Entity Framework Core Configurations

### Task 1.1: Product Configuration (Base)

**Location**: `InventoryManagement.Infrastructure/Data/Configurations/ProductConfiguration.cs`

**Prompt for Copilot:**

> Create ProductConfiguration class for EF Core following these specifications:
> 
> **Namespace**: `InventoryManagement.Infrastructure.Data.Configurations`
> 
> **Class**: Implements `IEntityTypeConfiguration<Product>`
> 
> **Configuration**:
> 1. **Table name**: "Products"
> 
> 2. **Primary key**: Id
> 
> 3. **Discriminator for TPH** (Table Per Hierarchy):
>    ```
>    builder.HasDiscriminator<int>("ProductType")
>        .HasValue<AromaBombelProduct>((int)ProductType.AromaBombel)
>        .HasValue<AromaBottleProduct>((int)ProductType.AromaBottle)
>        .HasValue<AromaDeviceProduct>((int)ProductType.AromaDevice)
>        .HasValue<SanitizingDeviceProduct>((int)ProductType.SanitizingDevice)
>        .HasValue<BatteryProduct>((int)ProductType.Battery);
>    ```
> 
> 4. **Name property** (ProductName value object):
>    - Required, MaxLength(200)
>    - HasConversion: name => name.Value, value => ProductName.Create(value)
> 
> 5. **Description**: Optional, MaxLength(1000)
> 
> 6. **Price property** (Money value object):
>    - Required
>    - Split into two columns: Price_Amount (decimal) and Price_Currency (string, max 3)
>    - Use OwnsOne for complex type:
>      ```
>      builder.OwnsOne(p => p.Price, priceBuilder => {
>          priceBuilder.Property(m => m.Amount).HasColumnName("Price").HasPrecision(18, 2).IsRequired();
>          priceBuilder.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3).IsRequired();
>      });
>      ```
> 
> 7. **PhotoUrl**: Optional, MaxLength(500)
> 
> 8. **StockQuantity**: Required, default 0
> 
> 9. **IsActive**: Required, default true
> 
> 10. **CreatedAt**: Required
> 
> 11. **UpdatedAt**: Required
> 
> 12. **Indexes**:
>     - Unique composite index on (Name, ProductType) - name unique within category
>     - Index on ProductType for filtering
>     - Index on IsActive for filtering
> 
> Add imports for all entity and enum types
> Add XML documentation

### Task 1.2: Aroma Bombel Product Configuration

**Location**: `InventoryManagement.Infrastructure/Data/Configurations/AromaBombelProductConfiguration.cs`

**Prompt for Copilot:**

> Create AromaBombelProductConfiguration class:
> 
> **Class**: Implements `IEntityTypeConfiguration<AromaBombelProduct>`
> 
> **Configuration**:
> 1. Configure Taste property:
>    - Optional (nullable)
>    - Store as int (enum value)
>    - HasColumnName("Taste")
> 
> Note: This inherits table from Product (TPH), just configures specific columns

### Task 1.3: Aroma Bottle Product Configuration

**Location**: `InventoryManagement.Infrastructure/Data/Configurations/AromaBottleProductConfiguration.cs`

**Prompt for Copilot:**

> Create AromaBottleProductConfiguration (similar to AromaBombel):
> - Configure Taste property as optional int

### Task 1.4: Aroma Device Product Configuration

**Location**: `InventoryManagement.Infrastructure/Data/Configurations/AromaDeviceProductConfiguration.cs`

**Prompt for Copilot:**

> Create AromaDeviceProductConfiguration:
> 
> **Configuration**:
> 1. Color: Optional int
> 2. Format: Optional string, MaxLength(200)
> 3. Programs: Optional string, MaxLength(2000)
> 4. PlugType: Required int
> 5. SquareMeter: Optional decimal, HasPrecision(10, 2)

### Task 1.5: Sanitizing Device Product Configuration

**Location**: `InventoryManagement.Infrastructure/Data/Configurations/SanitizingDeviceProductConfiguration.cs`

**Prompt for Copilot:**

> Create SanitizingDeviceProductConfiguration:
> 
> **Configuration**:
> 1. Color: Optional int
> 2. Format: Optional string, MaxLength(200)
> 3. Programs: Optional string, MaxLength(2000)
> 4. PlugType: Required int

### Task 1.6: Battery Product Configuration

**Location**: `InventoryManagement.Infrastructure/Data/Configurations/BatteryProductConfiguration.cs`

**Prompt for Copilot:**

> Create BatteryProductConfiguration:
> 
> **Configuration**:
> 1. Type: Optional string, MaxLength(100), HasColumnName("BatteryType")
> 2. Size: Optional int
> 3. Brand: Optional string, MaxLength(100)

### Task 1.7: Stock History Configuration

**Location**: `InventoryManagement.Infrastructure/Data/Configurations/StockHistoryConfiguration.cs`

**Prompt for Copilot:**

> Create StockHistoryConfiguration:
> 
> **Class**: Implements `IEntityTypeConfiguration<StockHistory>`
> 
> **Configuration**:
> 1. Table name: "StockHistories"
> 2. Primary key: Id
> 3. ProductId: Required, with index
> 4. QuantityChanged: Required
> 5. QuantityAfter: Required
> 6. ChangeType: Required, MaxLength(20)
> 7. Reason: Optional, MaxLength(500)
> 8. ChangedBy: Required, with index
> 9. ChangedAt: Required, with index (for date filtering)
> 
> **Relationships**:
> - No navigation property to Product (loose coupling)
> - No foreign key constraint (product might be deleted but history preserved)
> 
> **Indexes**:
> - Index on ProductId
> - Index on ChangedAt descending
> - Index on ChangedBy

---

## Task 2: Update ApplicationDbContext

**Location**: `InventoryManagement.Infrastructure/Data/ApplicationDbContext.cs`

**Prompt for Copilot:**

> Update ApplicationDbContext to add Products and StockHistories DbSets:
> 
> **Add DbSet properties**:
> ```
> public DbSet<Product> Products => Set<Product>();
> public DbSet<StockHistory> StockHistories => Set<StockHistory>();
> ```
> 
> **Add imports** for all entity types
> 
> Keep existing Users DbSet and OnModelCreating method (configurations are auto-applied from assembly)

---

## Task 3: Create Database Migration

### In Package Manager Console (Visual Studio):

**Step 1**: Set default project to `InventoryManagement.Infrastructure`

**Step 2**: Set startup project to `InventoryManagement.API`

**Step 3**: Run migration command:
```powershell
Add-Migration AddProductsAndStockHistory
```

**Step 4**: Review the generated migration file:
- Should create Products table with discriminator
- Should create StockHistories table
- Should have proper indexes

**Step 5**: Apply migration:
```powershell
Update-Database
```

**Expected Tables**:
- Products (single table with all product columns + discriminator)
- StockHistories

---

## Task 4: Create API Controllers

### Task 4.1: Products Controller

**Location**: `InventoryManagement.API/Controllers/ProductsController.cs`

**Prompt for Copilot:**

> Create ProductsController following these specifications:
> 
> **Namespace**: `InventoryManagement.API.Controllers`
> 
> **Attributes**:
> - [Authorize]
> - [ApiController]
> - [Route("api/[controller]")]
> 
> **Constructor**:
> - Inject IMediator mediator
> 
> **Endpoints**:
> 
> 1. **GetAll** [HttpGet]:
>    - Query parameter: includeInactive (bool, default false)
>    - Send GetAllProductsQuery
>    - Return Ok(result)
>    - ProducesResponseType: 200
> 
> 2. **GetByType** [HttpGet("type/{productTypeId:int}")]:
>    - Route parameter: productTypeId
>    - Query parameter: includeInactive
>    - Send GetProductsByTypeQuery
>    - Return Ok(result)
>    - ProducesResponseType: 200
> 
> 3. **GetById** [HttpGet("{id:guid}")]:
>    - Route parameter: id
>    - Send GetProductByIdQuery
>    - Return Ok(result)
>    - ProducesResponseType: 200, 404
> 
> 4. **GetLowStock** [HttpGet("low-stock")]:
>    - Query parameter: threshold (int, default 10)
>    - Send GetLowStockProductsQuery
>    - Return Ok(result)
>    - ProducesResponseType: 200
> 
> 5. **CreateAromaBombel** [HttpPost("aroma-bombel")]:
>    - Body: CreateAromaBombelCommand
>    - Send command
>    - Return CreatedAtAction(nameof(GetById), new { id }, id)
>    - ProducesResponseType: 201, 400, 409
> 
> 6. **CreateAromaBottle** [HttpPost("aroma-bottle")]:
>    - Body: CreateAromaBottleCommand
>    - Return CreatedAtAction
> 
> 7. **CreateAromaDevice** [HttpPost("aroma-device")]:
>    - Body: CreateAromaDeviceCommand
>    - Return CreatedAtAction
> 
> 8. **CreateSanitizingDevice** [HttpPost("sanitizing-device")]:
>    - Body: CreateSanitizingDeviceCommand
>    - Return CreatedAtAction
> 
> 9. **CreateBattery** [HttpPost("battery")]:
>    - Body: CreateBatteryCommand
>    - Return CreatedAtAction
> 
> 10. **UpdateAromaBombel** [HttpPut("aroma-bombel/{id:guid}")]:
>     - Route: id, Body: UpdateAromaBombelCommand
>     - Validate id matches command.ProductId
>     - Send command
>     - Return NoContent()
>     - ProducesResponseType: 204, 400, 404, 409
> 
> 11. **UpdateAromaBottle** [HttpPut("aroma-bottle/{id:guid}")]
> 
> 12. **UpdateAromaDevice** [HttpPut("aroma-device/{id:guid}")]
> 
> 13. **UpdateSanitizingDevice** [HttpPut("sanitizing-device/{id:guid}")]
> 
> 14. **UpdateBattery** [HttpPut("battery/{id:guid}")]
> 
> 15. **Delete** [HttpDelete("{id:guid}")]:
>     - Route: id
>     - Send DeleteProductCommand
>     - Return NoContent()
>     - ProducesResponseType: 204, 404
> 
> Add XML documentation for all endpoints

### Task 4.2: Stock Controller

**Location**: `InventoryManagement.API/Controllers/StockController.cs`

**Prompt for Copilot:**

> Create StockController following these specifications:
> 
> **Namespace**: `InventoryManagement.API.Controllers`
> 
> **Attributes**:
> - [Authorize]
> - [ApiController]
> - [Route("api/[controller]")]
> 
> **Endpoints**:
> 
> 1. **AddStock** [HttpPost("add")]:
>    - Body: AddStockCommand
>    - Send command
>    - Return Ok(new { ProductId = command.ProductId, NewStockQuantity = result })
>    - ProducesResponseType: 200, 400, 404
> 
> 2. **RemoveStock** [HttpPost("remove")]:
>    - Body: RemoveStockCommand
>    - Send command
>    - Return Ok(new { ProductId = command.ProductId, NewStockQuantity = result })
>    - ProducesResponseType: 200, 400, 404, 409 (insufficient stock)
> 
> 3. **GetHistory** [HttpGet("history")]:
>    - Query parameters: productId (Guid?), fromDate (DateTime?), toDate (DateTime?), take (int, default 50)
>    - Send GetStockHistoryQuery
>    - Return Ok(result)
>    - ProducesResponseType: 200
> 
> 4. **GetProductHistory** [HttpGet("history/{productId:guid}")]:
>    - Route: productId
>    - Query: fromDate, toDate, take
>    - Send GetStockHistoryQuery with productId
>    - Return Ok(result)
>    - ProducesResponseType: 200
> 
> Add XML documentation

---

## Task 5: Update Exception Handling Middleware

**Location**: `InventoryManagement.API/Middleware/ExceptionHandlingMiddleware.cs`

**Prompt for Copilot:**

> Update ExceptionHandlingMiddleware to handle new product exceptions:
> 
> **Add cases to switch expression**:
> 
> - ProductNotFoundException → 404 NotFound
> - DuplicateProductNameException → 409 Conflict
> - InsufficientStockException → 409 Conflict (or 400 BadRequest)
> 
> Keep existing exception handlers for User exceptions

---

## Task 6: Create Integration Tests

### Task 6.1: Products Controller Tests

**Location**: `InventoryManagement.API.Tests/IntegrationTests/ProductsControllerTests.cs`

**Prompt for Copilot:**

> Create integration tests for ProductsController:
> 
> **Test Methods**:
> 
> 1. **CreateAromaBombel_WithValidData_ShouldReturnCreated**
> 2. **CreateAromaBombel_WithDuplicateName_ShouldReturnConflict**
> 3. **CreateAromaBombel_WithInvalidPrice_ShouldReturnBadRequest**
> 4. **GetAll_ShouldReturnProducts**
> 5. **GetByType_AromaBombel_ShouldReturnOnlyAromaBombelProducts**
> 6. **GetById_WithValidId_ShouldReturnProduct**
> 7. **GetById_WithInvalidId_ShouldReturnNotFound**
> 8. **Update_WithValidData_ShouldReturnNoContent**
> 9. **Delete_WithValidId_ShouldReturnNoContent**
> 10. **GetLowStock_ShouldReturnProductsBelowThreshold**
> 
> Use WebApplicationFactory<Program>
> Login first to get JWT token
> Set Authorization header

### Task 6.2: Stock Controller Tests

**Location**: `InventoryManagement.API.Tests/IntegrationTests/StockControllerTests.cs`

**Prompt for Copilot:**

> Create integration tests for StockController:
> 
> **Test Methods**:
> 
> 1. **AddStock_WithValidQuantity_ShouldIncreaseStock**
> 2. **AddStock_WithInvalidProductId_ShouldReturnNotFound**
> 3. **RemoveStock_WithValidQuantity_ShouldDecreaseStock**
> 4. **RemoveStock_WithInsufficientStock_ShouldReturnConflict**
> 5. **GetHistory_ShouldReturnStockHistoryRecords**
> 6. **GetHistory_WithProductId_ShouldReturnFilteredHistory**

---

## Task 7: Test the API with Swagger

### Run the Application:

1. Start API from Visual Studio (F5)
2. Open Swagger: `https://localhost:7173/swagger`

### Test Scenarios:

**1. Login First**:
```json
POST /api/auth/login
{
  "email": "admin@inventoryapp.com",
  "password": "Admin@123"
}
```
Copy token and click "Authorize" → Enter `Bearer <token>`

**2. Create Aroma Bombel Product**:
```json
POST /api/products/aroma-bombel
{
  "name": "Rose Aroma Bomb",
  "description": "Beautiful rose scent",
  "price": 500,
  "currency": "ALL",
  "photoUrl": null,
  "tasteId": 1
}
```
Expected: 201 Created

**3. Create Aroma Device Product**:
```json
POST /api/products/aroma-device
{
  "name": "Premium Diffuser",
  "description": "High-end aroma diffuser",
  "price": 15000,
  "currency": "ALL",
  "colorId": 9,
  "format": "Cylinder",
  "programs": "Timer 1h, Timer 2h, Continuous",
  "plugTypeId": 1,
  "squareMeter": 50.5
}
```

**4. Create Battery Product**:
```json
POST /api/products/battery
{
  "name": "Duracell AA",
  "description": "Long lasting battery",
  "price": 100,
  "currency": "ALL",
  "type": "Alkaline",
  "sizeId": 1,
  "brand": "Duracell"
}
```

**5. Get All Products**:
```
GET /api/products
```

**6. Get Products by Type** (Aroma Bombel = 1):
```
GET /api/products/type/1
```

**7. Add Stock**:
```json
POST /api/stock/add
{
  "productId": "<guid-from-create>",
  "quantity": 100,
  "reason": "Initial stock"
}
```

**8. Remove Stock**:
```json
POST /api/stock/remove
{
  "productId": "<guid>",
  "quantity": 5,
  "reason": "Sale"
}
```

**9. Get Stock History**:
```
GET /api/stock/history/<productId>
```

**10. Get Low Stock Products**:
```
GET /api/products/low-stock?threshold=10
```

---

## Verification Checklist

Before considering Step 4C complete, verify:

- [ ] All EF Core configurations created
- [ ] ApplicationDbContext updated with Products and StockHistories
- [ ] Migration created and applied
- [ ] Products table created with TPH discriminator
- [ ] StockHistories table created
- [ ] ProductsController created with all endpoints
- [ ] StockController created
- [ ] Exception middleware updated
- [ ] Integration tests created
- [ ] Can create products of all 5 types via API
- [ ] Can update products
- [ ] Can delete (deactivate) products
- [ ] Can add/remove stock
- [ ] Stock history is recorded
- [ ] Can query low stock products
- [ ] Duplicate name validation works
- [ ] All tests pass

---

## API Endpoints Summary

### Products Controller (`/api/products`):

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/products` | Get all products |
| GET | `/api/products/type/{typeId}` | Get products by category |
| GET | `/api/products/{id}` | Get product by ID |
| GET | `/api/products/low-stock` | Get low stock products |
| POST | `/api/products/aroma-bombel` | Create Aroma Bombel |
| POST | `/api/products/aroma-bottle` | Create Aroma Bottle |
| POST | `/api/products/aroma-device` | Create Aroma Device |
| POST | `/api/products/sanitizing-device` | Create Sanitizing Device |
| POST | `/api/products/battery` | Create Battery |
| PUT | `/api/products/aroma-bombel/{id}` | Update Aroma Bombel |
| PUT | `/api/products/aroma-bottle/{id}` | Update Aroma Bottle |
| PUT | `/api/products/aroma-device/{id}` | Update Aroma Device |
| PUT | `/api/products/sanitizing-device/{id}` | Update Sanitizing Device |
| PUT | `/api/products/battery/{id}` | Update Battery |
| DELETE | `/api/products/{id}` | Delete (deactivate) product |

### Stock Controller (`/api/stock`):

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/stock/add` | Add stock to product |
| POST | `/api/stock/remove` | Remove stock from product |
| GET | `/api/stock/history` | Get stock history (all) |
| GET | `/api/stock/history/{productId}` | Get stock history for product |

---

## What You've Accomplished

✅ **EF Core Configuration** for TPH product hierarchy  
✅ **Database Migration** with Products and StockHistories tables  
✅ **REST API Controllers** for products and stock  
✅ **Stock Management** with history tracking  
✅ **Integration Tests** for API endpoints  
✅ **Exception Handling** for product-specific errors  

---

## Database Schema

### Products Table (TPH):
```
Products
├── Id (uniqueidentifier, PK)
├── Name (nvarchar(200))
├── Description (nvarchar(1000), nullable)
├── ProductType (int) - DISCRIMINATOR
├── Price (decimal(18,2))
├── Currency (nvarchar(3))
├── PhotoUrl (nvarchar(500), nullable)
├── StockQuantity (int)
├── IsActive (bit)
├── CreatedAt (datetime2)
├── UpdatedAt (datetime2)
├── Taste (int, nullable) - AromaBombel/AromaBottle
├── Color (int, nullable) - AromaDevice/SanitizingDevice
├── Format (nvarchar(200), nullable) - AromaDevice/SanitizingDevice
├── Programs (nvarchar(2000), nullable) - AromaDevice/SanitizingDevice
├── PlugType (int, nullable) - AromaDevice/SanitizingDevice
├── SquareMeter (decimal(10,2), nullable) - AromaDevice only
├── BatteryType (nvarchar(100), nullable) - Battery
├── Size (int, nullable) - Battery
└── Brand (nvarchar(100), nullable) - Battery
```

### StockHistories Table:
```
StockHistories
├── Id (uniqueidentifier, PK)
├── ProductId (uniqueidentifier)
├── QuantityChanged (int)
├── QuantityAfter (int)
├── ChangeType (nvarchar(20))
├── Reason (nvarchar(500), nullable)
├── ChangedBy (uniqueidentifier)
└── ChangedAt (datetime2)
```

---

## Next Steps

**Inventory Backend Complete!** 🎉

Now you can:
1. Build the **React Frontend** for Inventory Management
2. Add more features (reports, dashboards)
3. Add photo upload functionality
4. Implement real-time low stock alerts

---

## Tips

1. **Test each endpoint** in Swagger before moving on
2. **Create one product type first** - verify it works, then add others
3. **Check database** in SQL Server Object Explorer to verify data
4. **Review migration** before applying to ensure correct schema
5. **Run integration tests** to catch issues early

Congratulations on completing the Inventory Module backend! 🎊
