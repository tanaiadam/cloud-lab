# Photo Album 📸

Photo Album is a secure, full-stack personal photo management application. It features a modern Angular 21 frontend and a .NET 10 backend utilizing Azure cloud services for storage and database management and Vercel for Static Web Hosting (SPA),

---

## 🚀 Technology Stack & Versions

### Frontend
- **Framework:** Angular 21
- **State Management:** Unidirectional flow with **Angular Signals**
- **Form Handling:** Reactive Forms with strict validation
- **Styling:** SCSS with shared variables and mobile-responsive layout
- **Deployment:** Vercel

### Backend
- **Framework:** .NET 10.0 (ASP.NET Core API)
- **Database ORM:** Entity Framework Core
- **Authentication:** JWT (JSON Web Tokens) with Bearer scheme
- **Architecture:** Layered (API -> BLL -> DAL)

### Infrastructure (Azure)
- **API Hosting:** Azure App Service (Linux)
- **Database:** Azure SQL
- **Storage:** Azure Blob Storage

---

## 🏗️ System Architecture

### Frontend Architecture
The application uses a **Store Pattern** to centralize authentication state.
- **AuthStore:** Uses `signal` and `computed` for reactive UI updates.
- **Guards:** Functional guards protect routes from unauthorized access.
- **Interceptors:** Automatically attaches the JWT to every HTTP request.



### Backend Architecture
The API enforces **Multi-tenant Isolation**. Every request extracts the `UserId` from the JWT claims, ensuring users can only View, Upload, or Delete their own photos.
- **API:** Handles CORS, Authentication, and Controller logic.
- **BLL:** Manages Azure Blob stream uploads and business rules.
- **DAL:** Scopes all EF Core queries to the authenticated `Guid UserId`.



---

## 🔐 Local Development & Security

To run the project locally, you must configure your secrets. **Do not hardcode connection strings or keys in `appsettings.json`.**

### Setting up User Secrets (Backend)
Navigate to the `PhotoAlbum.Api` project folder and run the following commands to initialize and set your secrets:

```bash
# Initialize User Secrets
dotnet user-secrets init

# Set the Database Connection String
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=tcp:your-server.database.windows.net...;User ID=your-user;Password=your-password;"

# Set the JWT Secret Key (32 characters)
dotnet user-secrets set "Jwt:Key" "YourSuperSecretSecurityKey1234567890!"

# Set Azure Storage Connection
dotnet user-secrets set "AzureStorage:ConnectionString" "DefaultEndpointsProtocol=https;AccountName=youraccount;..."
