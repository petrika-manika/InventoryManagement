# Step 6B: Frontend - Clients Module React UI
## Instructions for GitHub Copilot

---

## Overview
This guide provides instructions for building the **Clients Module frontend** with React, TypeScript, and TailwindCSS. We'll implement UI for managing two types of clients: Individual and Business.

**Client Types:**
- **Individual** - Personal clients with basic contact information
- **Business** - Company clients with NIPT, owner, and contact person details

**Time estimate:** 4-5 hours

---

## Folder Structure

Create this folder structure in `src`:

```
src/
├── components/
│   └── clients/
│       ├── ClientCard.tsx
│       ├── ClientTable.tsx
│       ├── ClientFilters.tsx
│       ├── ClientStats.tsx
│       ├── ClientTypeModal.tsx
│       ├── IndividualClientForm.tsx
│       └── BusinessClientForm.tsx
├── services/
│   └── clientService.ts
├── types/
│   └── client.types.ts
├── utils/
│   └── formatters.ts  # Update with client formatters
└── pages/
    ├── ClientsPage.tsx
    └── ClientDetailPage.tsx  # Optional
```

---

## Part 1: Types and Services Setup

### Task 1: Create Client Types

**Location**: `src/types/client.types.ts`

**Prompt for Copilot:**

> Create TypeScript types for the Clients module:
> 
> **ClientType enum as const object**:
> ```typescript
> export const ClientType = {
>   Individual: 1,
>   Business: 2,
> } as const;
> 
> export type ClientTypeValue = typeof ClientType[keyof typeof ClientType];
> ```
> 
> **ClientType labels**:
> ```typescript
> export const ClientTypeLabels: Record<number, string> = {
>   1: 'Individual',
>   2: 'Business',
> };
> ```
> 
> **Base ClientDto interface**:
> - id: string
> - clientType: string
> - clientTypeId: number
> - address?: string
> - email?: string
> - phoneNumber?: string
> - notes?: string
> - createdAt: string
> - updatedAt: string
> - createdBy: string
> - updatedBy?: string
> - isActive: boolean
> 
> **IndividualClientDto interface** (extends ClientDto):
> - firstName: string
> - lastName: string
> - fullName: string
> 
> **BusinessClientDto interface** (extends ClientDto):
> - nipt: string
> - ownerFirstName?: string
> - ownerLastName?: string
> - ownerPhoneNumber?: string
> - ownerFullName?: string
> - contactPersonFirstName: string
> - contactPersonLastName: string
> - contactPersonPhoneNumber?: string
> - contactPersonFullName: string
> 
> **Create Request interfaces**:
> 
> **CreateIndividualClientRequest**:
> - firstName: string
> - lastName: string
> - address?: string
> - email?: string
> - phoneNumber?: string
> - notes?: string
> 
> **CreateBusinessClientRequest**:
> - nipt: string
> - ownerFirstName?: string
> - ownerLastName?: string
> - ownerPhoneNumber?: string
> - contactPersonFirstName: string
> - contactPersonLastName: string
> - contactPersonPhoneNumber?: string
> - address?: string
> - email?: string
> - phoneNumber?: string
> - notes?: string
> 
> **Update Request interfaces**:
> Same as Create but for updates (UpdateIndividualClientRequest, UpdateBusinessClientRequest)
> 
> Export all types

---

### Task 2: Create Client Service

**Location**: `src/services/clientService.ts`

**Prompt for Copilot:**

> Create clientService with API methods for client management:
> 
> **Import**:
> - apiClient from './apiClient'
> - All types from client.types.ts
> 
> **Methods**:
> 
> 1. **getAllClients(includeInactive = false): Promise<ClientDto[]>**
>    - GET /api/clients?includeInactive={includeInactive}
>    - Returns array of clients (can be Individual or Business)
> 
> 2. **getClientsByType(clientTypeId: number, includeInactive = false): Promise<ClientDto[]>**
>    - GET /api/clients/type/{clientTypeId}?includeInactive={includeInactive}
>    - Returns filtered clients by type
> 
> 3. **getClientById(id: string): Promise<ClientDto>**
>    - GET /api/clients/{id}
>    - Returns single client
> 
> 4. **searchClients(searchTerm?: string, clientTypeId?: number, includeInactive = false): Promise<ClientDto[]>**
>    - GET /api/clients/search?searchTerm={searchTerm}&clientTypeId={clientTypeId}&includeInactive={includeInactive}
>    - Returns search results
> 
> 5. **createIndividualClient(data: CreateIndividualClientRequest): Promise<string>**
>    - POST /api/clients/individual
>    - Body: data
>    - Returns: client ID
> 
> 6. **createBusinessClient(data: CreateBusinessClientRequest): Promise<string>**
>    - POST /api/clients/business
>    - Body: data
>    - Returns: client ID
> 
> 7. **updateIndividualClient(id: string, data: UpdateIndividualClientRequest): Promise<void>**
>    - PUT /api/clients/individual/{id}
>    - Body: data
> 
> 8. **updateBusinessClient(id: string, data: UpdateBusinessClientRequest): Promise<void>**
>    - PUT /api/clients/business/{id}
>    - Body: data
> 
> 9. **deleteClient(id: string): Promise<void>**
>    - DELETE /api/clients/{id}
>    - Soft deletes the client
> 
> **Helper method**:
> 10. **createClient(clientTypeId: number, data: any): Promise<string>**
>     - Switch on clientTypeId
>     - Call createIndividualClient or createBusinessClient
>     - Simplifies form submission
> 
> 11. **updateClient(clientTypeId: number, id: string, data: any): Promise<void>**
>     - Switch on clientTypeId
>     - Call updateIndividualClient or updateBusinessClient
> 
> **Error handling**:
> - Wrap all calls in try-catch
> - Throw errors with clear messages
> 
> Export clientService as default

---

### Task 3: Update Formatters Utility

**Location**: `src/utils/formatters.ts`

**Prompt for Copilot:**

> Add client-specific formatter functions:
> 
> **Function: getClientTypeLabel(clientTypeId: number): string**
> - Import ClientTypeLabels from client.types
> - Return ClientTypeLabels[clientTypeId] || 'Unknown'
> 
> **Function: formatPhoneNumber(phoneNumber?: string): string**
> - If null/empty, return '-'
> - Return formatted phone number (can add formatting logic if desired)
> 
> **Function: formatNIPT(nipt: string): string**
> - Format NIPT for display (e.g., "K12345678L" → "K 1234 5678 L")
> - Or return as-is if no formatting desired
> 
> Keep existing formatters (formatCurrency, formatDate, etc.)

---

## Part 2: Client Components

### Task 4: Create Client Card Component

**Location**: `src/components/clients/ClientCard.tsx`

**Prompt for Copilot:**

> Create ClientCard component to display a single client in grid view:
> 
> **Props interface**:
> ```typescript
> interface ClientCardProps {
>   client: ClientDto;
>   onEdit: (client: ClientDto) => void;
>   onDelete: (client: ClientDto) => void;
> }
> ```
> 
> **Component structure**:
> - Card container with hover effect
> - Header section:
>   - Client type badge (Individual: green, Business: blue)
>   - Client name (fullName for individual, contactPersonFullName for business)
> - Info section:
>   - For Individual: FirstName, LastName
>   - For Business: NIPT, Contact Person, Owner (if exists)
>   - Email (if exists)
>   - Phone (if exists)
>   - Address (if exists, truncated to 100 chars)
> - Notes preview (if exists, first 50 chars with "...")
> - Action buttons:
>   - Edit button (blue)
>   - Delete button (red)
> 
> **Styling (TailwindCSS)**:
> - Card: `bg-white rounded-lg shadow hover:shadow-xl transition-shadow border border-gray-200 p-5`
> - Header: flex justify-between items-start
> - Type badge: 
>   - Individual: `bg-green-100 text-green-800`
>   - Business: `bg-blue-100 text-blue-800`
> - Name: `text-lg font-bold text-gray-900 mt-2`
> - Info items: `text-sm text-gray-600 flex items-center gap-2`
> - Icons: Use @heroicons/react/24/outline (UserIcon, BuildingOfficeIcon, EnvelopeIcon, PhoneIcon, MapPinIcon)
> - Actions: flex gap-2, buttons with icons
> 
> **Type checking**:
> - Use type guards to check if client is IndividualClientDto or BusinessClientDto
> - TypeScript type narrowing with 'clientTypeId' or 'firstName' / 'nipt' checks
> 
> Export as default

---

### Task 5: Create Client Table Component

**Location**: `src/components/clients/ClientTable.tsx`

**Prompt for Copilot:**

> Create ClientTable component to display clients in table view:
> 
> **Props interface**:
> ```typescript
> interface ClientTableProps {
>   clients: ClientDto[];
>   onEdit: (client: ClientDto) => void;
>   onDelete: (client: ClientDto) => void;
>   loading?: boolean;
> }
> ```
> 
> **Component structure**:
> - Responsive table wrapper (horizontal scroll on mobile)
> - Table columns:
>   - Type (badge)
>   - Name (Individual: fullName, Business: contactPersonFullName)
>   - Contact Info (Email / Phone)
>   - For Business: NIPT
>   - Address (truncated)
>   - Actions (Edit, Delete icon buttons)
> - Loading state
> - Empty state
> 
> **Styling**:
> - Table: `min-w-full divide-y divide-gray-200`
> - Header: `bg-gray-50`
> - Header cells: `px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase`
> - Body rows: `hover:bg-gray-50 transition-colors`
> - Type badge sizing: small (`text-xs px-2 py-0.5`)
> - Action buttons: icon-only, with hover colors
> 
> **Empty state**:
> ```typescript
> {clients.length === 0 && !loading && (
>   <tr>
>     <td colSpan={6} className="px-6 py-12 text-center">
>       <UsersIcon className="mx-auto h-12 w-12 text-gray-400" />
>       <p className="mt-2 text-sm text-gray-500">No clients found</p>
>     </td>
>   </tr>
> )}
> ```
> 
> Export as default

---

### Task 6: Create Client Filters Component

**Location**: `src/components/clients/ClientFilters.tsx`

**Prompt for Copilot:**

> Create ClientFilters component for filtering and searching clients:
> 
> **Props interface**:
> ```typescript
> interface ClientFiltersProps {
>   searchTerm: string;
>   onSearchChange: (value: string) => void;
>   filterType: number;  // 0 = All, 1 = Individual, 2 = Business
>   onTypeChange: (typeId: number) => void;
>   viewMode: 'grid' | 'table';
>   onViewModeChange: (mode: 'grid' | 'table') => void;
> }
> ```
> 
> **Component structure**:
> - Horizontal flex container with filters
> - Search input (with MagnifyingGlassIcon)
> - Client type select dropdown:
>   - Options: All Clients, Individual, Business
> - View mode toggle (grid/table icons)
> 
> **Layout**:
> ```typescript
> <div className="flex flex-col sm:flex-row gap-4 items-stretch sm:items-center">
>   {/* Search input - flex-1 */}
>   <div className="flex-1">
>     <Input
>       type="text"
>       placeholder="Search by name, email, phone, or NIPT..."
>       value={searchTerm}
>       onChange={(e) => onSearchChange(e.target.value)}
>     />
>   </div>
>   
>   {/* Type filter */}
>   <div className="w-full sm:w-64">
>     <Select
>       value={filterType}
>       onChange={onTypeChange}
>       options={[
>         { value: 0, label: 'All Clients' },
>         { value: 1, label: 'Individual' },
>         { value: 2, label: 'Business' },
>       ]}
>     />
>   </div>
>   
>   {/* View mode toggle */}
>   <div className="flex gap-2">
>     <button /* grid icon */ />
>     <button /* table icon */ />
>   </div>
> </div>
> ```
> 
> Import Input and Select from common components
> Import icons: MagnifyingGlassIcon, Squares2X2Icon, TableCellsIcon
> 
> Export as default

---

### Task 7: Create Client Stats Component

**Location**: `src/components/clients/ClientStats.tsx`

**Prompt for Copilot:**

> Create ClientStats component to display client statistics:
> 
> **Props interface**:
> ```typescript
> interface ClientStatsProps {
>   clients: ClientDto[];
> }
> ```
> 
> **Component logic**:
> Calculate statistics:
> - Total clients count
> - Individual clients count (clientTypeId === 1)
> - Business clients count (clientTypeId === 2)
> - Active clients (isActive === true)
> 
> **Component structure**:
> Grid of stat cards (4 columns on desktop, 2 on tablet, 1 on mobile)
> 
> Each card shows:
> - Icon with colored background
> - Label
> - Value (large, bold)
> 
> **Stats to display**:
> 1. **Total Clients**: count of all clients, purple, UsersIcon
> 2. **Individual**: count where clientTypeId === 1, green, UserIcon
> 3. **Business**: count where clientTypeId === 2, blue, BuildingOfficeIcon
> 4. **Active**: count where isActive === true, emerald, CheckCircleIcon
> 
> **Styling**:
> - Grid: `grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 mb-6`
> - Card: `bg-white rounded-lg shadow p-6 flex items-center gap-4`
> - Icon container: `p-3 rounded-full` with background color
> - Icon: `h-6 w-6 text-white`
> - Label: `text-sm font-medium text-gray-600`
> - Value: `text-2xl font-bold text-gray-900`
> 
> Export as default

---

### Task 8: Create Client Type Selection Modal

**Location**: `src/components/clients/ClientTypeModal.tsx`

**Prompt for Copilot:**

> Create ClientTypeModal for selecting client type before creation:
> 
> **Props interface**:
> ```typescript
> interface ClientTypeModalProps {
>   isOpen: boolean;
>   onClose: () => void;
>   onSelectType: (typeId: number) => void;
> }
> ```
> 
> **Component structure**:
> - Modal with title "Select Client Type"
> - Grid of 2 client type cards (2 columns on desktop, 1 on mobile)
> 
> **Client type cards data**:
> ```typescript
> const clientTypes = [
>   {
>     id: 1,
>     name: 'Individual',
>     description: 'Personal client with basic contact information',
>     icon: UserIcon,
>     color: 'bg-green-500',
>   },
>   {
>     id: 2,
>     name: 'Business',
>     description: 'Company client with NIPT, owner, and contact person',
>     icon: BuildingOfficeIcon,
>     color: 'bg-blue-500',
>   },
> ];
> ```
> 
> **Styling**:
> - Grid: `grid grid-cols-1 md:grid-cols-2 gap-4 mt-4`
> - Card: `border-2 border-gray-200 rounded-lg p-6 hover:border-primary-500 hover:bg-primary-50 cursor-pointer transition-all`
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
> Export as default

---

### Task 9: Create Individual Client Form

**Location**: `src/components/clients/IndividualClientForm.tsx`

**Prompt for Copilot:**

> Create IndividualClientForm component for creating/editing individual clients:
> 
> **Props interface**:
> ```typescript
> interface IndividualClientFormProps {
>   isOpen: boolean;
>   onClose: () => void;
>   client?: IndividualClientDto;  // undefined for create, defined for edit
>   onSubmit: (data: CreateIndividualClientRequest | UpdateIndividualClientRequest) => Promise<void>;
> }
> ```
> 
> **Form state** (use useState):
> ```typescript
> const [formData, setFormData] = useState({
>   firstName: client?.firstName || '',
>   lastName: client?.lastName || '',
>   address: client?.address || '',
>   email: client?.email || '',
>   phoneNumber: client?.phoneNumber || '',
>   notes: client?.notes || '',
> });
> const [errors, setErrors] = useState<Record<string, string>>({});
> const [loading, setLoading] = useState(false);
> ```
> 
> **Form fields** (2-column grid on desktop):
> - **Row 1**: First Name (required), Last Name (required)
> - **Row 2**: Email (optional, validated), Phone Number (optional)
> - **Row 3**: Address (textarea, optional, full width, rows 2)
> - **Row 4**: Notes (textarea, optional, full width, rows 3)
> 
> **Validation**:
> - FirstName: required, min 1, max 50 characters
> - LastName: required, min 1, max 50 characters
> - Email: valid email format when provided (regex: /^[^\s@]+@[^\s@]+\.[^\s@]+$/)
> - PhoneNumber: valid format when provided (regex: /^[\d\s\+\-\(\)]+$/)
> - Address: max 500 characters
> 
> **Handle submit**:
> ```typescript
> const handleSubmit = async (e: React.FormEvent) => {
>   e.preventDefault();
>   
>   // Validate
>   const newErrors: Record<string, string> = {};
>   
>   if (!formData.firstName || formData.firstName.trim().length === 0) {
>     newErrors.firstName = 'First name is required';
>   } else if (formData.firstName.length > 50) {
>     newErrors.firstName = 'First name must be 50 characters or less';
>   }
>   
>   if (!formData.lastName || formData.lastName.trim().length === 0) {
>     newErrors.lastName = 'Last name is required';
>   } else if (formData.lastName.length > 50) {
>     newErrors.lastName = 'Last name must be 50 characters or less';
>   }
>   
>   if (formData.email && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email)) {
>     newErrors.email = 'Invalid email format';
>   }
>   
>   if (formData.phoneNumber && !/^[\d\s\+\-\(\)]+$/.test(formData.phoneNumber)) {
>     newErrors.phoneNumber = 'Invalid phone number format';
>   }
>   
>   if (formData.address && formData.address.length > 500) {
>     newErrors.address = 'Address must be 500 characters or less';
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
>     toast.success(client ? 'Client updated successfully' : 'Client created successfully');
>     onClose();
>   } catch (error: any) {
>     toast.error(error.message || 'Failed to save client');
>   } finally {
>     setLoading(false);
>   }
> };
> ```
> 
> **Modal structure**:
> - Title: "Create Individual Client" or "Edit Individual Client"
> - Form with 2-column grid
> - Footer with Cancel and Save buttons
> 
> **Styling**:
> - Modal: max-w-2xl
> - Form grid: `grid grid-cols-1 md:grid-cols-2 gap-4`
> - Full width fields: `md:col-span-2`
> 
> Import Modal, Input, Textarea, Button from common
> Import toast from react-hot-toast
> 
> Export as default

---

### Task 10: Create Business Client Form

**Location**: `src/components/clients/BusinessClientForm.tsx`

**Prompt for Copilot:**

> Create BusinessClientForm component for creating/editing business clients:
> 
> **Props interface**:
> ```typescript
> interface BusinessClientFormProps {
>   isOpen: boolean;
>   onClose: () => void;
>   client?: BusinessClientDto;
>   onSubmit: (data: CreateBusinessClientRequest | UpdateBusinessClientRequest) => Promise<void>;
> }
> ```
> 
> **Form state**:
> ```typescript
> const [formData, setFormData] = useState({
>   nipt: client?.nipt || '',
>   ownerFirstName: client?.ownerFirstName || '',
>   ownerLastName: client?.ownerLastName || '',
>   ownerPhoneNumber: client?.ownerPhoneNumber || '',
>   contactPersonFirstName: client?.contactPersonFirstName || '',
>   contactPersonLastName: client?.contactPersonLastName || '',
>   contactPersonPhoneNumber: client?.contactPersonPhoneNumber || '',
>   address: client?.address || '',
>   email: client?.email || '',
>   phoneNumber: client?.phoneNumber || '',
>   notes: client?.notes || '',
> });
> ```
> 
> **Form fields** (2-column grid):
> - **Section 1 - Business Info**:
>   - Row 1: NIPT (required, 10 characters)
>   - Row 2: Email (optional), Phone (optional)
>   - Row 3: Address (textarea, 2 rows, full width)
> 
> - **Section 2 - Owner Info** (section header):
>   - Row 4: Owner First Name (optional), Owner Last Name (optional)
>   - Row 5: Owner Phone Number (optional, full width or span 1)
> 
> - **Section 3 - Contact Person** (section header):
>   - Row 6: Contact First Name (required), Contact Last Name (required)
>   - Row 7: Contact Phone Number (optional, full width or span 1)
> 
> - **Section 4 - Notes**:
>   - Row 8: Notes (textarea, 3 rows, full width)
> 
> **Validation**:
> - NIPT: required, exactly 10 alphanumeric characters (regex: /^[A-Za-z0-9]{10}$/)
> - ContactPersonFirstName: required, 1-50 characters
> - ContactPersonLastName: required, 1-50 characters
> - OwnerFirstName/LastName: max 50 characters when provided
> - All phone numbers: phone regex when provided
> - Email: email format when provided
> - Address: max 500 characters
> 
> **Handle submit**: Similar to Individual form but with more validations
> 
> **Modal structure**:
> - Title: "Create Business Client" or "Edit Business Client"
> - Form with sections separated by headings
> - 2-column grid layout
> 
> **Section headers styling**:
> ```typescript
> <h3 className="md:col-span-2 text-sm font-semibold text-gray-700 mt-4 mb-2 border-b pb-2">
>   Owner Information (Optional)
> </h3>
> ```
> 
> Export as default

---

## Part 3: Clients Page

### Task 11: Create Clients Page

**Location**: `src/pages/ClientsPage.tsx`

**Prompt for Copilot:**

> Create the main ClientsPage component with complete client management:
> 
> **Component state**:
> ```typescript
> const [clients, setClients] = useState<ClientDto[]>([]);
> const [filteredClients, setFilteredClients] = useState<ClientDto[]>([]);
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
> const [selectedClient, setSelectedClient] = useState<ClientDto | null>(null);
> const [selectedClientType, setSelectedClientType] = useState<number | null>(null);
> ```
> 
> **useEffect - Load clients on mount**:
> ```typescript
> useEffect(() => {
>   loadClients();
> }, []);
> ```
> 
> **useEffect - Filter clients when search/filter changes**:
> ```typescript
> useEffect(() => {
>   let filtered = clients;
>   
>   // Filter by type
>   if (filterType > 0) {
>     filtered = filtered.filter(c => c.clientTypeId === filterType);
>   }
>   
>   // Filter by search term
>   if (searchTerm) {
>     const term = searchTerm.toLowerCase();
>     filtered = filtered.filter(c => {
>       // Search in common fields
>       if (c.email?.toLowerCase().includes(term)) return true;
>       if (c.phoneNumber?.toLowerCase().includes(term)) return true;
>       
>       // Type-specific search
>       if (c.clientTypeId === 1) {
>         const individual = c as IndividualClientDto;
>         return individual.fullName.toLowerCase().includes(term);
>       } else {
>         const business = c as BusinessClientDto;
>         return business.nipt.toLowerCase().includes(term) ||
>                business.contactPersonFullName.toLowerCase().includes(term) ||
>                (business.ownerFullName && business.ownerFullName.toLowerCase().includes(term));
>       }
>     });
>   }
>   
>   setFilteredClients(filtered);
> }, [clients, filterType, searchTerm]);
> ```
> 
> **Functions**:
> ```typescript
> const loadClients = async () => {
>   setLoading(true);
>   try {
>     const data = await clientService.getAllClients();
>     setClients(data);
>   } catch (error) {
>     toast.error('Failed to load clients');
>   } finally {
>     setLoading(false);
>   }
> };
> 
> const handleAddClick = () => {
>   setSelectedClient(null);
>   setSelectedClientType(null);
>   setShowTypeModal(true);
> };
> 
> const handleSelectType = (typeId: number) => {
>   setSelectedClientType(typeId);
>   setShowTypeModal(false);
>   setShowFormModal(true);
> };
> 
> const handleEdit = (client: ClientDto) => {
>   setSelectedClient(client);
>   setSelectedClientType(client.clientTypeId);
>   setShowFormModal(true);
> };
> 
> const handleDelete = (client: ClientDto) => {
>   setSelectedClient(client);
>   setShowDeleteDialog(true);
> };
> 
> const confirmDelete = async () => {
>   if (!selectedClient) return;
>   
>   try {
>     await clientService.deleteClient(selectedClient.id);
>     toast.success('Client deleted successfully');
>     loadClients();
>     setShowDeleteDialog(false);
>   } catch (error: any) {
>     toast.error(error.message || 'Failed to delete client');
>   }
> };
> 
> const handleFormSubmit = async (data: any) => {
>   if (!selectedClientType) return;
>   
>   try {
>     if (selectedClient) {
>       // Update
>       await clientService.updateClient(selectedClientType, selectedClient.id, data);
>     } else {
>       // Create
>       await clientService.createClient(selectedClientType, data);
>     }
>     loadClients();
>     setShowFormModal(false);
>   } catch (error) {
>     throw error; // Let form handle error
>   }
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
>         <h1 className="text-3xl font-bold text-gray-900">Clients</h1>
>         <p className="text-gray-600 mt-1">Manage your individual and business clients</p>
>       </div>
>       <Button onClick={handleAddClick}>
>         <PlusIcon className="h-5 w-5 mr-2" />
>         Add Client
>       </Button>
>     </div>
> 
>     {/* Stats */}
>     <ClientStats clients={clients} />
> 
>     {/* Filters */}
>     <ClientFilters
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
>     {!loading && filteredClients.length === 0 && (
>       <div className="text-center py-12">
>         <UsersIcon className="mx-auto h-12 w-12 text-gray-400" />
>         <h3 className="mt-2 text-sm font-medium text-gray-900">No clients found</h3>
>         <p className="mt-1 text-sm text-gray-500">Get started by creating a new client.</p>
>         <div className="mt-6">
>           <Button onClick={handleAddClick}>
>             <PlusIcon className="h-5 w-5 mr-2" />
>             Add Client
>           </Button>
>         </div>
>       </div>
>     )}
> 
>     {/* Grid View */}
>     {!loading && viewMode === 'grid' && filteredClients.length > 0 && (
>       <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 mt-6">
>         {filteredClients.map(client => (
>           <ClientCard
>             key={client.id}
>             client={client}
>             onEdit={handleEdit}
>             onDelete={handleDelete}
>           />
>         ))}
>       </div>
>     )}
> 
>     {/* Table View */}
>     {!loading && viewMode === 'table' && filteredClients.length > 0 && (
>       <div className="mt-6">
>         <ClientTable
>           clients={filteredClients}
>           onEdit={handleEdit}
>           onDelete={handleDelete}
>         />
>       </div>
>     )}
> 
>     {/* Modals */}
>     <ClientTypeModal
>       isOpen={showTypeModal}
>       onClose={() => setShowTypeModal(false)}
>       onSelectType={handleSelectType}
>     />
> 
>     {/* Type-specific form modals */}
>     {selectedClientType === 1 && (
>       <IndividualClientForm
>         isOpen={showFormModal}
>         onClose={() => setShowFormModal(false)}
>         client={selectedClient as IndividualClientDto}
>         onSubmit={handleFormSubmit}
>       />
>     )}
>     {selectedClientType === 2 && (
>       <BusinessClientForm
>         isOpen={showFormModal}
>         onClose={() => setShowFormModal(false)}
>         client={selectedClient as BusinessClientDto}
>         onSubmit={handleFormSubmit}
>       />
>     )}
> 
>     {/* Delete Confirmation */}
>     <ConfirmationDialog
>       isOpen={showDeleteDialog}
>       onClose={() => setShowDeleteDialog(false)}
>       onConfirm={confirmDelete}
>       title="Delete Client"
>       message={`Are you sure you want to delete this client? This action cannot be undone.`}
>       confirmLabel="Delete"
>       confirmVariant="danger"
>     />
>   </div>
> );
> ```
> 
> Import all components, services, icons
> Export as default

---

### Task 12: Create Client Detail Page (Optional)

**Location**: `src/pages/ClientDetailPage.tsx`

**Prompt for Copilot:**

> Create a placeholder ClientDetailPage component:
> 
> ```typescript
> import { useParams, useNavigate } from 'react-router-dom';
> import { Button } from '../components/common/Button';
> import { ArrowLeftIcon } from '@heroicons/react/24/outline';
> 
> export default function ClientDetailPage() {
>   const { id } = useParams<{ id: string }>();
>   const navigate = useNavigate();
> 
>   return (
>     <div className="p-6">
>       <Button 
>         variant="secondary" 
>         onClick={() => navigate('/clients')}
>         className="mb-6"
>       >
>         <ArrowLeftIcon className="h-5 w-5 mr-2" />
>         Back to Clients
>       </Button>
> 
>       <div className="bg-white rounded-lg shadow p-8 text-center">
>         <h1 className="text-2xl font-bold text-gray-900 mb-4">
>           Client Detail Page
>         </h1>
>         <p className="text-gray-600 mb-2">
>           Client ID: {id}
>         </p>
>         <p className="text-sm text-gray-500">
>           This page will show full client details in a future update.
>         </p>
>       </div>
>     </div>
>   );
> }
> ```
> 
> Export as default

---

### Task 13: Update Navigation

**Location**: `src/components/layout/Layout.tsx` or wherever navigation is defined

**Prompt for Copilot:**

> Add Clients navigation link to the sidebar/navbar:
> 
> ```typescript
> <NavLink to="/clients">
>   <UsersIcon className="h-5 w-5" />
>   <span>Clients</span>
> </NavLink>
> ```
> 
> Place after Products link, before other links

---

### Task 14: Update Routes

**Location**: `src/App.tsx` or `src/routes/index.tsx`

**Prompt for Copilot:**

> Add client routes to the application:
> 
> ```typescript
> // Import
> import ClientsPage from './pages/ClientsPage';
> import ClientDetailPage from './pages/ClientDetailPage';
> 
> // Add routes (inside protected routes)
> <Route path="/clients" element={<ClientsPage />} />
> <Route path="/clients/:id" element={<ClientDetailPage />} />
> ```
> 
> Place with other protected routes

---

## Verification Checklist

Before considering the Clients Module complete, verify:

### Types & Services:
- [ ] Client types defined (ClientType, DTOs, Request interfaces)
- [ ] ClientService created with all methods
- [ ] Formatters updated with client utilities
- [ ] API client configured for /api/clients endpoints

### Components:
- [ ] ClientCard displays correctly
- [ ] ClientTable shows all columns properly
- [ ] ClientFilters search works
- [ ] ClientFilters type filter works
- [ ] ClientFilters view toggle works
- [ ] ClientStats calculates correctly
- [ ] ClientTypeModal displays both types

### Forms:
- [ ] IndividualClientForm creates and edits
- [ ] IndividualClientForm validates correctly
- [ ] BusinessClientForm creates and edits
- [ ] BusinessClientForm validates NIPT (10 chars, alphanumeric)
- [ ] BusinessClientForm requires contact person
- [ ] Email validation works
- [ ] Phone validation works
- [ ] All forms show errors

### Clients Page:
- [ ] Clients load on mount
- [ ] Can click "Add Client"
- [ ] ClientTypeModal opens
- [ ] Selecting type opens correct form
- [ ] Can create client (both types)
- [ ] Can edit client (both types)
- [ ] Can delete client with confirmation
- [ ] Grid view displays correctly
- [ ] Table view displays correctly
- [ ] View toggle works
- [ ] Search filters clients
- [ ] Type filter works

### General:
- [ ] No TypeScript errors
- [ ] No console errors
- [ ] All API calls work
- [ ] Toast notifications appear
- [ ] Loading states work
- [ ] Empty states display
- [ ] Responsive on mobile
- [ ] Navigation link works
- [ ] Routes configured

---

## What You've Accomplished

✅ **Complete Client Management UI** for both types  
✅ **Type-specific forms** with proper validation  
✅ **Beautiful grid and table views**  
✅ **Search and filter functionality**  
✅ **Client statistics dashboard**  
✅ **Type selection modal** before creation  
✅ **Professional UI** with TailwindCSS  
✅ **Responsive design** for all screen sizes  
✅ **Proper error handling** with toast notifications  
✅ **Loading and empty states**  

---

## Key Patterns Used

### 1. Type-Specific Forms
Separate forms for Individual and Business with different fields.

### 2. Client Type Detection
Use clientTypeId to determine which form/display to show.

### 3. Search Functionality
Search across multiple fields (name, email, phone, NIPT).

### 4. Form Validation
- Required fields (firstName, lastName, NIPT, contact person)
- Format validation (email, phone, NIPT)
- Length constraints

### 5. Soft Delete
Clients are deactivated, not permanently deleted.

---

## Tips for Testing

1. **Test both client types** - Create Individual and Business clients
2. **Test NIPT validation** - Must be exactly 10 alphanumeric characters
3. **Test required fields** - Contact person required for Business
4. **Test email/phone validation** - Invalid formats should show errors
5. **Test search** - Search by name, email, phone, NIPT
6. **Test filters** - Filter by Individual/Business
7. **Test views** - Toggle between grid and table
8. **Test edit** - Edit both client types
9. **Test delete** - Verify confirmation dialog
10. **Check responsive** - Test on mobile screen sizes

---

## Important Notes

**Individual Clients:**
- Simple structure with first/last name
- All fields except names are optional

**Business Clients:**
- NIPT is required and unique (10 alphanumeric)
- Contact person first/last name required
- Owner information is optional
- More complex form with sections

**Validation:**
- Email: `/^[^\s@]+@[^\s@]+\.[^\s@]+$/`
- Phone: `/^[\d\s\+\-\(\)]+$/`
- NIPT: `/^[A-Za-z0-9]{10}$/`

Good luck! 🚀
