# Step 5B: React UI - Product List & CRUD
## Instructions for GitHub Copilot

---

## Overview
This guide provides instructions for building the complete Product Management UI with CRUD operations for all 5 product types. You'll create the products page, product forms, filters, and all necessary components.

**Prerequisites**: Complete Step 5A (Setup - Types, Services, Components)

**Time estimate**: 4-5 hours

---

## Product Types Reminder

We have **5 product types**, each with different fields:

| Type | ID | Unique Fields |
|------|----|--------------| 
| **AromaBombel** | 1 | tasteId (optional) |
| **AromaBottle** | 2 | tasteId (optional) |
| **AromaDevice** | 3 | colorId, format, programs, plugTypeId (required), squareMeter |
| **SanitizingDevice** | 4 | colorId, format, programs, plugTypeId (required) |
| **Battery** | 5 | type, sizeId, brand |

**Common fields**: name, description, price, currency, photoUrl, stockQuantity, isActive

---

## Task 1: Create Product Card Component

### Location: `src/components/products/ProductCard.tsx`

**Prompt for Copilot:**

> Create a ProductCard component to display a single product in grid view:
> 
> **Props interface**:
> ```typescript
> interface ProductCardProps {
>   product: ProductDto;
>   onEdit: (product: ProductDto) => void;
>   onDelete: (product: ProductDto) => void;
>   onManageStock: (product: ProductDto) => void;
> }
> ```
> 
> **Component structure**:
> - Card container with hover effect
> - Product image section (or placeholder if no photoUrl)
> - Product info section:
>   - Product name (bold, text-lg)
>   - Product type badge (using getProductTypeLabel)
>   - Taste badge (if tasteId exists, using getTasteTypeLabel)
>   - Color badge (if colorId exists, using getColorTypeLabel)
>   - Price (formatted with formatCurrency)
>   - Stock badge (using StockBadge component from Step 5A if created, or create inline)
>   - Description (truncated to 100 characters with "..." if longer)
> - Action buttons section:
>   - Edit button (blue, with PencilIcon)
>   - Delete button (red, with TrashIcon)
>   - Manage Stock button (purple, with ChartBarIcon)
> 
> **Styling (TailwindCSS)**:
> - Card: `bg-white rounded-lg shadow hover:shadow-xl transition-shadow cursor-pointer border border-gray-200 overflow-hidden`
> - Image section: `h-48 w-full bg-gray-200 flex items-center justify-center`
> - Image (if exists): `h-full w-full object-cover`
> - Image placeholder: `text-gray-400` with CubeIcon
> - Content: `p-4 space-y-3`
> - Name: `text-lg font-bold text-gray-900`
> - Badges row: `flex flex-wrap gap-2 items-center`
> - Badge base: `px-2 py-1 rounded-full text-xs font-medium`
> - Type badge: `bg-blue-100 text-blue-800`
> - Taste badge: `bg-green-100 text-green-800`
> - Color badge: `bg-purple-100 text-purple-800`
> - Price: `text-xl font-bold text-primary-600`
> - Description: `text-sm text-gray-600`
> - Actions: `flex gap-2 mt-4 pt-4 border-t border-gray-200`
> - Button base: `flex-1 px-3 py-2 rounded-lg font-medium transition-colors text-sm`
> - Edit: `bg-blue-500 hover:bg-blue-600 text-white`
> - Delete: `bg-red-500 hover:bg-red-600 text-white`
> - Manage Stock: `bg-purple-500 hover:bg-purple-600 text-white`
> 
> **Stock Badge inline** (if not created in 5A):
> ```typescript
> const stockBadge = product.stockQuantity === 0 
>   ? { color: 'bg-red-100 text-red-800', label: 'Out of Stock' }
>   : product.isLowStock 
>   ? { color: 'bg-yellow-100 text-yellow-800', label: `Low Stock (${product.stockQuantity})` }
>   : { color: 'bg-green-100 text-green-800', label: `In Stock (${product.stockQuantity})` };
> ```
> 
> **Import**:
> - ProductDto from types
> - formatCurrency, getProductTypeLabel, getTasteTypeLabel, getColorTypeLabel from formatters
> - Icons from '@heroicons/react/24/outline': PencilIcon, TrashIcon, ChartBarIcon, CubeIcon
> 
> **Export ProductCard as default**

---

## Task 2: Create Product Table Component

### Location: `src/components/products/ProductTable.tsx`

**Prompt for Copilot:**

> Create a ProductTable component to display products in table view:
> 
> **Props interface**:
> ```typescript
> interface ProductTableProps {
>   products: ProductDto[];
>   onEdit: (product: ProductDto) => void;
>   onDelete: (product: ProductDto) => void;
>   onManageStock: (product: ProductDto) => void;
>   loading?: boolean;
> }
> ```
> 
> **Component structure**:
> - Responsive table wrapper (horizontal scroll on mobile)
> - Table with columns:
>   - Image (thumbnail, 60x60px)
>   - Name
>   - Type (badge)
>   - Taste/Color (badge, if applicable)
>   - Price
>   - Stock (badge with quantity)
>   - Actions (Edit, Delete, Manage Stock icon buttons)
> - Loading state (show spinner)
> - Empty state (if no products)
> 
> **Styling (TailwindCSS)**:
> - Wrapper: `overflow-x-auto`
> - Table: `min-w-full divide-y divide-gray-200`
> - Header: `bg-gray-50`
> - Header cell: `px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider`
> - Body: `bg-white divide-y divide-gray-200`
> - Body row: `hover:bg-gray-50 transition-colors`
> - Body cell: `px-6 py-4 whitespace-nowrap`
> - Image: `h-12 w-12 rounded-lg object-cover`
> - Action buttons: icon buttons with hover effects
>   - Edit: `text-blue-600 hover:text-blue-900`
>   - Delete: `text-red-600 hover:text-red-900`
>   - Manage: `text-purple-600 hover:text-purple-900`
> 
> **Empty state**:
> ```typescript
> {products.length === 0 && !loading && (
>   <tr>
>     <td colSpan={7} className="px-6 py-12 text-center">
>       <CubeIcon className="mx-auto h-12 w-12 text-gray-400" />
>       <p className="mt-2 text-sm text-gray-500">No products found</p>
>     </td>
>   </tr>
> )}
> ```
> 
> **Import**:
> - ProductDto from types
> - formatCurrency, getProductTypeLabel, getTasteTypeLabel, getColorTypeLabel from formatters
> - Icons from '@heroicons/react/24/outline'
> 
> **Export ProductTable as default**

---

## Task 3: Create Product Filters Component

### Location: `src/components/products/ProductFilters.tsx`

**Prompt for Copilot:**

> Create a ProductFilters component for filtering and searching products:
> 
> **Props interface**:
> ```typescript
> interface ProductFiltersProps {
>   searchTerm: string;
>   onSearchChange: (value: string) => void;
>   filterType: number;  // 0 = All, 1-5 = specific types
>   onTypeChange: (typeId: number) => void;
>   viewMode: 'grid' | 'table';
>   onViewModeChange: (mode: 'grid' | 'table') => void;
> }
> ```
> 
> **Component structure**:
> - Horizontal flex container with filters
> - Search input (with MagnifyingGlassIcon)
> - ProductTypeSelect with showAll={true}
> - View mode toggle buttons (grid/table icons)
> 
> **Layout**:
> ```typescript
> <div className="flex flex-col sm:flex-row gap-4 items-stretch sm:items-center">
>   {/* Search input - flex-1 */}
>   <div className="flex-1">
>     <Input
>       type="text"
>       placeholder="Search products by name or description..."
>       value={searchTerm}
>       onChange={(e) => onSearchChange(e.target.value)}
>     />
>   </div>
>   
>   {/* Product type filter */}
>   <div className="w-full sm:w-64">
>     <ProductTypeSelect
>       value={filterType}
>       onChange={onTypeChange}
>       showAll={true}
>     />
>   </div>
>   
>   {/* View mode toggle */}
>   <div className="flex gap-2">
>     <button
>       onClick={() => onViewModeChange('grid')}
>       className={`p-2 rounded-lg ${viewMode === 'grid' ? 'bg-primary-100 text-primary-700' : 'bg-gray-100 text-gray-600 hover:bg-gray-200'}`}
>     >
>       <Squares2X2Icon className="h-6 w-6" />
>     </button>
>     <button
>       onClick={() => onViewModeChange('table')}
>       className={`p-2 rounded-lg ${viewMode === 'table' ? 'bg-primary-100 text-primary-700' : 'bg-gray-100 text-gray-600 hover:bg-gray-200'}`}
>     >
>       <TableCellsIcon className="h-6 w-6" />
>     </button>
>   </div>
> </div>
> ```
> 
> **Import**:
> - Input from common components (or create inline if needed)
> - ProductTypeSelect from components/products
> - Icons from '@heroicons/react/24/outline': MagnifyingGlassIcon, Squares2X2Icon, TableCellsIcon
> 
> **Export ProductFilters as default**

---

## Task 4: Create Product Stats Component

### Location: `src/components/products/ProductStats.tsx`

**Prompt for Copilot:**

> Create a ProductStats component to display product statistics:
> 
> **Props interface**:
> ```typescript
> interface ProductStatsProps {
>   products: ProductDto[];
> }
> ```
> 
> **Component logic**:
> Calculate statistics:
> - Total products count
> - Count by each type (AromaBombel, AromaBottle, AromaDevice, SanitizingDevice, Battery)
> - Total inventory value (sum of all product prices × stock quantities)
> - Low stock count (products with isLowStock = true)
> 
> **Component structure**:
> Grid of stat cards (4 columns on desktop, 2 on tablet, 1 on mobile)
> 
> Each card shows:
> - Icon with colored background
> - Label
> - Value (large, bold)
> - Optional subtext
> 
> **Stats to display**:
> 1. **Total Products**: count of all products, blue, CubeIcon
> 2. **Aroma Bombels**: count where productTypeId === 1, green, BeakerIcon
> 3. **Aroma Bottles**: count where productTypeId === 2, cyan, BeakerIcon
> 4. **Aroma Devices**: count where productTypeId === 3, purple, CpuChipIcon
> 5. **Sanitizing Devices**: count where productTypeId === 4, orange, SparklesIcon
> 6. **Batteries**: count where productTypeId === 5, yellow, BoltIcon
> 7. **Total Value**: sum of (price × stockQuantity), indigo, CurrencyDollarIcon
> 8. **Low Stock**: count where isLowStock === true, red, ExclamationTriangleIcon
> 
> **Styling**:
> - Grid: `grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 mb-6`
> - Card: `bg-white rounded-lg shadow p-6 flex items-center gap-4`
> - Icon container: `p-3 rounded-full` with background color
> - Icon: `h-6 w-6 text-white`
> - Content: `flex-1`
> - Label: `text-sm font-medium text-gray-600`
> - Value: `text-2xl font-bold text-gray-900`
> - Subtext: `text-xs text-gray-500 mt-1`
> 
> **Example card**:
> ```typescript
> <div className="bg-white rounded-lg shadow p-6 flex items-center gap-4">
>   <div className="p-3 rounded-full bg-blue-500">
>     <CubeIcon className="h-6 w-6 text-white" />
>   </div>
>   <div className="flex-1">
>     <p className="text-sm font-medium text-gray-600">Total Products</p>
>     <p className="text-2xl font-bold text-gray-900">{totalCount}</p>
>   </div>
> </div>
> ```
> 
> **Import**:
> - ProductDto from types
> - formatCurrency from formatters
> - Icons from '@heroicons/react/24/outline'
> 
> **Export ProductStats as default**

---

## Task 5: Create Product Type Selection Modal

### Location: `src/components/products/ProductTypeModal.tsx`

**Prompt for Copilot:**

> Create a ProductTypeModal for selecting product type before creation:
> 
> **Props interface**:
> ```typescript
> interface ProductTypeModalProps {
>   isOpen: boolean;
>   onClose: () => void;
>   onSelectType: (typeId: number) => void;
> }
> ```
> 
> **Component structure**:
> - Modal with title "Select Product Type"
> - Grid of 5 product type cards (2 columns on desktop, 1 on mobile)
> - Each card shows:
>   - Icon
>   - Type name
>   - Short description
>   - Clickable to select
> 
> **Product type cards data**:
> ```typescript
> const productTypes = [
>   {
>     id: 1,
>     name: 'Aroma Bombel',
>     description: 'Aroma bombs with various tastes for small spaces',
>     icon: BeakerIcon,
>     color: 'bg-green-500',
>   },
>   {
>     id: 2,
>     name: 'Aroma Bottle',
>     description: 'Aroma bottles with liquid refills',
>     icon: BeakerIcon,
>     color: 'bg-cyan-500',
>   },
>   {
>     id: 3,
>     name: 'Aroma Device',
>     description: 'Electronic aroma devices for medium to large spaces',
>     icon: CpuChipIcon,
>     color: 'bg-purple-500',
>   },
>   {
>     id: 4,
>     name: 'Sanitizing Device',
>     description: 'Hygienic sanitizing devices for cleaning',
>     icon: SparklesIcon,
>     color: 'bg-orange-500',
>   },
>   {
>     id: 5,
>     name: 'Battery',
>     description: 'Replacement batteries (LR6/AA and LR9/AAA)',
>     icon: BoltIcon,
>     color: 'bg-yellow-500',
>   },
> ];
> ```
> 
> **Styling**:
> - Modal: use Modal component from common (or create inline)
> - Grid: `grid grid-cols-1 md:grid-cols-2 gap-4 mt-4`
> - Card: `border-2 border-gray-200 rounded-lg p-6 hover:border-primary-500 hover:bg-primary-50 cursor-pointer transition-all`
> - Active/hover: `border-primary-500 bg-primary-50`
> - Icon container: `h-12 w-12 rounded-full flex items-center justify-center mb-4` with color
> - Icon: `h-6 w-6 text-white`
> - Name: `text-lg font-bold text-gray-900 mb-2`
> - Description: `text-sm text-gray-600`
> 
> **On card click**:
> ```typescript
> onClick={() => {
>   onSelectType(type.id);
>   onClose();
> }}
> ```
> 
> **Import**:
> - Modal from common components
> - Icons from '@heroicons/react/24/outline'
> 
> **Export ProductTypeModal as default**

---

## Task 6: Create Product Forms (5 forms, one for each type)

### Location: `src/components/products/AromaBombelForm.tsx`

**Prompt for Copilot:**

> Create AromaBombelForm component for creating/editing Aroma Bombel products:
> 
> **Props interface**:
> ```typescript
> interface AromaBombelFormProps {
>   isOpen: boolean;
>   onClose: () => void;
>   product?: AromaBombelProductDto;  // undefined for create, defined for edit
>   onSubmit: (data: CreateAromaBombelRequest | UpdateAromaBombelRequest) => Promise<void>;
> }
> ```
> 
> **Form state** (use useState):
> ```typescript
> const [formData, setFormData] = useState({
>   name: product?.name || '',
>   description: product?.description || '',
>   price: product?.price || 0,
>   currency: product?.currency || 'ALL',
>   photoUrl: product?.photoUrl || '',
>   tasteId: product?.tasteId || undefined,
> });
> const [errors, setErrors] = useState<Record<string, string>>({});
> const [loading, setLoading] = useState(false);
> ```
> 
> **Form fields** (2-column grid on desktop):
> - **Row 1**: Name (required), Price (required, number, min 0)
> - **Row 2**: Currency (select: ALL, EUR, USD - default ALL), TasteSelect (optional)
> - **Row 3**: Photo URL (optional, full width)
> - **Row 4**: Description (textarea, optional, full width, rows 3)
> 
> **Validation**:
> - Name: required, min 2 characters
> - Price: required, must be positive number
> - Currency: required
> 
> **Handle submit**:
> ```typescript
> const handleSubmit = async (e: React.FormEvent) => {
>   e.preventDefault();
>   
>   // Validate
>   const newErrors: Record<string, string> = {};
>   if (!formData.name || formData.name.length < 2) {
>     newErrors.name = 'Name must be at least 2 characters';
>   }
>   if (!formData.price || formData.price <= 0) {
>     newErrors.price = 'Price must be greater than 0';
>   }
>   if (!formData.currency) {
>     newErrors.currency = 'Currency is required';
>   }
>   
>   if (Object.keys(newErrors).length > 0) {
>     setErrors(newErrors);
>     return;
>   }
>   
>   setLoading(true);
>   try {
>     await onSubmit(formData);
>     toast.success(product ? 'Product updated successfully' : 'Product created successfully');
>     onClose();
>   } catch (error) {
>     toast.error(error.message || 'Failed to save product');
>   } finally {
>     setLoading(false);
>   }
> };
> ```
> 
> **Modal structure**:
> - Title: "Create Aroma Bombel" or "Edit Aroma Bombel"
> - Form with 2-column grid
> - Footer with Cancel and Save buttons
> 
> **Styling**:
> - Modal: max-w-2xl
> - Form grid: `grid grid-cols-1 md:grid-cols-2 gap-4`
> - Full width fields: `md:col-span-2`
> - Buttons: Cancel (gray), Save (primary, with loading spinner)
> 
> **Import**:
> - Modal, Input, Textarea, Button from common
> - TasteSelect from components/products
> - Select from common (for currency)
> - toast from react-hot-toast
> - Types from product.types.ts
> 
> **Export AromaBombelForm as default**

---

### Location: `src/components/products/AromaBottleForm.tsx`

**Prompt for Copilot:**

> Create AromaBottleForm component - **IDENTICAL to AromaBombelForm** except:
> - Component name: AromaBottleForm
> - Props interface uses AromaBottleProductDto
> - Modal titles: "Create Aroma Bottle" / "Edit Aroma Bottle"
> - Toast messages: "Aroma Bottle created/updated"
> 
> All fields and logic are the same as AromaBombel (name, description, price, currency, photoUrl, tasteId)
> 
> **Export AromaBottleForm as default**

---

### Location: `src/components/products/AromaDeviceForm.tsx`

**Prompt for Copilot:**

> Create AromaDeviceForm component for Aroma Device products:
> 
> **Props interface**:
> ```typescript
> interface AromaDeviceFormProps {
>   isOpen: boolean;
>   onClose: () => void;
>   product?: AromaDeviceProductDto;
>   onSubmit: (data: CreateAromaDeviceRequest | UpdateAromaDeviceRequest) => Promise<void>;
> }
> ```
> 
> **Form state**:
> ```typescript
> const [formData, setFormData] = useState({
>   name: product?.name || '',
>   description: product?.description || '',
>   price: product?.price || 0,
>   currency: product?.currency || 'ALL',
>   photoUrl: product?.photoUrl || '',
>   colorId: product?.colorId || undefined,
>   format: product?.format || '',
>   programs: product?.programs || '',
>   plugTypeId: product?.plugTypeId || undefined,  // REQUIRED
>   squareMeter: product?.squareMeter || undefined,
> });
> ```
> 
> **Form fields** (2-column grid):
> - **Row 1**: Name (required), Price (required)
> - **Row 2**: Currency (select), ColorSelect (optional)
> - **Row 3**: PlugTypeSelect (REQUIRED), Square Meter (number, optional)
> - **Row 4**: Photo URL (optional, full width)
> - **Row 5**: Format (textarea, optional, full width, rows 2)
> - **Row 6**: Programs (textarea, optional, full width, rows 2)
> - **Row 7**: Description (textarea, optional, full width, rows 3)
> 
> **Validation**:
> - Name: required
> - Price: required, positive
> - Currency: required
> - **plugTypeId: REQUIRED** (show error if not selected)
> - squareMeter: if provided, must be positive
> 
> **Modal title**: "Create Aroma Device" / "Edit Aroma Device"
> 
> **Import**:
> - ColorSelect, PlugTypeSelect from components/products
> - Other imports same as AromaBombel
> 
> **Export AromaDeviceForm as default**

---

### Location: `src/components/products/SanitizingDeviceForm.tsx`

**Prompt for Copilot:**

> Create SanitizingDeviceForm component for Sanitizing Device products:
> 
> **Props interface**:
> ```typescript
> interface SanitizingDeviceFormProps {
>   isOpen: boolean;
>   onClose: () => void;
>   product?: SanitizingDeviceProductDto;
>   onSubmit: (data: CreateSanitizingDeviceRequest | UpdateSanitizingDeviceRequest) => Promise<void>;
> }
> ```
> 
> **Form state**:
> ```typescript
> const [formData, setFormData] = useState({
>   name: product?.name || '',
>   description: product?.description || '',
>   price: product?.price || 0,
>   currency: product?.currency || 'ALL',
>   photoUrl: product?.photoUrl || '',
>   colorId: product?.colorId || undefined,
>   format: product?.format || '',
>   programs: product?.programs || '',
>   plugTypeId: product?.plugTypeId || undefined,  // REQUIRED
>   // NO squareMeter for Sanitizing Device
> });
> ```
> 
> **Form fields** (2-column grid):
> - **Row 1**: Name (required), Price (required)
> - **Row 2**: Currency (select), ColorSelect (optional)
> - **Row 3**: PlugTypeSelect (REQUIRED), Photo URL
> - **Row 4**: Format (textarea, optional, full width, rows 2)
> - **Row 5**: Programs (textarea, optional, full width, rows 2)
> - **Row 6**: Description (textarea, optional, full width, rows 3)
> 
> **Validation**:
> - Name: required
> - Price: required, positive
> - Currency: required
> - **plugTypeId: REQUIRED**
> 
> **IMPORTANT**: This form does NOT have squareMeter field (unlike AromaDevice)
> 
> **Modal title**: "Create Sanitizing Device" / "Edit Sanitizing Device"
> 
> **Export SanitizingDeviceForm as default**

---

### Location: `src/components/products/BatteryForm.tsx`

**Prompt for Copilot:**

> Create BatteryForm component for Battery products:
> 
> **Props interface**:
> ```typescript
> interface BatteryFormProps {
>   isOpen: boolean;
>   onClose: () => void;
>   product?: BatteryProductDto;
>   onSubmit: (data: CreateBatteryRequest | UpdateBatteryRequest) => Promise<void>;
> }
> ```
> 
> **Form state**:
> ```typescript
> const [formData, setFormData] = useState({
>   name: product?.name || '',
>   description: product?.description || '',
>   price: product?.price || 0,
>   currency: product?.currency || 'ALL',
>   photoUrl: product?.photoUrl || '',
>   type: product?.type || '',           // Battery type (e.g., "Lithium Ion")
>   sizeId: product?.sizeId || undefined,  // LR6 (1) or LR9 (2)
>   brand: product?.brand || '',
> });
> ```
> 
> **Form fields** (2-column grid):
> - **Row 1**: Name (required), Price (required)
> - **Row 2**: Currency (select), Battery Type (input text, optional)
> - **Row 3**: BatterySizeSelect (optional), Brand (input text, optional)
> - **Row 4**: Photo URL (optional, full width)
> - **Row 5**: Description (textarea, optional, full width, rows 3)
> 
> **Validation**:
> - Name: required
> - Price: required, positive
> - Currency: required
> 
> **Modal title**: "Create Battery" / "Edit Battery"
> 
> **Import**:
> - BatterySizeSelect from components/products
> - Other imports same as previous forms
> 
> **Export BatteryForm as default**

---

## Task 7: Create Products Page (Main)

### Location: `src/pages/ProductsPage.tsx`

**Prompt for Copilot:**

> Create the main ProductsPage component with complete product management:
> 
> **Component state**:
> ```typescript
> const [products, setProducts] = useState<ProductDto[]>([]);
> const [filteredProducts, setFilteredProducts] = useState<ProductDto[]>([]);
> const [loading, setLoading] = useState(true);
> const [searchTerm, setSearchTerm] = useState('');
> const [filterType, setFilterType] = useState(0); // 0 = All
> const [viewMode, setViewMode] = useState<'grid' | 'table'>('grid');
> 
> // Modal states
> const [showTypeModal, setShowTypeModal] = useState(false);
> const [showFormModal, setShowFormModal] = useState(false);
> const [showDeleteDialog, setShowDeleteDialog] = useState(false);
> 
> // Selected items
> const [selectedProduct, setSelectedProduct] = useState<ProductDto | null>(null);
> const [selectedProductType, setSelectedProductType] = useState<number | null>(null);
> ```
> 
> **useEffect - Load products on mount**:
> ```typescript
> useEffect(() => {
>   loadProducts();
> }, []);
> ```
> 
> **useEffect - Filter products when search/filter changes**:
> ```typescript
> useEffect(() => {
>   let filtered = products;
>   
>   // Filter by type
>   if (filterType > 0) {
>     filtered = filtered.filter(p => p.productTypeId === filterType);
>   }
>   
>   // Filter by search term
>   if (searchTerm) {
>     const term = searchTerm.toLowerCase();
>     filtered = filtered.filter(p => 
>       p.name.toLowerCase().includes(term) ||
>       (p.description && p.description.toLowerCase().includes(term))
>     );
>   }
>   
>   setFilteredProducts(filtered);
> }, [products, filterType, searchTerm]);
> ```
> 
> **Functions**:
> ```typescript
> const loadProducts = async () => {
>   setLoading(true);
>   try {
>     const data = await productService.getAllProducts();
>     setProducts(data);
>   } catch (error) {
>     toast.error('Failed to load products');
>   } finally {
>     setLoading(false);
>   }
> };
> 
> const handleAddClick = () => {
>   setSelectedProduct(null);
>   setSelectedProductType(null);
>   setShowTypeModal(true);
> };
> 
> const handleSelectType = (typeId: number) => {
>   setSelectedProductType(typeId);
>   setShowTypeModal(false);
>   setShowFormModal(true);
> };
> 
> const handleEdit = (product: ProductDto) => {
>   setSelectedProduct(product);
>   setSelectedProductType(product.productTypeId);
>   setShowFormModal(true);
> };
> 
> const handleDelete = (product: ProductDto) => {
>   setSelectedProduct(product);
>   setShowDeleteDialog(true);
> };
> 
> const confirmDelete = async () => {
>   if (!selectedProduct) return;
>   
>   try {
>     await productService.deleteProduct(selectedProduct.id);
>     toast.success('Product deleted successfully');
>     loadProducts();
>     setShowDeleteDialog(false);
>   } catch (error) {
>     toast.error('Failed to delete product');
>   }
> };
> 
> const handleFormSubmit = async (data: any) => {
>   if (!selectedProductType) return;
>   
>   try {
>     if (selectedProduct) {
>       // Update
>       await productService.updateProduct(selectedProductType, selectedProduct.id, data);
>     } else {
>       // Create
>       await productService.createProduct(selectedProductType, data);
>     }
>     loadProducts();
>     setShowFormModal(false);
>   } catch (error) {
>     throw error; // Let form handle error
>   }
> };
> 
> const handleManageStock = (product: ProductDto) => {
>   // TODO: Implement in Step 5C
>   toast.info('Stock management coming in Step 5C');
> };
> ```
> 
> **Page structure**:
> ```typescript
> return (
>   <div className="p-6">
>     {/* Header */}
>     <div className="flex justify-between items-center mb-6">
>       <div>
>         <h1 className="text-3xl font-bold text-gray-900">Products</h1>
>         <p className="text-gray-600 mt-1">Manage your inventory products</p>
>       </div>
>       <Button onClick={handleAddClick}>
>         <PlusIcon className="h-5 w-5 mr-2" />
>         Add Product
>       </Button>
>     </div>
> 
>     {/* Stats */}
>     <ProductStats products={products} />
> 
>     {/* Filters */}
>     <ProductFilters
>       searchTerm={searchTerm}
>       onSearchChange={setSearchTerm}
>       filterType={filterType}
>       onTypeChange={setFilterType}
>       viewMode={viewMode}
>       onViewModeChange={setViewMode}
>     />
> 
>     {/* Loading */}
>     {loading && (
>       <div className="flex justify-center py-12">
>         <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary-600"></div>
>       </div>
>     )}
> 
>     {/* Empty State */}
>     {!loading && filteredProducts.length === 0 && (
>       <div className="text-center py-12">
>         <CubeIcon className="mx-auto h-12 w-12 text-gray-400" />
>         <h3 className="mt-2 text-sm font-medium text-gray-900">No products found</h3>
>         <p className="mt-1 text-sm text-gray-500">Get started by creating a new product.</p>
>         <div className="mt-6">
>           <Button onClick={handleAddClick}>
>             <PlusIcon className="h-5 w-5 mr-2" />
>             Add Product
>           </Button>
>         </div>
>       </div>
>     )}
> 
>     {/* Grid View */}
>     {!loading && viewMode === 'grid' && filteredProducts.length > 0 && (
>       <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6 mt-6">
>         {filteredProducts.map(product => (
>           <ProductCard
>             key={product.id}
>             product={product}
>             onEdit={handleEdit}
>             onDelete={handleDelete}
>             onManageStock={handleManageStock}
>           />
>         ))}
>       </div>
>     )}
> 
>     {/* Table View */}
>     {!loading && viewMode === 'table' && filteredProducts.length > 0 && (
>       <div className="mt-6">
>         <ProductTable
>           products={filteredProducts}
>           onEdit={handleEdit}
>           onDelete={handleDelete}
>           onManageStock={handleManageStock}
>         />
>       </div>
>     )}
> 
>     {/* Modals */}
>     <ProductTypeModal
>       isOpen={showTypeModal}
>       onClose={() => setShowTypeModal(false)}
>       onSelectType={handleSelectType}
>     />
> 
>     {/* Type-specific form modals */}
>     {selectedProductType === 1 && (
>       <AromaBombelForm
>         isOpen={showFormModal}
>         onClose={() => setShowFormModal(false)}
>         product={selectedProduct as AromaBombelProductDto}
>         onSubmit={handleFormSubmit}
>       />
>     )}
>     {selectedProductType === 2 && (
>       <AromaBottleForm
>         isOpen={showFormModal}
>         onClose={() => setShowFormModal(false)}
>         product={selectedProduct as AromaBottleProductDto}
>         onSubmit={handleFormSubmit}
>       />
>     )}
>     {selectedProductType === 3 && (
>       <AromaDeviceForm
>         isOpen={showFormModal}
>         onClose={() => setShowFormModal(false)}
>         product={selectedProduct as AromaDeviceProductDto}
>         onSubmit={handleFormSubmit}
>       />
>     )}
>     {selectedProductType === 4 && (
>       <SanitizingDeviceForm
>         isOpen={showFormModal}
>         onClose={() => setShowFormModal(false)}
>         product={selectedProduct as SanitizingDeviceProductDto}
>         onSubmit={handleFormSubmit}
>       />
>     )}
>     {selectedProductType === 5 && (
>       <BatteryForm
>         isOpen={showFormModal}
>         onClose={() => setShowFormModal(false)}
>         product={selectedProduct as BatteryProductDto}
>         onSubmit={handleFormSubmit}
>       />
>     )}
> 
>     {/* Delete Confirmation */}
>     <ConfirmationDialog
>       isOpen={showDeleteDialog}
>       onClose={() => setShowDeleteDialog(false)}
>       onConfirm={confirmDelete}
>       title="Delete Product"
>       message={`Are you sure you want to delete "${selectedProduct?.name}"? This action cannot be undone.`}
>       confirmLabel="Delete"
>       confirmVariant="danger"
>     />
>   </div>
> );
> ```
> 
> **Import**:
> - All components created above
> - All form components (5 forms)
> - productService
> - toast from react-hot-toast
> - All types
> - Icons from '@heroicons/react/24/outline': PlusIcon, CubeIcon
> - Button, ConfirmationDialog from common
> 
> **Export ProductsPage as default**

---

## Task 8: Create Product Detail Page (Placeholder)

### Location: `src/pages/ProductDetailPage.tsx`

**Prompt for Copilot:**

> Create a placeholder ProductDetailPage component:
> 
> ```typescript
> import { useParams, useNavigate } from 'react-router-dom';
> import { Button } from '../components/common/Button';
> import { ArrowLeftIcon } from '@heroicons/react/24/outline';
> 
> export default function ProductDetailPage() {
>   const { id } = useParams<{ id: string }>();
>   const navigate = useNavigate();
> 
>   return (
>     <div className="p-6">
>       <Button 
>         variant="secondary" 
>         onClick={() => navigate('/products')}
>         className="mb-6"
>       >
>         <ArrowLeftIcon className="h-5 w-5 mr-2" />
>         Back to Products
>       </Button>
> 
>       <div className="bg-white rounded-lg shadow p-8 text-center">
>         <h1 className="text-2xl font-bold text-gray-900 mb-4">
>           Product Detail Page
>         </h1>
>         <p className="text-gray-600 mb-2">
>           Product ID: {id}
>         </p>
>         <p className="text-sm text-gray-500">
>           This page will show full product details and stock history in a future update.
>         </p>
>       </div>
>     </div>
>   );
> }
> ```
> 
> This is just a placeholder. Full implementation can be added later.
> 
> **Export ProductDetailPage as default**

---

## Verification Checklist

Before moving to Step 5C, verify all functionality works:

### Components:
- [ ] ProductCard displays correctly with all badges
- [ ] ProductTable shows all columns properly
- [ ] ProductFilters search works
- [ ] ProductFilters type filter works
- [ ] ProductFilters view toggle works
- [ ] ProductStats calculates correctly
- [ ] ProductTypeModal displays all 5 types

### Forms:
- [ ] AromaBombelForm creates and edits
- [ ] AromaBottleForm creates and edits
- [ ] AromaDeviceForm creates and edits (with plugTypeId required)
- [ ] SanitizingDeviceForm creates and edits (no squareMeter field)
- [ ] BatteryForm creates and edits
- [ ] All forms validate properly
- [ ] All forms show errors

### Products Page:
- [ ] Products load on mount
- [ ] Can click "Add Product"
- [ ] ProductTypeModal opens
- [ ] Selecting type opens correct form
- [ ] Can create product (all 5 types)
- [ ] Can edit product (all 5 types)
- [ ] Can delete product with confirmation
- [ ] Grid view displays correctly
- [ ] Table view displays correctly
- [ ] View toggle works
- [ ] Search filters products
- [ ] Type filter works
- [ ] "Manage Stock" shows "Coming in Step 5C" toast

### General:
- [ ] No TypeScript errors
- [ ] No console errors
- [ ] All API calls work
- [ ] Toast notifications appear
- [ ] Loading states work
- [ ] Empty states display
- [ ] Responsive on mobile
- [ ] All buttons have proper styling

---

## What You've Accomplished

✅ **Complete Product CRUD UI** for all 5 product types  
✅ **Type-specific forms** with proper validation  
✅ **Beautiful grid and table views**  
✅ **Search and filter functionality**  
✅ **Product statistics dashboard**  
✅ **Type selection modal** before creation  
✅ **Professional UI** with TailwindCSS  
✅ **Responsive design** for all screen sizes  
✅ **Proper error handling** with toast notifications  
✅ **Loading and empty states**  

---

## Next Steps

Ready for **Step 5C: Stock Management UI** where you'll implement:
- Stock badges with color coding
- Add Stock modal
- Remove Stock modal
- Stock Management hub
- Stock History page
- Low stock alerts
- Dashboard integration

---

## Tips for Testing

1. **Test all product types** - Create one of each type (AromaBombel, AromaBottle, AromaDevice, SanitizingDevice, Battery)
2. **Test validation** - Try submitting forms with empty required fields
3. **Test edit** - Edit products of each type
4. **Test delete** - Verify confirmation dialog shows
5. **Test search** - Search by name and description
6. **Test filters** - Filter by each product type
7. **Test views** - Toggle between grid and table
8. **Test responsive** - Check on mobile screen sizes
9. **Check backend** - Verify data is saved correctly in database
10. **Test plugTypeId** - Ensure it's required for AromaDevice and SanitizingDevice

**Important notes:**
- AromaDevice has `squareMeter` field
- SanitizingDevice does NOT have `squareMeter` field
- plugTypeId is REQUIRED for both device types
- Battery size options are LR6 (AA) and LR9 (AAA)
- All products share common fields (name, description, price, currency, photoUrl)

Good luck! 🚀
