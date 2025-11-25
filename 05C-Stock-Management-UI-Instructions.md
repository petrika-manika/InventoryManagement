# Step 5C: React UI - Stock Management
## Instructions for GitHub Copilot

---

## Overview
This guide provides instructions for building the complete Stock Management UI including stock adjustments, history tracking, and low stock alerts. You'll integrate stock management into the products page and create a dedicated stock history page.

**Prerequisites**: Complete Step 5A (Setup) and Step 5B (Product CRUD)

**Time estimate**: 3-4 hours

---

## Stock Management Features

We'll build:
- **Add Stock** - Increase product stock with reason
- **Remove Stock** - Decrease product stock with validation
- **Stock History** - View all stock changes with filters
- **Low Stock Alerts** - Dashboard warnings for low stock products
- **Stock Badges** - Visual indicators for stock levels

---

## Task 1: Create Stock Badge Component

### Location: `src/components/products/StockBadge.tsx`

**Prompt for Copilot:**

> Create a StockBadge component to display stock levels with color coding:
> 
> **Props interface**:
> ```typescript
> interface StockBadgeProps {
>   quantity: number;
>   isLowStock: boolean;
>   size?: 'sm' | 'md' | 'lg';  // default 'md'
>   showQuantity?: boolean;      // default true
> }
> ```
> 
> **Component logic**:
> Determine badge variant based on stock:
> - quantity === 0: Red badge, "Out of Stock"
> - isLowStock === true (quantity 1-10): Yellow badge, "Low Stock (X)"
> - quantity > 10: Green badge, "In Stock (X)"
> 
> **Styling (TailwindCSS)**:
> - Base: `inline-flex items-center rounded-full font-medium`
> - Size sm: `px-2 py-0.5 text-xs`
> - Size md: `px-2.5 py-1 text-sm`
> - Size lg: `px-3 py-1.5 text-base`
> - Out of Stock: `bg-red-100 text-red-800`
> - Low Stock: `bg-yellow-100 text-yellow-800`
> - In Stock: `bg-green-100 text-green-800`
> 
> **Component**:
> ```typescript
> export default function StockBadge({ 
>   quantity, 
>   isLowStock, 
>   size = 'md',
>   showQuantity = true 
> }: StockBadgeProps) {
>   const sizeClasses = {
>     sm: 'px-2 py-0.5 text-xs',
>     md: 'px-2.5 py-1 text-sm',
>     lg: 'px-3 py-1.5 text-base',
>   };
> 
>   let colorClasses = '';
>   let label = '';
> 
>   if (quantity === 0) {
>     colorClasses = 'bg-red-100 text-red-800';
>     label = 'Out of Stock';
>   } else if (isLowStock) {
>     colorClasses = 'bg-yellow-100 text-yellow-800';
>     label = showQuantity ? `Low Stock (${quantity})` : 'Low Stock';
>   } else {
>     colorClasses = 'bg-green-100 text-green-800';
>     label = showQuantity ? `In Stock (${quantity})` : 'In Stock';
>   }
> 
>   return (
>     <span className={`inline-flex items-center rounded-full font-medium ${sizeClasses[size]} ${colorClasses}`}>
>       {label}
>     </span>
>   );
> }
> ```
> 
> **Export StockBadge as default**

---

## Task 2: Create Add Stock Modal

### Location: `src/components/products/AddStockModal.tsx`

**Prompt for Copilot:**

> Create AddStockModal component for adding stock to products:
> 
> **Props interface**:
> ```typescript
> interface AddStockModalProps {
>   isOpen: boolean;
>   onClose: () => void;
>   product: ProductDto | null;
>   onSuccess: () => void;  // Callback to refresh products
> }
> ```
> 
> **Form state**:
> ```typescript
> const [formData, setFormData] = useState({
>   quantity: 0,
>   reason: '',
> });
> const [errors, setErrors] = useState<Record<string, string>>({});
> const [loading, setLoading] = useState(false);
> ```
> 
> **Component structure**:
> - Modal title: "Add Stock"
> - Product info display:
>   - Product name (bold)
>   - Current stock with StockBadge
> - Form fields:
>   - Quantity (number input, required, min 1, max 1000)
>   - Reason (textarea, optional, rows 3)
> - Stock calculation display:
>   - "Current: X + Adding: Y = New Stock: X+Y"
> - Footer: Cancel and Add Stock buttons
> 
> **Validation**:
> ```typescript
> const validate = () => {
>   const newErrors: Record<string, string> = {};
>   
>   if (!formData.quantity || formData.quantity <= 0) {
>     newErrors.quantity = 'Quantity must be greater than 0';
>   }
>   if (formData.quantity > 1000) {
>     newErrors.quantity = 'Quantity cannot exceed 1000';
>   }
>   
>   setErrors(newErrors);
>   return Object.keys(newErrors).length === 0;
> };
> ```
> 
> **Handle submit**:
> ```typescript
> const handleSubmit = async (e: React.FormEvent) => {
>   e.preventDefault();
>   
>   if (!product || !validate()) return;
>   
>   setLoading(true);
>   try {
>     await stockService.addStock({
>       productId: product.id,
>       quantity: formData.quantity,
>       reason: formData.reason || undefined,
>     });
>     
>     toast.success(`Added ${formData.quantity} units to stock`);
>     onSuccess();
>     onClose();
>     
>     // Reset form
>     setFormData({ quantity: 0, reason: '' });
>     setErrors({});
>   } catch (error: any) {
>     toast.error(error.message || 'Failed to add stock');
>   } finally {
>     setLoading(false);
>   }
> };
> ```
> 
> **Styling**:
> - Modal: max-w-md
> - Product info section: `bg-gray-50 p-4 rounded-lg mb-4`
> - Calculation display: `bg-blue-50 border border-blue-200 p-3 rounded-lg mb-4`
> - Calculation text: `text-sm text-blue-900 font-medium`
> - Buttons: Cancel (gray), Add Stock (green)
> 
> **Import**:
> - Modal, Input, Textarea, Button from common
> - StockBadge from components/products
> - stockService from services
> - toast from react-hot-toast
> - ProductDto from types
> 
> **Export AddStockModal as default**

---

## Task 3: Create Remove Stock Modal

### Location: `src/components/products/RemoveStockModal.tsx`

**Prompt for Copilot:**

> Create RemoveStockModal component for removing stock from products:
> 
> **Props interface**:
> ```typescript
> interface RemoveStockModalProps {
>   isOpen: boolean;
>   onClose: () => void;
>   product: ProductDto | null;
>   onSuccess: () => void;
> }
> ```
> 
> **Form state**:
> ```typescript
> const [formData, setFormData] = useState({
>   quantity: 0,
>   reason: '',
> });
> const [errors, setErrors] = useState<Record<string, string>>({});
> const [loading, setLoading] = useState(false);
> const [showWarning, setShowWarning] = useState(false);
> ```
> 
> **Component structure**:
> - Modal title: "Remove Stock"
> - Product info display with current stock
> - Form fields:
>   - Quantity (number input, required, min 1, max current stock)
>   - Reason (textarea, optional)
> - Stock calculation display:
>   - "Current: X - Removing: Y = Remaining: X-Y"
> - Warning if remaining < 10:
>   - "⚠️ Warning: Stock will be low after removal"
> - Error if remaining < 0:
>   - "❌ Cannot remove more than available stock"
> - Footer: Cancel and Remove Stock buttons
> 
> **Validation**:
> ```typescript
> const validate = () => {
>   if (!product) return false;
>   
>   const newErrors: Record<string, string> = {};
>   
>   if (!formData.quantity || formData.quantity <= 0) {
>     newErrors.quantity = 'Quantity must be greater than 0';
>   }
>   if (formData.quantity > product.stockQuantity) {
>     newErrors.quantity = `Cannot remove more than available stock (${product.stockQuantity})`;
>   }
>   
>   setErrors(newErrors);
>   
>   // Check if will be low stock
>   const remaining = product.stockQuantity - formData.quantity;
>   setShowWarning(remaining > 0 && remaining < 10);
>   
>   return Object.keys(newErrors).length === 0;
> };
> ```
> 
> **Handle quantity change**:
> ```typescript
> useEffect(() => {
>   if (formData.quantity > 0) {
>     validate();
>   }
> }, [formData.quantity]);
> ```
> 
> **Handle submit**:
> ```typescript
> const handleSubmit = async (e: React.FormEvent) => {
>   e.preventDefault();
>   
>   if (!product || !validate()) return;
>   
>   setLoading(true);
>   try {
>     await stockService.removeStock({
>       productId: product.id,
>       quantity: formData.quantity,
>       reason: formData.reason || undefined,
>     });
>     
>     toast.success(`Removed ${formData.quantity} units from stock`);
>     onSuccess();
>     onClose();
>     
>     // Reset
>     setFormData({ quantity: 0, reason: '' });
>     setErrors({});
>     setShowWarning(false);
>   } catch (error: any) {
>     toast.error(error.message || 'Failed to remove stock');
>   } finally {
>     setLoading(false);
>   }
> };
> ```
> 
> **Styling**:
> - Warning banner: `bg-yellow-50 border border-yellow-200 p-3 rounded-lg mb-4 flex items-start gap-2`
> - Warning icon: `text-yellow-600`
> - Warning text: `text-sm text-yellow-800`
> - Calculation display: `bg-red-50 border border-red-200 p-3 rounded-lg mb-4`
> - Remove button: red background
> 
> **Import**:
> - Modal, Input, Textarea, Button from common
> - StockBadge from components/products
> - stockService
> - toast
> - Icons: ExclamationTriangleIcon from '@heroicons/react/24/outline'
> 
> **Export RemoveStockModal as default**

---

## Task 4: Create Stock Management Hub Modal

### Location: `src/components/products/StockManagementModal.tsx`

**Prompt for Copilot:**

> Create StockManagementModal as a hub for all stock operations:
> 
> **Props interface**:
> ```typescript
> interface StockManagementModalProps {
>   isOpen: boolean;
>   onClose: () => void;
>   product: ProductDto | null;
>   onRefresh: () => void;
> }
> ```
> 
> **Component state**:
> ```typescript
> const [recentHistory, setRecentHistory] = useState<StockHistoryDto[]>([]);
> const [loading, setLoading] = useState(false);
> const [showAddModal, setShowAddModal] = useState(false);
> const [showRemoveModal, setShowRemoveModal] = useState(false);
> ```
> 
> **Load recent history on product change**:
> ```typescript
> useEffect(() => {
>   if (product && isOpen) {
>     loadRecentHistory();
>   }
> }, [product, isOpen]);
> 
> const loadRecentHistory = async () => {
>   if (!product) return;
>   
>   setLoading(true);
>   try {
>     const history = await stockService.getProductStockHistory(product.id, 5);
>     setRecentHistory(history);
>   } catch (error) {
>     console.error('Failed to load stock history:', error);
>   } finally {
>     setLoading(false);
>   }
> };
> ```
> 
> **Component structure**:
> - Modal title: "Manage Stock"
> - Product info section:
>   - Product name, type
>   - Current stock with StockBadge
>   - Last updated date
> - Quick actions (3 large buttons):
>   - "Add Stock" (green, with PlusIcon)
>   - "Remove Stock" (red, with MinusIcon)
>   - "View Full History" (blue, with ClockIcon) → Navigate to /stock-history?productId=X
> - Recent history section:
>   - Title: "Recent Changes (Last 5)"
>   - Mini table: Date, Type, Quantity, Reason
>   - Link: "View all history" → Navigate to stock history page
> 
> **Handle success**:
> ```typescript
> const handleSuccess = () => {
>   loadRecentHistory();
>   onRefresh();
> };
> ```
> 
> **Styling**:
> - Modal: max-w-3xl
> - Product info: `bg-gray-50 p-6 rounded-lg mb-6`
> - Action buttons grid: `grid grid-cols-3 gap-4 mb-6`
> - Action button: `p-4 rounded-lg border-2 border-gray-200 hover:border-primary-500 hover:bg-primary-50 transition-all cursor-pointer text-center`
> - Recent history table: `text-sm`
> - Quantity change: green for + (additions), red for - (removals)
> 
> **Import**:
> - Modal, Button from common
> - StockBadge from components/products
> - AddStockModal, RemoveStockModal (nested modals)
> - stockService
> - useNavigate from react-router-dom
> - Icons from '@heroicons/react/24/outline': PlusIcon, MinusIcon, ClockIcon
> - formatDateTime from formatters
> 
> **Export StockManagementModal as default**

---

## Task 5: Update Products Page with Stock Management

### Location: `src/pages/ProductsPage.tsx`

**Prompt for Copilot:**

> Update ProductsPage to integrate stock management:
> 
> **Add state**:
> ```typescript
> const [showStockModal, setShowStockModal] = useState(false);
> const [selectedProductForStock, setSelectedProductForStock] = useState<ProductDto | null>(null);
> ```
> 
> **Update handleManageStock function**:
> ```typescript
> const handleManageStock = (product: ProductDto) => {
>   setSelectedProductForStock(product);
>   setShowStockModal(true);
> };
> ```
> 
> **Add StockManagementModal to render**:
> ```typescript
> {/* Stock Management Modal */}
> <StockManagementModal
>   isOpen={showStockModal}
>   onClose={() => setShowStockModal(false)}
>   product={selectedProductForStock}
>   onRefresh={loadProducts}
> />
> ```
> 
> **Import**:
> - StockManagementModal from components/products
> 
> **Remove the toast.info line** from handleManageStock that said "Coming in Step 5C"

---

## Task 6: Create Stock History Table Component

### Location: `src/components/products/StockHistoryTable.tsx`

**Prompt for Copilot:**

> Create StockHistoryTable component to display stock history:
> 
> **Props interface**:
> ```typescript
> interface StockHistoryTableProps {
>   history: StockHistoryDto[];
>   loading?: boolean;
> }
> ```
> 
> **Component structure**:
> - Responsive table wrapper
> - Table columns:
>   - Date & Time (formatDateTime)
>   - Product Name
>   - Change Type (badge: Added/Removed/Adjusted)
>   - Quantity Change (with + or - and color)
>   - Quantity After
>   - Reason
>   - Changed By (user name)
> - Loading state
> - Empty state
> 
> **Change Type Badge**:
> ```typescript
> const getChangeTypeBadge = (changeType: string) => {
>   const badges = {
>     Added: 'bg-green-100 text-green-800',
>     Removed: 'bg-red-100 text-red-800',
>     Adjusted: 'bg-blue-100 text-blue-800',
>   };
>   return badges[changeType] || 'bg-gray-100 text-gray-800';
> };
> ```
> 
> **Quantity Change Display**:
> ```typescript
> const formatQuantityChange = (quantity: number) => {
>   if (quantity > 0) {
>     return <span className="text-green-600 font-medium">+{quantity}</span>;
>   } else if (quantity < 0) {
>     return <span className="text-red-600 font-medium">{quantity}</span>;
>   }
>   return <span className="text-gray-600">0</span>;
> };
> ```
> 
> **Styling**:
> - Table: responsive with horizontal scroll
> - Header: `bg-gray-50`
> - Rows: `hover:bg-gray-50`
> - Empty state: centered with icon
> 
> **Import**:
> - StockHistoryDto from types
> - formatDateTime from formatters
> - Icons: ClockIcon for empty state
> 
> **Export StockHistoryTable as default**

---

## Task 7: Create Stock History Page

### Location: `src/pages/StockHistoryPage.tsx`

**Prompt for Copilot:**

> Create StockHistoryPage component for viewing all stock history:
> 
> **Component state**:
> ```typescript
> const [history, setHistory] = useState<StockHistoryDto[]>([]);
> const [filteredHistory, setFilteredHistory] = useState<StockHistoryDto[]>([]);
> const [loading, setLoading] = useState(true);
> const [searchTerm, setSearchTerm] = useState('');
> const [filterProductId, setFilterProductId] = useState<string>('');
> const [filterChangeType, setFilterChangeType] = useState<string>('');
> const [products, setProducts] = useState<ProductDto[]>([]);
> ```
> 
> **useEffect - Load data on mount**:
> ```typescript
> useEffect(() => {
>   loadData();
> }, []);
> 
> const loadData = async () => {
>   setLoading(true);
>   try {
>     const [historyData, productsData] = await Promise.all([
>       stockService.getStockHistory({}),
>       productService.getAllProducts(),
>     ]);
>     setHistory(historyData);
>     setProducts(productsData);
>   } catch (error) {
>     toast.error('Failed to load stock history');
>   } finally {
>     setLoading(false);
>   }
> };
> ```
> 
> **useEffect - Filter history**:
> ```typescript
> useEffect(() => {
>   let filtered = history;
>   
>   // Filter by search term
>   if (searchTerm) {
>     const term = searchTerm.toLowerCase();
>     filtered = filtered.filter(h =>
>       h.productName.toLowerCase().includes(term) ||
>       (h.reason && h.reason.toLowerCase().includes(term)) ||
>       h.changedByName.toLowerCase().includes(term)
>     );
>   }
>   
>   // Filter by product
>   if (filterProductId) {
>     filtered = filtered.filter(h => h.productId === filterProductId);
>   }
>   
>   // Filter by change type
>   if (filterChangeType) {
>     filtered = filtered.filter(h => h.changeType === filterChangeType);
>   }
>   
>   setFilteredHistory(filtered);
> }, [history, searchTerm, filterProductId, filterChangeType]);
> ```
> 
> **Page structure**:
> ```typescript
> return (
>   <div className="p-6">
>     {/* Header */}
>     <div className="mb-6">
>       <h1 className="text-3xl font-bold text-gray-900">Stock History</h1>
>       <p className="text-gray-600 mt-1">Track all stock movements and adjustments</p>
>     </div>
> 
>     {/* Filters */}
>     <div className="bg-white rounded-lg shadow p-4 mb-6">
>       <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
>         {/* Search */}
>         <Input
>           type="text"
>           placeholder="Search by product, reason, or user..."
>           value={searchTerm}
>           onChange={(e) => setSearchTerm(e.target.value)}
>         />
>         
>         {/* Filter by product */}
>         <Select
>           value={filterProductId}
>           onChange={(value) => setFilterProductId(value as string)}
>           options={[
>             { value: '', label: 'All Products' },
>             ...products.map(p => ({ value: p.id, label: p.name }))
>           ]}
>           placeholder="Filter by product"
>         />
>         
>         {/* Filter by change type */}
>         <Select
>           value={filterChangeType}
>           onChange={(value) => setFilterChangeType(value as string)}
>           options={[
>             { value: '', label: 'All Types' },
>             { value: 'Added', label: 'Added' },
>             { value: 'Removed', label: 'Removed' },
>             { value: 'Adjusted', label: 'Adjusted' },
>           ]}
>           placeholder="Filter by type"
>         />
>       </div>
>     </div>
> 
>     {/* Stats */}
>     <div className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-6">
>       <div className="bg-white rounded-lg shadow p-6">
>         <p className="text-sm font-medium text-gray-600">Total Changes</p>
>         <p className="text-2xl font-bold text-gray-900">{filteredHistory.length}</p>
>       </div>
>       <div className="bg-white rounded-lg shadow p-6">
>         <p className="text-sm font-medium text-gray-600">Stock Added</p>
>         <p className="text-2xl font-bold text-green-600">
>           {filteredHistory.filter(h => h.changeType === 'Added').length}
>         </p>
>       </div>
>       <div className="bg-white rounded-lg shadow p-6">
>         <p className="text-sm font-medium text-gray-600">Stock Removed</p>
>         <p className="text-2xl font-bold text-red-600">
>           {filteredHistory.filter(h => h.changeType === 'Removed').length}
>         </p>
>       </div>
>       <div className="bg-white rounded-lg shadow p-6">
>         <p className="text-sm font-medium text-gray-600">Adjustments</p>
>         <p className="text-2xl font-bold text-blue-600">
>           {filteredHistory.filter(h => h.changeType === 'Adjusted').length}
>         </p>
>       </div>
>     </div>
> 
>     {/* History Table */}
>     <div className="bg-white rounded-lg shadow">
>       <StockHistoryTable history={filteredHistory} loading={loading} />
>     </div>
>   </div>
> );
> ```
> 
> **Import**:
> - StockHistoryTable from components/products
> - Input, Select from common
> - stockService, productService
> - toast
> - Types
> 
> **Export StockHistoryPage as default**

---

## Task 8: Create Low Stock Alert Component

### Location: `src/components/products/LowStockAlert.tsx`

**Prompt for Copilot:**

> Create LowStockAlert component for dashboard warnings:
> 
> **Props interface**:
> ```typescript
> interface LowStockAlertProps {
>   products: ProductDto[];
>   threshold?: number;  // default 10
>   onAddStock?: (product: ProductDto) => void;
> }
> ```
> 
> **Component logic**:
> ```typescript
> const lowStockProducts = products
>   .filter(p => p.stockQuantity <= (threshold || 10) && p.stockQuantity > 0)
>   .sort((a, b) => a.stockQuantity - b.stockQuantity); // Lowest first
> 
> const outOfStockProducts = products.filter(p => p.stockQuantity === 0);
> ```
> 
> **Component structure**:
> - Alert banner (only show if there are low/out of stock products)
> - Title: "⚠️ Stock Alerts"
> - Two sections:
>   - Out of Stock (red) - if any
>   - Low Stock (yellow) - if any
> - Each product shows:
>   - Product name
>   - StockBadge
>   - "Add Stock" button (if onAddStock provided)
> - "View All" link to /stock-history
> - Collapsible (can be expanded/collapsed)
> 
> **Styling**:
> - Alert container: `bg-yellow-50 border-l-4 border-yellow-400 p-4 mb-6`
> - Out of stock section: `bg-red-50 border border-red-200 p-3 rounded mb-3`
> - Low stock section: `bg-yellow-50 border border-yellow-200 p-3 rounded`
> - Product item: `flex justify-between items-center py-2 border-b last:border-b-0`
> - Add Stock button: small, green
> 
> **Limit display**:
> - Show max 5 products per section
> - If more exist, show "... and X more"
> 
> **Import**:
> - StockBadge from components/products
> - Button from common
> - Icons: ExclamationTriangleIcon, ChevronDownIcon, ChevronUpIcon
> - Link from react-router-dom
> 
> **Export LowStockAlert as default**

---

## Task 9: Update Dashboard with Low Stock Alert

### Location: `src/pages/DashboardPage.tsx`

**Prompt for Copilot:**

> Update DashboardPage to show low stock alerts:
> 
> **Add state**:
> ```typescript
> const [products, setProducts] = useState<ProductDto[]>([]);
> const [showAddStockModal, setShowAddStockModal] = useState(false);
> const [selectedProduct, setSelectedProduct] = useState<ProductDto | null>(null);
> ```
> 
> **Load products on mount**:
> ```typescript
> useEffect(() => {
>   loadProducts();
> }, []);
> 
> const loadProducts = async () => {
>   try {
>     const data = await productService.getAllProducts();
>     setProducts(data);
>   } catch (error) {
>     console.error('Failed to load products:', error);
>   }
> };
> ```
> 
> **Handle add stock from alert**:
> ```typescript
> const handleAddStockFromAlert = (product: ProductDto) => {
>   setSelectedProduct(product);
>   setShowAddStockModal(true);
> };
> ```
> 
> **Add to page structure** (at top, before existing dashboard content):
> ```typescript
> {/* Low Stock Alert */}
> <LowStockAlert
>   products={products}
>   onAddStock={handleAddStockFromAlert}
> />
> 
> {/* Existing dashboard content */}
> ...
> 
> {/* Add Stock Modal */}
> <AddStockModal
>   isOpen={showAddStockModal}
>   onClose={() => setShowAddStockModal(false)}
>   product={selectedProduct}
>   onSuccess={loadProducts}
> />
> ```
> 
> **Import**:
> - LowStockAlert from components/products
> - AddStockModal from components/products
> - productService
> 
> Keep all existing dashboard functionality

---

## Task 10: Update ProductCard with StockBadge

### Location: `src/components/products/ProductCard.tsx`

**Prompt for Copilot:**

> Update ProductCard to use the new StockBadge component:
> 
> **Replace inline stock badge logic** with:
> ```typescript
> <StockBadge 
>   quantity={product.stockQuantity} 
>   isLowStock={product.isLowStock}
>   size="sm"
> />
> ```
> 
> **Import**:
> - StockBadge from components/products
> 
> Remove any inline stock badge styling code

---

## Task 11: Update ProductTable with StockBadge

### Location: `src/components/products/ProductTable.tsx`

**Prompt for Copilot:**

> Update ProductTable to use the new StockBadge component:
> 
> **Replace inline stock display** in the Stock column with:
> ```typescript
> <StockBadge 
>   quantity={product.stockQuantity} 
>   isLowStock={product.isLowStock}
>   size="sm"
> />
> ```
> 
> **Import**:
> - StockBadge from components/products

---

## Verification Checklist

Before considering Step 5C complete, verify:

### Components:
- [ ] StockBadge displays correctly with colors
- [ ] AddStockModal works (adds stock)
- [ ] RemoveStockModal works (removes stock with validation)
- [ ] RemoveStockModal shows warning when stock will be low
- [ ] RemoveStockModal prevents removing more than available
- [ ] StockManagementModal shows recent history
- [ ] StockManagementModal quick actions work
- [ ] StockHistoryTable displays all columns
- [ ] LowStockAlert shows on dashboard
- [ ] LowStockAlert "Add Stock" button works

### Stock Operations:
- [ ] Can add stock to any product
- [ ] Stock quantity updates after adding
- [ ] Can remove stock from products
- [ ] Cannot remove more than available
- [ ] Warning appears when stock will be low
- [ ] Toast notifications appear for all operations
- [ ] Products refresh after stock changes

### Stock History:
- [ ] Stock History page loads
- [ ] Can filter by product
- [ ] Can filter by change type
- [ ] Can search history
- [ ] History shows all changes (Added, Removed)
- [ ] Quantity changes color-coded (green +, red -)
- [ ] Stats calculate correctly

### Dashboard:
- [ ] Low stock alert appears when products are low
- [ ] Out of stock section shows (red) when products are 0
- [ ] Low stock section shows (yellow) when products are 1-10
- [ ] Can add stock directly from alert
- [ ] Alert collapses/expands
- [ ] "View All" link navigates to stock history

### Integration:
- [ ] ProductCard shows stock badge
- [ ] ProductTable shows stock badge
- [ ] "Manage Stock" button on ProductsPage opens StockManagementModal
- [ ] All modals close properly
- [ ] Products refresh after all stock operations

### General:
- [ ] No TypeScript errors
- [ ] No console errors
- [ ] All API calls work
- [ ] Loading states work
- [ ] All styling consistent

---

## What You've Accomplished

✅ **Complete Stock Management System**  
✅ **Add and remove stock** with validation  
✅ **Stock history tracking** with full audit trail  
✅ **Stock badges** with color-coded indicators  
✅ **Low stock alerting** on dashboard  
✅ **Stock Management hub** with recent history  
✅ **Dedicated Stock History page** with filters  
✅ **Dashboard integration** with quick actions  
✅ **Professional UI** with warnings and validations  
✅ **Real-time updates** and refreshing  

---

## Complete Inventory System Summary

At this point, you have a **production-ready Inventory Management System** with:

### Backend (Steps 1-4):
- ✅ Clean Architecture with Domain, Application, Infrastructure layers
- ✅ CQRS with MediatR
- ✅ 5 Product types with proper inheritance
- ✅ Stock management with history tracking
- ✅ User authentication and authorization
- ✅ Unit and integration tests

### Frontend (Steps 5A-5C):
- ✅ TypeScript types matching backend
- ✅ Product CRUD for all 5 types
- ✅ Type-specific forms with validation
- ✅ Stock management (add/remove/history)
- ✅ Dashboard with alerts
- ✅ Beautiful, responsive UI
- ✅ ~35-40 React components
- ✅ ~5,000-6,000 lines of code

---

## Optional Enhancements

If you want to extend the system further:

### 1. Image Upload:
- Integrate Cloudinary or AWS S3
- Add image preview in forms
- Implement drag-and-drop upload

### 2. Advanced Filtering:
- Date range for stock history
- Multiple product selection
- Export to CSV/Excel

### 3. Notifications:
- Email alerts for low stock
- Browser push notifications
- Daily/weekly stock reports

### 4. Analytics:
- Stock movement trends
- Product performance metrics
- Predictive stock alerts

### 5. Barcode/QR:
- Generate product barcodes
- Scan for quick stock updates
- Print product labels

### 6. Mobile App:
- React Native version
- Quick stock adjustments
- Barcode scanning

---

## Testing Guide

### Stock Management Testing:

1. **Add Stock:**
   - Add 50 units to a product
   - Verify quantity updates
   - Check history shows "Added" entry

2. **Remove Stock:**
   - Remove 20 units
   - Try removing more than available (should fail)
   - Remove units to bring stock below 10 (should show warning)
   - Verify history shows "Removed" entry

3. **Low Stock Alerts:**
   - Create/update product with stock = 5
   - Verify yellow "Low Stock" badge appears
   - Verify alert appears on dashboard
   - Click "Add Stock" from alert

4. **Out of Stock:**
   - Remove all stock from a product (quantity = 0)
   - Verify red "Out of Stock" badge
   - Verify product appears in dashboard alert (red section)
   - Try removing stock (should fail)

5. **Stock History:**
   - Navigate to Stock History page
   - Verify all changes appear
   - Test filters (by product, by type)
   - Test search functionality

6. **Stock Management Modal:**
   - Click "Manage Stock" on any product
   - Verify recent history loads
   - Test Add/Remove from modal
   - Verify history updates

---

## Congratulations! 🎉

You've built a **complete, professional Inventory Management System** with:

- 🏗️ **Clean Architecture** backend
- ⚛️ **Modern React** frontend
- 📦 **5 Product Types** fully managed
- 📊 **Stock Management** with history
- 🎨 **Beautiful UI** with TailwindCSS
- 🔒 **Type-safe** with TypeScript
- ✅ **Production-ready** code

**Total Development Time:** ~20-25 hours  
**Total Code:** ~8,000-10,000 lines  
**Components:** ~40 React components  
**Features:** Full CRUD + Stock Management + Analytics

You now have a solid foundation that can be extended for any inventory, warehouse, or e-commerce system! 🚀

---

## Need Help?

**Common Issues:**

- **Stock not updating:** Check API response, verify product ID
- **Validation errors:** Check min/max values, required fields
- **History not loading:** Verify stockService methods, check API endpoints
- **Modals not closing:** Check state updates, verify onClose handlers

**Tips:**
- Test stock operations with small quantities first
- Always check browser console for errors
- Verify API responses in Network tab
- Test edge cases (0 stock, negative numbers)

**Remember:**
- Stock operations are permanent (no undo)
- History tracks all changes
- Low stock threshold is 10 by default
- All stock changes require authentication

Good luck! 🎊
