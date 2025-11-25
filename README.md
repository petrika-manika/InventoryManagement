# Inventory Management System

A comprehensive inventory management system built with **Clean Architecture**, **.NET 8**, **React**, **TypeScript**, and **TailwindCSS**.

---

## ?? **Features**

### **Backend (.NET 8 Web API)**
- ? **Clean Architecture** - Domain, Application, Infrastructure, API layers
- ? **CQRS Pattern** - Using MediatR for commands and queries
- ? **Entity Framework Core** - Code-first with migrations
- ? **JWT Authentication** - Secure token-based authentication
- ? **FluentValidation** - Input validation
- ? **RESTful API** - Following REST best practices
- ? **Swagger/OpenAPI** - Interactive API documentation
- ? **Exception Handling** - Global exception middleware
- ? **Unit & Integration Tests** - Comprehensive test coverage

### **Modules**
1. **User Management**
   - User CRUD operations
   - Authentication (Login/Logout)
   - JWT token generation
   - Password hashing with BCrypt

2. **Inventory Management**
   - 5 Product types:
     - Aroma Bombel
     - Aroma Bottle
     - Aroma Device
     - Sanitizing Device
     - Battery
   - Stock management (Add/Remove stock)
   - Stock history tracking
   - Low stock alerts
   - Product filtering by type

3. **Client Management**
   - Individual clients
   - Business clients (with NIPT validation)
   - Client CRUD operations
   - Search functionality
   - Soft delete support

### **Frontend (React + TypeScript)**
- ? **React 18** - Modern React with hooks
- ? **TypeScript** - Type-safe development
- ? **TailwindCSS** - Utility-first CSS framework
- ? **React Router** - Client-side routing
- ? **Axios** - HTTP client
- ? **React Hook Form** - Form management
- ? **Context API** - State management

---

## ??? **Architecture**

### **Clean Architecture Layers**

```
???????????????????????????????????????????????
?            Presentation (API)               ?
?  - Controllers                              ?
?  - Middleware (Exception Handling)          ?
?  - Swagger Configuration                    ?
???????????????????????????????????????????????
                    ?
???????????????????????????????????????????????
?         Infrastructure Layer                ?
?  - EF Core DbContext                        ?
?  - Entity Configurations                    ?
?  - Migrations                               ?
?  - Services (Password, JWT, CurrentUser)    ?
???????????????????????????????????????????????
                    ?
???????????????????????????????????????????????
?          Application Layer                  ?
?  - Commands & Queries (CQRS)                ?
?  - MediatR Handlers                         ?
?  - FluentValidation Validators              ?
?  - DTOs                                     ?
?  - Mappers                                  ?
???????????????????????????????????????????????
                    ?
???????????????????????????????????????????????
?           Domain Layer                      ?
?  - Entities (User, Product, Client)         ?
?  - Value Objects (Email, Money, NIPT)       ?
?  - Enums                                    ?
?  - Domain Events                            ?
?  - Domain Exceptions                        ?
???????????????????????????????????????????????
```

---

## ?? **Prerequisites**

- **.NET 8 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Node.js 18+** - [Download](https://nodejs.org/)
- **SQL Server** (LocalDB or full instance)
- **Visual Studio 2022** or **VS Code**
- **Git**

---

## ??? **Getting Started**

### **1. Clone the Repository**

```bash
git clone https://github.com/YOUR_USERNAME/InventoryManagement.git
cd InventoryManagement
```

### **2. Backend Setup**

#### **a. Restore NuGet Packages**

```bash
dotnet restore
```

#### **b. Update Connection String**

Edit `src/Presentation/InventoryManagement.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InventoryManagementDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

#### **c. Apply Database Migrations**

```bash
cd src/Infrastructure/InventoryManagement.Infrastructure
dotnet ef database update --startup-project ../../Presentation/InventoryManagement.API
```

This will create the database and seed the default admin user:
- **Email:** `admin@inventoryapp.com`
- **Password:** `Admin@123`

#### **d. Run the API**

```bash
cd ../../Presentation/InventoryManagement.API
dotnet run
```

API will be available at:
- **HTTPS:** `https://localhost:7xxx`
- **HTTP:** `http://localhost:5xxx`
- **Swagger:** `https://localhost:7xxx/swagger`

### **3. Frontend Setup**

#### **a. Navigate to Web Project**

```bash
cd src/Presentation/InventoryManagement.Web
```

#### **b. Install Dependencies**

```bash
npm install
```

#### **c. Configure API URL**

Edit `src/utils/apiClient.ts` if needed (default: `https://localhost:7xxx`)

#### **d. Run the App**

```bash
npm run dev
```

Frontend will be available at: `http://localhost:5173`

---

## ?? **Running Tests**

### **Backend Tests**

```bash
# Run all tests
dotnet test

# Run specific project tests
dotnet test tests/InventoryManagement.Domain.Tests
dotnet test tests/InventoryManagement.Application.Tests
dotnet test tests/InventoryManagement.API.Tests
```

### **Test Coverage**
- Domain Layer: Unit tests for entities and value objects
- Application Layer: Unit tests for commands and queries
- API Layer: Integration tests for controllers

---

## ?? **Project Structure**

```
InventoryManagement/
??? src/
?   ??? Core/
?   ?   ??? InventoryManagement.Domain/          # Domain entities, value objects
?   ?   ??? InventoryManagement.Application/     # CQRS, MediatR, DTOs
?   ??? Infrastructure/
?   ?   ??? InventoryManagement.Infrastructure/  # EF Core, migrations, services
?   ??? Presentation/
?       ??? InventoryManagement.API/             # Web API controllers
?       ??? InventoryManagement.Web/             # React frontend
??? tests/
?   ??? InventoryManagement.Domain.Tests/
?   ??? InventoryManagement.Application.Tests/
?   ??? InventoryManagement.API.Tests/
??? docs/                                        # Documentation files
```

---

## ?? **Authentication**

### **Login**

**Endpoint:** `POST /api/auth/login`

**Request:**
```json
{
  "email": "admin@inventoryapp.com",
  "password": "Admin@123"
}
```

**Response:**
```json
{
  "user": {
    "id": "guid",
    "firstName": "System",
    "lastName": "Administrator",
    "email": "admin@inventoryapp.com"
  },
  "token": "eyJhbGc..."
}
```

### **Using JWT Token**

Add the token to request headers:
```
Authorization: Bearer {your-token}
```

---

## ?? **API Endpoints**

### **Authentication**
- `POST /api/auth/login` - Login

### **Users**
- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get user by ID
- `GET /api/users/me` - Get current user
- `POST /api/users` - Create user
- `PUT /api/users/{id}` - Update user
- `PATCH /api/users/{id}/activate` - Activate user
- `PATCH /api/users/{id}/deactivate` - Deactivate user

### **Products**
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `GET /api/products/type/{typeId}` - Get products by type
- `GET /api/products/low-stock` - Get low stock products
- `POST /api/products/aroma-bombel` - Create Aroma Bombel
- `POST /api/products/aroma-bottle` - Create Aroma Bottle
- `POST /api/products/aroma-device` - Create Aroma Device
- `POST /api/products/sanitizing-device` - Create Sanitizing Device
- `POST /api/products/battery` - Create Battery
- `PUT /api/products/{type}/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product

### **Stock**
- `POST /api/stock/add` - Add stock
- `POST /api/stock/remove` - Remove stock
- `GET /api/stock/history` - Get stock history
- `GET /api/stock/history/{productId}` - Get product stock history

### **Clients**
- `GET /api/clients` - Get all clients
- `GET /api/clients/{id}` - Get client by ID
- `GET /api/clients/type/{typeId}` - Get clients by type
- `GET /api/clients/search` - Search clients
- `POST /api/clients/individual` - Create individual client
- `POST /api/clients/business` - Create business client
- `PUT /api/clients/individual/{id}` - Update individual client
- `PUT /api/clients/business/{id}` - Update business client
- `DELETE /api/clients/{id}` - Delete client (soft delete)

---

## ??? **Database Schema**

### **Users Table**
- Id (Guid)
- FirstName, LastName, Email
- PasswordHash
- IsActive
- CreatedAt, UpdatedAt

### **Products Table (TPH)**
- Id (Guid)
- ProductType (Discriminator)
- Name, Description
- Price, Currency
- StockQuantity
- Type-specific fields (Taste, Color, PlugType, etc.)

### **StockHistories Table**
- Id (Guid)
- ProductId
- QuantityChanged
- QuantityAfter
- ChangeType, Reason
- ChangedBy, ChangedAt

### **Clients Table (TPH)**
- Id (string)
- ClientType (Discriminator)
- Common fields (Address, Email, Phone)
- Individual-specific (FirstName, LastName)
- Business-specific (NIPT, Owner, ContactPerson)

---

## ?? **Key Design Patterns**

1. **Clean Architecture** - Separation of concerns
2. **CQRS** - Command Query Responsibility Segregation
3. **Mediator Pattern** - Decoupling with MediatR
4. **Repository Pattern** - DbContext as repository
5. **Factory Pattern** - Entity creation methods
6. **Value Object Pattern** - Email, Money, NIPT
7. **Table Per Hierarchy (TPH)** - Products and Clients inheritance

---

## ?? **Technologies Used**

### **Backend**
- .NET 8
- Entity Framework Core 8
- MediatR
- FluentValidation
- BCrypt.Net
- JWT Bearer Authentication
- Swagger/Swashbuckle
- xUnit, Moq, FluentAssertions

### **Frontend**
- React 18
- TypeScript
- Vite
- TailwindCSS
- React Router
- Axios
- React Hook Form

---

## ?? **Development Guidelines**

### **Backend**
- Follow Clean Architecture principles
- Use CQRS for all business operations
- Validate input with FluentValidation
- Follow SOLID principles
- Write unit tests for domain logic
- Write integration tests for API endpoints

### **Frontend**
- Use TypeScript for type safety
- Follow React best practices (hooks, functional components)
- Use TailwindCSS for styling
- Handle errors gracefully
- Show loading states

---

## ?? **Roadmap**

- [ ] Add role-based authorization
- [ ] Implement real-time notifications (SignalR)
- [ ] Add file upload for product images
- [ ] Create reports module
- [ ] Add dashboard with charts
- [ ] Implement caching (Redis)
- [ ] Add logging (Serilog)
- [ ] Deploy to Azure

---

## ?? **Contributing**

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## ?? **License**

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## ?? **Author**

**Petrika Manika**
- GitHub: [@petrika-manika](https://github.com/petrika-manika)

---

## ?? **Support**

If you have any questions or issues, please:
1. Check the [Documentation](docs/)
2. Open an [Issue](https://github.com/YOUR_USERNAME/InventoryManagement/issues)
3. Contact the author

---

**Happy Coding! ??**
