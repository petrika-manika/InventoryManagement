# Step 5A: React UI - Inventory Module Setup
## Instructions for GitHub Copilot

---

## Overview
This guide provides instructions for setting up the React UI components for the Inventory module. We'll create the folder structure, TypeScript types, API services, and common components needed for product management.

**Prerequisites**: Complete User Management UI (Steps 3A-3C) and Inventory Backend (Steps 4A-4C)

---

## Folder Structure

Create this folder structure in `src/`:

```
src/
├── components/
│   ├── common/          # (already exists from User Management)
│   ├── auth/            # (already exists)
│   ├── users/           # (already exists)
│   ├── products/        # NEW - Product components
│   │   ├── ProductCard.tsx
│   │   ├── ProductForm.tsx
│   │   ├── ProductTable.tsx
│   │   ├── ProductFilters.tsx
│   │   ├── StockModal.tsx
│   │   └── StockHistoryTable.tsx
│   └── layout/          # (already exists)
├── pages/
│   ├── LoginPage.tsx    # (exists)
│   ├── UsersPage.tsx    # (exists)
│   ├── ProductsPage.tsx # NEW
│   ├── ProductDetailPage.tsx # NEW
│   └── StockHistoryPage.tsx  # NEW
├── services/
│   ├── apiClient.ts     # (exists)
│   ├── authService.ts   # (exists)
│   ├── userService.ts   # (exists)
│   ├── productService.ts # NEW
│   └── stockService.ts   # NEW
├── types/
│   ├── user.types.ts    # (exists)
│   ├── api.types.ts     # (exists)
│   └── product.types.ts # NEW
└── utils/
    ├── apiConfig.ts     # UPDATE with product endpoints
    └── formatters.ts    # NEW - formatting utilities
```

Create the new folders:
```bash
mkdir src/components/products
```

---

## Task 1: Create TypeScript Types

### Location: `src/types/product.types.ts`

**Prompt for Copilot:**

> Create TypeScript types for the Inventory module following these specifications based on the backend enums:
> 
> **Enums** (export as const objects for better TypeScript support):
> 
> 1. **ProductType** (5 types):
>    ```typescript
>    export const ProductType = {
>      AromaBombel: 1,
>      AromaBottle: 2,
>      AromaDevice: 3,
>      SanitizingDevice: 4,
>      Battery: 5,
>    } as const;
>    export type ProductTypeValue = typeof ProductType[keyof typeof ProductType];
>    ```
> 
> 2. **ProductTypeLabels** - mapping ID to display name:
>    ```typescript
>    export const ProductTypeLabels: Record<number, string> = {
>      1: 'Aroma Bombel',
>      2: 'Aroma Bottle',
>      3: 'Aroma Device',
>      4: 'Sanitizing Device',
>      5: 'Battery',
>    };
>    ```
> 
> 3. **TasteType** (4 tastes):
>    ```typescript
>    export const TasteType = {
>      Flower: 1,
>      Sweet: 2,
>      Fresh: 3,
>      Fruit: 4,
>    } as const;
>    export type TasteTypeValue = typeof TasteType[keyof typeof TasteType];
>    ```
>    
>    Also create **TasteTypeLabels** mapping:
>    ```typescript
>    export const TasteTypeLabels: Record<number, string> = {
>      1: 'Flower',
>      2: 'Sweet',
>      3: 'Fresh',
>      4: 'Fruit',
>    };
>    ```
> 
> 4. **ColorType** (11 colors):
>    ```typescript
>    export const ColorType = {
>      Red: 1,
>      Blue: 2,
>      Green: 3,
>      Yellow: 4,
>      Orange: 5,
>      Purple: 6,
>      Pink: 7,
>      Brown: 8,
>      Black: 9,
>      White: 10,
>      Gray: 11,
>    } as const;
>    export type ColorTypeValue = typeof ColorType[keyof typeof ColorType];
>    ```
>    
>    Also create **ColorTypeLabels** mapping:
>    ```typescript
>    export const ColorTypeLabels: Record<number, string> = {
>      1: 'Red',
>      2: 'Blue',
>      3: 'Green',
>      4: 'Yellow',
>      5: 'Orange',
>      6: 'Purple',
>      7: 'Pink',
>      8: 'Brown',
>      9: 'Black',
>      10: 'White',
>      11: 'Gray',
>    };
>    ```
> 
> 5. **DevicePlugType** (2 types):
>    ```typescript
>    export const DevicePlugType = {
>      WithPlug: 1,
>      WithoutPlug: 2,
>    } as const;
>    export type DevicePlugTypeValue = typeof DevicePlugType[keyof typeof DevicePlugType];
>    ```
>    
>    Also create **DevicePlugTypeLabels** mapping:
>    ```typescript
>    export const DevicePlugTypeLabels: Record<number, string> = {
>      1: 'With Plug',
>      2: 'Without Plug',
>    };
>    ```
> 
> 6. **BatterySize** (2 sizes - LR6 is AA, LR9 is AAA):
>    ```typescript
>    export const BatterySize = {
>      LR6: 1,  // AA battery
>      LR9: 2,  // AAA battery
>    } as const;
>    export type BatterySizeValue = typeof BatterySize[keyof typeof BatterySize];
>    ```
>    
>    Also create **BatterySizeLabels** mapping:
>    ```typescript
>    export const BatterySizeLabels: Record<number, string> = {
>      1: 'LR6 (AA)',
>      2: 'LR9 (AAA)',
>    };
>    ```
> 
> **Interfaces**:
> 
> 1. **ProductDto** (base interface for all products):
>    ```typescript
>    export interface ProductDto {
>      id: string;
>      name: string;
>      description: string | null;
>      productType: string;           // e.g., "AromaBombel"
>      productTypeId: number;          // e.g., 1
>      price: number;
>      currency: string;               // e.g., "ALL"
>      photoUrl: string | null;
>      stockQuantity: number;
>      isActive: boolean;
>      isLowStock: boolean;
>      createdAt: string;              // ISO date string
>      updatedAt: string;              // ISO date string
>    }
>    ```
> 
> 2. **AromaBombelProductDto** extends ProductDto:
>    ```typescript
>    export interface AromaBombelProductDto extends ProductDto {
>      taste: string | null;           // e.g., "Flower"
>      tasteId: number | null;         // e.g., 1
>    }
>    ```
> 
> 3. **AromaBottleProductDto** extends ProductDto:
>    ```typescript
>    export interface AromaBottleProductDto extends ProductDto {
>      taste: string | null;
>      tasteId: number | null;
>    }
>    ```
> 
> 4. **AromaDeviceProductDto** extends ProductDto:
>    ```typescript
>    export interface AromaDeviceProductDto extends ProductDto {
>      color: string | null;           // e.g., "Red"
>      colorId: number | null;         // e.g., 1
>      format: string | null;          // Device format description
>      programs: string | null;        // Device programs description
>      plugType: string;               // e.g., "WithPlug"
>      plugTypeId: number;             // e.g., 1
>      squareMeter: number | null;     // Coverage area in square meters
>    }
>    ```
> 
> 5. **SanitizingDeviceProductDto** extends ProductDto:
>    ```typescript
>    export interface SanitizingDeviceProductDto extends ProductDto {
>      color: string | null;
>      colorId: number | null;
>      format: string | null;
>      programs: string | null;
>      plugType: string;
>      plugTypeId: number;
>      // Note: SanitizingDevice does NOT have squareMeter field
>    }
>    ```
> 
> 6. **BatteryProductDto** extends ProductDto:
>    ```typescript
>    export interface BatteryProductDto extends ProductDto {
>      type: string | null;            // Battery type (e.g., "Lithium Ion")
>      size: string | null;            // e.g., "LR6"
>      sizeId: number | null;          // e.g., 1
>      brand: string | null;           // Battery brand
>    }
>    ```
> 
> **Request Interfaces for Creating Products**:
> 
> 7. **CreateAromaBombelRequest**:
>    ```typescript
>    export interface CreateAromaBombelRequest {
>      name: string;
>      description?: string;
>      price: number;
>      currency: string;               // Default: "ALL"
>      photoUrl?: string;
>      tasteId?: number;               // Optional taste selection
>    }
>    ```
> 
> 8. **CreateAromaBottleRequest** (identical to AromaBombel):
>    ```typescript
>    export interface CreateAromaBottleRequest {
>      name: string;
>      description?: string;
>      price: number;
>      currency: string;
>      photoUrl?: string;
>      tasteId?: number;
>    }
>    ```
> 
> 9. **CreateAromaDeviceRequest**:
>    ```typescript
>    export interface CreateAromaDeviceRequest {
>      name: string;
>      description?: string;
>      price: number;
>      currency: string;
>      photoUrl?: string;
>      colorId?: number;               // Optional color
>      format?: string;                // Device format
>      programs?: string;              // Device programs
>      plugTypeId: number;             // REQUIRED - 1 or 2
>      squareMeter?: number;           // Coverage area
>    }
>    ```
> 
> 10. **CreateSanitizingDeviceRequest**:
>     ```typescript
>     export interface CreateSanitizingDeviceRequest {
>       name: string;
>       description?: string;
>       price: number;
>       currency: string;
>       photoUrl?: string;
>       colorId?: number;
>       format?: string;
>       programs?: string;
>       plugTypeId: number;            // REQUIRED
>       // No squareMeter for SanitizingDevice
>     }
>     ```
> 
> 11. **CreateBatteryRequest**:
>     ```typescript
>     export interface CreateBatteryRequest {
>       name: string;
>       description?: string;
>       price: number;
>       currency: string;
>       photoUrl?: string;
>       type?: string;                 // Battery type
>       sizeId?: number;               // LR6 (1) or LR9 (2)
>       brand?: string;                // Battery brand
>     }
>     ```
> 
> **Update Request Interfaces** (same as Create but with productId):
> 
> 12. **UpdateAromaBombelRequest** extends CreateAromaBombelRequest:
>     ```typescript
>     export interface UpdateAromaBombelRequest extends CreateAromaBombelRequest {
>       productId: string;
>     }
>     ```
> 
> 13. Similarly create: **UpdateAromaBottleRequest**, **UpdateAromaDeviceRequest**, **UpdateSanitizingDeviceRequest**, **UpdateBatteryRequest**
> 
> **Stock Management Interfaces**:
> 
> 14. **AddStockRequest**:
>     ```typescript
>     export interface AddStockRequest {
>       productId: string;
>       quantity: number;              // Must be positive
>       reason?: string;               // Optional reason for adding stock
>     }
>     ```
> 
> 15. **RemoveStockRequest** (identical to AddStock):
>     ```typescript
>     export interface RemoveStockRequest {
>       productId: string;
>       quantity: number;              // Must be positive
>       reason?: string;               // Optional reason for removing stock
>     }
>     ```
> 
> 16. **StockHistoryDto**:
>     ```typescript
>     export interface StockHistoryDto {
>       id: string;
>       productId: string;
>       productName: string;           // Product name at time of change
>       quantityChanged: number;       // Positive for add, negative for remove
>       quantityAfter: number;         // Stock quantity after change
>       changeType: string;            // "Added" | "Removed" | "Adjusted"
>       reason: string | null;         // Reason for stock change
>       changedBy: string;             // User ID who made the change
>       changedByName: string;         // User full name
>       changedAt: string;             // ISO date string
>     }
>     ```
> 
> **Add JSDoc comments** for each interface explaining its purpose and usage.
> 
> **Export all types, enums, and label mappings**

---

## Task 2: Update API Configuration

### Location: `src/utils/apiConfig.ts`

**Prompt for Copilot:**

> Update apiConfig.ts to add product and stock endpoints:
> 
> **Add to API_ENDPOINTS object**:
> 
> ```typescript
> PRODUCTS: {
>   BASE: '/api/products',
>   GET_ALL: '/api/products',
>   GET_BY_TYPE: (typeId: number) => `/api/products/type/${typeId}`,
>   GET_BY_ID: (id: string) => `/api/products/${id}`,
>   GET_LOW_STOCK: '/api/products/low-stock',
>   
>   // Create endpoints for each product type
>   CREATE_AROMA_BOMBEL: '/api/products/aroma-bombel',
>   CREATE_AROMA_BOTTLE: '/api/products/aroma-bottle',
>   CREATE_AROMA_DEVICE: '/api/products/aroma-device',
>   CREATE_SANITIZING_DEVICE: '/api/products/sanitizing-device',
>   CREATE_BATTERY: '/api/products/battery',
>   
>   // Update endpoints for each product type
>   UPDATE_AROMA_BOMBEL: (id: string) => `/api/products/aroma-bombel/${id}`,
>   UPDATE_AROMA_BOTTLE: (id: string) => `/api/products/aroma-bottle/${id}`,
>   UPDATE_AROMA_DEVICE: (id: string) => `/api/products/aroma-device/${id}`,
>   UPDATE_SANITIZING_DEVICE: (id: string) => `/api/products/sanitizing-device/${id}`,
>   UPDATE_BATTERY: (id: string) => `/api/products/battery/${id}`,
>   
>   // Delete endpoint (common for all types)
>   DELETE: (id: string) => `/api/products/${id}`,
> },
> 
> STOCK: {
>   ADD: '/api/stock/add',
>   REMOVE: '/api/stock/remove',
>   HISTORY: '/api/stock/history',
>   HISTORY_BY_PRODUCT: (productId: string) => `/api/stock/history/${productId}`,
> },
> ```
> 
> Keep existing AUTH and USERS endpoints

---

## Task 3: Create Formatting Utilities

### Location: `src/utils/formatters.ts`

**Prompt for Copilot:**

> Create formatting utility functions:
> 
> **Functions**:
> 
> 1. **formatCurrency(amount: number, currency: string = 'ALL')** → string:
>    - Format number with thousand separators
>    - Append currency code
>    - Example: formatCurrency(1500, 'ALL') → '1,500 ALL'
>    - Use Intl.NumberFormat for proper formatting
> 
> 2. **formatDate(dateString: string)** → string:
>    - Parse ISO date string
>    - Return formatted date: 'MMM dd, yyyy'
>    - Example: '2024-11-25T10:30:00Z' → 'Nov 25, 2024'
> 
> 3. **formatDateTime(dateString: string)** → string:
>    - Format with time: 'MMM dd, yyyy HH:mm'
>    - Example: '2024-11-25T10:30:00Z' → 'Nov 25, 2024 10:30'
> 
> 4. **formatStockChange(quantity: number)** → string:
>    - If positive: return '+{quantity}' (for additions)
>    - If negative: return '{quantity}' (already has minus sign)
>    - Example: formatStockChange(50) → '+50'
>    - Example: formatStockChange(-20) → '-20'
> 
> 5. **getStockChangeColor(quantity: number)** → string:
>    - Return TailwindCSS color class
>    - Positive: 'text-green-600'
>    - Negative: 'text-red-600'
>    - Zero: 'text-gray-600'
> 
> 6. **getStockBadgeVariant(stockQuantity: number, isLowStock: boolean)** → object:
>    - Return { color: string, label: string }
>    - If stockQuantity === 0: { color: 'red', label: 'Out of Stock' }
>    - If isLowStock: { color: 'yellow', label: 'Low Stock' }
>    - Else: { color: 'green', label: 'In Stock' }
> 
> 7. **getProductTypeLabel(typeId: number)** → string:
>    - Use ProductTypeLabels mapping
>    - Return label or 'Unknown'
> 
> 8. **getTasteTypeLabel(tasteId: number | null)** → string:
>    - Use TasteTypeLabels mapping
>    - Return label or 'N/A' if null
> 
> 9. **getColorTypeLabel(colorId: number | null)** → string:
>    - Use ColorTypeLabels mapping
>    - Return label or 'N/A' if null
> 
> 10. **getPlugTypeLabel(plugTypeId: number)** → string:
>     - Use DevicePlugTypeLabels mapping
>     - Return label or 'Unknown'
> 
> 11. **getBatterySizeLabel(sizeId: number | null)** → string:
>     - Use BatterySizeLabels mapping
>     - Return label or 'N/A' if null
> 
> **Import all enum label mappings from product.types.ts**
> 
> **Export all functions**

---

## Task 4: Create Product Service

### Location: `src/services/productService.ts`

**Prompt for Copilot:**

> Create productService for product-related API calls:
> 
> **Imports**:
> - apiClient, API_ENDPOINTS from utils
> - All product type interfaces, request interfaces from product.types.ts
> 
> **productService object with methods**:
> 
> 1. **getAllProducts()** → Promise<ProductDto[]>:
>    - GET to PRODUCTS.GET_ALL
>    - Return response.data (array of all products)
> 
> 2. **getProductsByType(typeId: number)** → Promise<ProductDto[]>:
>    - GET to PRODUCTS.GET_BY_TYPE(typeId)
>    - Return response.data
>    - Use when filtering by product type
> 
> 3. **getProductById(id: string)** → Promise<ProductDto>:
>    - GET to PRODUCTS.GET_BY_ID(id)
>    - Return response.data (single product with all details)
> 
> 4. **getLowStockProducts(threshold: number = 10)** → Promise<ProductDto[]>:
>    - GET to PRODUCTS.GET_LOW_STOCK with threshold query param
>    - Return response.data
>    - Use for low stock alerts
> 
> **Create Methods** (one for each product type):
> 
> 5. **createAromaBombel(data: CreateAromaBombelRequest)** → Promise<string>:
>    - POST to PRODUCTS.CREATE_AROMA_BOMBEL
>    - Body: data object
>    - Return response.data (new product ID)
> 
> 6. **createAromaBottle(data: CreateAromaBottleRequest)** → Promise<string>:
>    - POST to PRODUCTS.CREATE_AROMA_BOTTLE
>    - Return product ID
> 
> 7. **createAromaDevice(data: CreateAromaDeviceRequest)** → Promise<string>:
>    - POST to PRODUCTS.CREATE_AROMA_DEVICE
>    - Return product ID
> 
> 8. **createSanitizingDevice(data: CreateSanitizingDeviceRequest)** → Promise<string>:
>    - POST to PRODUCTS.CREATE_SANITIZING_DEVICE
>    - Return product ID
> 
> 9. **createBattery(data: CreateBatteryRequest)** → Promise<string>:
>    - POST to PRODUCTS.CREATE_BATTERY
>    - Return product ID
> 
> **Update Methods** (one for each product type):
> 
> 10. **updateAromaBombel(id: string, data: UpdateAromaBombelRequest)** → Promise<void>:
>     - PUT to PRODUCTS.UPDATE_AROMA_BOMBEL(id)
>     - Body: data object
> 
> 11. **updateAromaBottle(id: string, data: UpdateAromaBottleRequest)** → Promise<void>
> 
> 12. **updateAromaDevice(id: string, data: UpdateAromaDeviceRequest)** → Promise<void>
> 
> 13. **updateSanitizingDevice(id: string, data: UpdateSanitizingDeviceRequest)** → Promise<void>
> 
> 14. **updateBattery(id: string, data: UpdateBatteryRequest)** → Promise<void>
> 
> **Delete Method**:
> 
> 15. **deleteProduct(id: string)** → Promise<void>:
>     - DELETE to PRODUCTS.DELETE(id)
>     - Works for all product types
> 
> **Helper Methods** (simplify form submission):
> 
> 16. **createProduct(typeId: number, data: any)** → Promise<string>:
>     - Switch on typeId to call correct create method:
>       - typeId 1 (AromaBombel) → createAromaBombel(data)
>       - typeId 2 (AromaBottle) → createAromaBottle(data)
>       - typeId 3 (AromaDevice) → createAromaDevice(data)
>       - typeId 4 (SanitizingDevice) → createSanitizingDevice(data)
>       - typeId 5 (Battery) → createBattery(data)
>     - Return product ID
>     - Throw error if typeId invalid
> 
> 17. **updateProduct(typeId: number, id: string, data: any)** → Promise<void>:
>     - Switch on typeId to call correct update method
>     - Similar logic to createProduct
> 
> **Export productService as default**
> 
> **Add JSDoc comments** for each method explaining parameters and return values
> 
> **Include try-catch blocks** for error handling

---

## Task 5: Create Stock Service

### Location: `src/services/stockService.ts`

**Prompt for Copilot:**

> Create stockService for stock management API calls:
> 
> **Imports**:
> - apiClient, API_ENDPOINTS from utils
> - AddStockRequest, RemoveStockRequest, StockHistoryDto from product.types.ts
> 
> **stockService object with methods**:
> 
> 1. **addStock(data: AddStockRequest)** → Promise<{ productId: string; newStockQuantity: number }>:
>    - POST to STOCK.ADD
>    - Body: { productId, quantity, reason }
>    - Return response.data (productId and new stock quantity)
>    - Use for adding stock to products
> 
> 2. **removeStock(data: RemoveStockRequest)** → Promise<{ productId: string; newStockQuantity: number }>:
>    - POST to STOCK.REMOVE
>    - Body: { productId, quantity, reason }
>    - Return response.data
>    - Use for removing stock from products
> 
> 3. **getStockHistory(params: { productId?: string; fromDate?: string; toDate?: string; take?: number })** → Promise<StockHistoryDto[]>:
>    - GET to STOCK.HISTORY with query params
>    - Params:
>      - productId (optional): filter by specific product
>      - fromDate (optional): filter from date (ISO string)
>      - toDate (optional): filter to date (ISO string)
>      - take (optional): limit number of results (default 50)
>    - Return response.data (array of stock history entries)
>    - Use for full stock history with filters
> 
> 4. **getProductStockHistory(productId: string, take: number = 50)** → Promise<StockHistoryDto[]>:
>    - GET to STOCK.HISTORY_BY_PRODUCT(productId) with take query param
>    - Return response.data (stock history for specific product)
>    - Use for product detail page stock history
> 
> **Export stockService as default**
> 
> **Add JSDoc comments** for each method
> 
> **Include try-catch blocks** for error handling

---

## Task 6: Create Select/Dropdown Components for Enums

### Location: `src/components/common/Select.tsx`

**Prompt for Copilot:**

> Create reusable Select component with TailwindCSS:
> 
> **Props interface (SelectProps)**:
> ```typescript
> interface SelectProps {
>   label?: string;
>   options: Array<{ value: string | number; label: string }>;
>   value: string | number | undefined;
>   onChange: (value: string | number) => void;
>   placeholder?: string;
>   error?: string;
>   disabled?: boolean;
>   required?: boolean;
>   className?: string;
> }
> ```
> 
> **Component**:
> - Use React.forwardRef for react-hook-form compatibility
> - Render label if provided
> - Render select element with options
> - Include placeholder option if provided (value: '', disabled)
> - Show error message below select if error exists
> - Apply disabled styling if disabled
> - Show required asterisk (*) if required
> 
> **Styling (TailwindCSS)**:
> - Label: 'block text-sm font-medium text-gray-700 mb-1'
> - Required asterisk: 'text-red-500 ml-1'
> - Select base: 'w-full px-3 py-2 border rounded-lg shadow-sm focus:outline-none focus:ring-2'
> - Normal state: 'border-gray-300 focus:border-primary-500 focus:ring-primary-500'
> - Error state: 'border-red-300 focus:border-red-500 focus:ring-red-500'
> - Disabled: 'bg-gray-100 cursor-not-allowed opacity-60'
> - Error message: 'mt-1 text-sm text-red-600'
> 
> **Export Select as default**

### Location: `src/components/common/Textarea.tsx`

**Prompt for Copilot:**

> Create reusable Textarea component with TailwindCSS:
> 
> **Props interface** extends React.TextareaHTMLAttributes<HTMLTextAreaElement>:
> ```typescript
> interface TextareaProps extends React.TextareaHTMLAttributes<HTMLTextAreaElement> {
>   label?: string;
>   error?: string;
>   helperText?: string;
> }
> ```
> 
> **Component**:
> - Use React.forwardRef
> - Render label if provided
> - Render textarea element
> - Show helper text below textarea if provided
> - Show error message below if error exists
> - Default rows: 3
> - Resize: vertical only
> 
> **Styling** (similar to Input and Select components):
> - Label: 'block text-sm font-medium text-gray-700 mb-1'
> - Textarea base: 'w-full px-3 py-2 border rounded-lg shadow-sm focus:outline-none focus:ring-2 resize-y'
> - Normal: 'border-gray-300 focus:border-primary-500 focus:ring-primary-500'
> - Error: 'border-red-300 focus:border-red-500 focus:ring-red-500'
> - Disabled: 'bg-gray-100 cursor-not-allowed opacity-60'
> - Helper text: 'mt-1 text-sm text-gray-500'
> - Error message: 'mt-1 text-sm text-red-600'
> 
> **Export Textarea as default**

---

## Task 7: Create Enum Select Components

### Location: `src/components/products/ProductTypeSelect.tsx`

**Prompt for Copilot:**

> Create ProductTypeSelect component:
> 
> **Props**:
> ```typescript
> interface ProductTypeSelectProps {
>   value: number | undefined;
>   onChange: (value: number) => void;
>   label?: string;               // Default: "Product Type"
>   error?: string;
>   disabled?: boolean;
>   showAll?: boolean;            // If true, adds "All Types" option with value 0
> }
> ```
> 
> **Component**:
> - Import ProductType and ProductTypeLabels from product.types.ts
> - Create options array from ProductTypeLabels:
>   ```typescript
>   const options = Object.entries(ProductTypeLabels).map(([value, label]) => ({
>     value: Number(value),
>     label,
>   }));
>   ```
> - If showAll is true, prepend { value: 0, label: 'All Types' } to options
> - Render Select component with:
>   - label prop (default: "Product Type")
>   - options array
>   - value and onChange from props
>   - error and disabled from props
> 
> **Export ProductTypeSelect as default**

### Location: `src/components/products/TasteSelect.tsx`

**Prompt for Copilot:**

> Create TasteSelect component:
> 
> **Props**:
> ```typescript
> interface TasteSelectProps {
>   value: number | undefined;
>   onChange: (value: number | undefined) => void;
>   label?: string;               // Default: "Taste"
>   error?: string;
>   disabled?: boolean;
>   allowNone?: boolean;          // If true, allows selecting "None"
> }
> ```
> 
> **Component**:
> - Import TasteType and TasteTypeLabels from product.types.ts
> - Create options array from TasteTypeLabels
> - Render Select with:
>   - label prop (default: "Taste")
>   - options array
>   - value and onChange
>   - If allowNone is true, set placeholder to "None"
> 
> **Export TasteSelect as default**

### Location: `src/components/products/ColorSelect.tsx`

**Prompt for Copilot:**

> Create ColorSelect component:
> 
> **Props**:
> ```typescript
> interface ColorSelectProps {
>   value: number | undefined;
>   onChange: (value: number | undefined) => void;
>   label?: string;               // Default: "Color"
>   error?: string;
>   disabled?: boolean;
>   allowNone?: boolean;
> }
> ```
> 
> **Component**:
> - Import ColorType and ColorTypeLabels from product.types.ts
> - Create options array from ColorTypeLabels
> - Render Select component
> - If allowNone is true, set placeholder to "None"
> 
> **Export ColorSelect as default**

### Location: `src/components/products/PlugTypeSelect.tsx`

**Prompt for Copilot:**

> Create PlugTypeSelect component:
> 
> **Props**:
> ```typescript
> interface PlugTypeSelectProps {
>   value: number | undefined;
>   onChange: (value: number) => void;
>   label?: string;               // Default: "Plug Type"
>   error?: string;
>   disabled?: boolean;
>   required?: boolean;           // Should be true for devices
> }
> ```
> 
> **Component**:
> - Import DevicePlugType and DevicePlugTypeLabels from product.types.ts
> - Create options array from DevicePlugTypeLabels
> - Render Select component with required prop
> 
> **Export PlugTypeSelect as default**

### Location: `src/components/products/BatterySizeSelect.tsx`

**Prompt for Copilot:**

> Create BatterySizeSelect component:
> 
> **Props**:
> ```typescript
> interface BatterySizeSelectProps {
>   value: number | undefined;
>   onChange: (value: number | undefined) => void;
>   label?: string;               // Default: "Battery Size"
>   error?: string;
>   disabled?: boolean;
>   allowNone?: boolean;
> }
> ```
> 
> **Component**:
> - Import BatterySize and BatterySizeLabels from product.types.ts
> - Create options array from BatterySizeLabels
> - Render Select component
> - If allowNone is true, set placeholder to "None"
> 
> **Export BatterySizeSelect as default**

---

## Task 8: Update Navigation/Layout

### Location: `src/components/layout/Layout.tsx`

**Prompt for Copilot:**

> Update Layout component to add Products navigation:
> 
> **Add navigation links** in the sidebar or header navigation:
> - Dashboard (existing)
> - Users (existing)
> - **Products (NEW)** - link to /products
> - **Stock History (NEW)** - link to /stock-history
> 
> **Use NavLink** from react-router-dom for active state styling:
> ```typescript
> import { NavLink } from 'react-router-dom';
> ```
> 
> **Active state styling**:
> - Active: 'bg-primary-700 text-white'
> - Inactive: 'text-primary-100 hover:bg-primary-600 hover:text-white'
> - Base: 'px-4 py-2 rounded-md transition-colors'
> 
> **Keep existing**:
> - Header with user info and logout button
> - Responsive mobile menu if exists
> 
> **Icons** (if using Heroicons):
> - Products: CubeIcon or ShoppingBagIcon
> - Stock History: ClockIcon or DocumentTextIcon

---

## Task 9: Update App.tsx Routes

### Location: `src/App.tsx`

**Prompt for Copilot:**

> Update App.tsx to add product routes:
> 
> **Add imports**:
> ```typescript
> import ProductsPage from './pages/ProductsPage';
> import ProductDetailPage from './pages/ProductDetailPage';
> import StockHistoryPage from './pages/StockHistoryPage';
> ```
> 
> **Add routes inside ProtectedRoute wrapper** (after existing routes):
> ```typescript
> <Route path="/products" element={<ProductsPage />} />
> <Route path="/products/:id" element={<ProductDetailPage />} />
> <Route path="/stock-history" element={<StockHistoryPage />} />
> ```
> 
> **Keep existing routes**:
> - /login → LoginPage
> - /dashboard → DashboardPage
> - /users → UsersPage
> - / → Navigate to /dashboard

---

## Verification Checklist

Before moving to Step 5B, verify:

- [ ] Folder structure created (src/components/products)
- [ ] product.types.ts created with all 5 enums and interfaces
- [ ] Enum label mappings created (ProductTypeLabels, TasteTypeLabels, etc.)
- [ ] apiConfig.ts updated with PRODUCTS and STOCK endpoints
- [ ] formatters.ts created with all utility functions
- [ ] productService.ts created with all CRUD methods
- [ ] stockService.ts created with stock management methods
- [ ] Select.tsx component created
- [ ] Textarea.tsx component created
- [ ] All 5 enum select components created:
  - [ ] ProductTypeSelect.tsx
  - [ ] TasteSelect.tsx
  - [ ] ColorSelect.tsx
  - [ ] PlugTypeSelect.tsx
  - [ ] BatterySizeSelect.tsx
- [ ] Layout.tsx updated with Products and Stock History navigation
- [ ] App.tsx routes added for products pages
- [ ] No TypeScript errors
- [ ] App compiles successfully (`npm run dev` works)

---

## What You've Accomplished

✅ **Complete TypeScript type system** for all 5 product types  
✅ **Enum definitions** matching backend exactly (ProductType, TasteType, ColorType, DevicePlugType, BatterySize)  
✅ **Label mappings** for UI display  
✅ **API configuration** for all product and stock endpoints  
✅ **Formatting utilities** for currency, dates, stock changes, and labels  
✅ **Product service** with CRUD operations for all product types  
✅ **Stock service** for add/remove stock and history  
✅ **Reusable components** (Select, Textarea)  
✅ **Enum-specific selectors** for all dropdowns  
✅ **Navigation and routing** configured  

---

## Next Steps

Ready for **Step 5B: Product List & CRUD UI** where you'll create:
- Products page with table and optional grid view
- Product filters (by type, search)
- Create product forms (one for each product type)
- Edit product forms
- Delete confirmation dialog
- Stock management modals (add/remove stock)

---

## Tips

1. **Test types first** - Open product.types.ts and verify no TypeScript errors
2. **Test enum values** - Console.log ProductType, TasteType values to verify they match backend (1, 2, 3, 4, 5)
3. **Verify API calls** - Test productService.getAllProducts() in browser console
4. **Verify formatters** - Test formatCurrency(1500, 'ALL') returns '1,500 ALL'
5. **Reuse components** - Button, Input, Modal components from User Management still work
6. **Consistent styling** - Follow same TailwindCSS patterns as Users page

**Important Notes:**
- Product types are numbered 1-5 (not 0-4)
- TasteType only applies to AromaBombel and AromaBottle
- ColorType applies to AromaDevice and SanitizingDevice
- DevicePlugType is REQUIRED for AromaDevice and SanitizingDevice
- BatterySize only applies to Battery products
- AromaDevice has squareMeter field, SanitizingDevice does NOT

Good luck! 🚀
