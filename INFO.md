# RisingSigma Training Plan Application

## Project Overview

RisingSigma is a full-stack training plan application designed to simplify the creation and management of workout plans. The project serves as a modern, user-friendly alternative to Excel-based workout tracking, providing a comprehensive solution for fitness enthusiasts and trainers.

## Architecture

The application follows a modern microservices architecture with the following components:

### Backend (.NET Core API)

- **Technology**: C# with .NET Core
- **Architecture**: RESTful API with Entity Framework Core
- **Database**: Microsoft SQL Server
- **Features**:
  - Exercise management
  - User authentication (in progress)
  - Training plan creation and management
  - Data persistence with Entity Framework

### Frontend (Ionic + Angular)

- **Technology**: Ionic Framework with Angular 18
- **Features**:
  - Responsive design for mobile and web
  - Modern UI components
  - Cross-platform compatibility
  - Progressive Web App (PWA) capabilities

### Database

- **Technology**: Microsoft SQL Server Express
- **Features**:
  - Entity Framework Core migrations
  - Structured data storage for users, exercises, and training plans

## Project Structure

```
/
├── Backend/                     # .NET Core API
│   └── RisingSigma/             # Solution folder
│       ├── RisingSigma.API/     # Main API project
│       │   ├── Controllers/     # API controllers
│       │   ├── Logic/           # Business logic layer
│       │   └── Properties/      # Project properties
│       ├── RisingSigma.Database/ # Database layer with Entity Framework
│       │   ├── Entities/        # Database entities
│       │   └── Migrations/      # EF Core migrations
│       ├── RisingSigma.Api.Test/ # Unit tests
│       └── RisingSigma.sln      # Solution file
├── IONIC/                       # Ionic Angular frontend
│   └── src/app/                 # Angular application source
│       ├── account/             # Account management
│       ├── header/              # Header component
│       ├── home/                # Home page
│       ├── plan/                # Training plans
│       ├── settings/            # Settings page
│       ├── shared/              # Shared components
│       ├── tabs/                # Tab navigation
│       ├── workout/             # Workout features
│       └── workout-table/       # Workout table component
├── db/                          # Database Docker configuration
├── .devcontainer/               # Development container configuration
├── .vscode/                     # VS Code configuration
├── docker-compose-build.yml     # Docker Compose for local build
└── docker-compose-hub.yml       # Docker Compose for Docker Hub images
```

## Getting Started

### Prerequisites

- Docker and Docker Compose
- Node.js 18+ (for local development)
- .NET 8 SDK (for local development)

### Environment Setup

1. **Clone the repository**

   ```bash
   git clone <repository-url>
   cd risingsigma
   ```

2. **Create environment file**
   Create a `.env` file in the project root:

   ```env
   # Database Configuration
   DB_PASSWORD=YourSecurePassword123!
   DB_NAME=RisingSigma

   # Docker Hub Configuration
   DOCKER_HUB_USER=dukq

   # Application Ports
   API_PORT=5000
   FRONTEND_PORT=8080
   ADMINER_PORT=8090
   DB_PORT=1433

   # Development Ports (for .devcontainer)
   DEV_FRONTEND_PORT=4200
   IONIC_PORT=8100

   # Container Names (optional, defaults provided)
   DB_CONTAINER_NAME=rs-database
   BACKEND_CONTAINER_NAME=rs-backend
   FRONTEND_CONTAINER_NAME=rs-frontend
   ADMINER_CONTAINER_NAME=rs-adminer
   ```

## Running with Docker Compose

### Option 1: Local Build (Recommended for Development)

Build and run all services locally:

```bash
# Start all services (database, backend, frontend)
docker-compose -f docker-compose-build.yml up -d

# View logs
docker-compose -f docker-compose-build.yml logs -f

# Stop services
docker-compose -f docker-compose-build.yml down
```

**Services will be available at:**

- Frontend (Ionic): http://localhost:${FRONTEND_PORT} (default: 8080)
- Backend API: http://localhost:${API_PORT} (default: 5000)
- Database: localhost:${DB_PORT} (default: 1433)
- Adminer (Database UI): http://localhost:${ADMINER_PORT} (default: 8090)

### Option 2: Docker Hub Images

Use pre-built images from Docker Hub:

**Important:** Users need to create their own `.env` file with their own database password:

```bash
# 1. Create your own .env file (copy from .env.example)
cp .env.example .env

# 2. Edit .env and set your own secure password
# Example .env content:
# DB_PASSWORD=MySecurePassword123!
# DOCKER_HUB_USER=dukq
# (other settings as needed)

# 3. Pull and run images from Docker Hub
docker-compose -f docker-compose-hub.yml up -d

# Stop services
docker-compose -f docker-compose-hub.yml down
```

**Security Note:** The database password is **NOT** included in the Docker images. Each user sets their own password via the `.env` file.

### Database Management

The SQL Server database includes:

- Automatic health checks
- Data persistence with Docker volumes
- Entity Framework migrations applied on startup

## Development with Dev Containers

The project includes a complete Visual Studio Code Dev Container setup for seamless development.

### Prerequisites for Dev Container Development

- Visual Studio Code
- Docker Desktop
- Dev Containers extension for VS Code

### Starting Development Environment

1. **Open in VS Code**

   ```bash
   code .
   ```

2. **Reopen in Container**
   - VS Code will detect the `.devcontainer` configuration
   - Click "Reopen in Container" when prompted
   - Or use Command Palette: `Dev Containers: Reopen in Container`

### Dev Container Features

The development environment includes:

**Pre-installed Extensions:**

- C# Dev Kit
- MS SQL Server extension
- Angular Language Service
- Ionic extension
- Prettier, ESLint
- Tailwind CSS IntelliSense

**Pre-configured Services:**

- Full development workspace with all dependencies
- SQL Server database with development data
- Port forwarding for all services
- Automatic npm install for frontend dependencies

**Development Ports:**

- Frontend Dev Server: http://localhost:${DEV_FRONTEND_PORT} (default: 4200)
- Backend API: http://localhost:${API_PORT} (default: 5000) / https://localhost:5001
- Ionic Dev Server: http://localhost:${IONIC_PORT} (default: 8100)
- Database: localhost:${DB_PORT} (default: 1433)
- Adminer (Database UI): http://localhost:${ADMINER_PORT} (default: 8090)

### Development Workflow

1. **Backend Development**

   **For Debugging (Recommended):**

   - Use `Ctrl+Shift+P` → `Debug: Start Debugging` or press `F5`
   - Select ".NET Core Launch (web)" configuration
   - This starts the API with debugging capabilities

   **Alternative - Command Line:**

   ```bash
   # Navigate to backend directory
   cd Backend/RisingSigma

   # Restore dependencies
   dotnet restore

   # Run the API
   dotnet run --project RisingSigma.API

   # Or use the VS Code task: "watch"
   ```

2. **Frontend Development**

   ```bash
   # Navigate to frontend directory
   cd IONIC

   # Install dependencies (done automatically in dev container)
   npm install

   # Start development server
   npm start

   # Or for Ionic serve
   ionic serve
   ```

3. **Database Operations**

   ```bash
   # Add new migration
   cd Backend/RisingSigma
   dotnet ef migrations add MigrationName --project RisingSigma.Database --startup-project RisingSigma.API

   # Update database
   dotnet ef database update --project RisingSigma.Database --startup-project RisingSigma.API
   ```

4. **Database Management with SQL Server Extension**

   The dev container includes the **MS SQL Server Extension** (`ms-mssql.mssql`) for direct database access within VS Code.

   **Setting up the connection:**

   1. Open the **SQL Server** panel in VS Code sidebar
   2. Click **"Add Connection"**
   3. Enter connection details:
      - **Server**: `localhost` (or `database` from within container)
      - **Port**: `1433`
      - **Authentication**: `SQL Login`
      - **Username**: `sa`
      - **Password**: Your `DB_PASSWORD` from `.env` file
      - **Database**: `RisingSigma`

   **Features available:**

   - Browse and view database tables
   - Execute SQL queries directly in VS Code
   - Explore database schema and relationships
   - IntelliSense support for SQL queries
   - View query results in integrated panels
   - Manage stored procedures and functions

   **Alternative**: Use Adminer web interface at http://localhost:8090

## Available VS Code Tasks

The project includes pre-configured VS Code tasks:

- **build**: Build the .NET API
- **publish**: Publish the .NET API
- **watch**: Run the API with hot reload

Access tasks via: `Terminal` → `Run Task` or `Ctrl+Shift+P` → `Tasks: Run Task`

## API Endpoints

### Exercise Management

- `GET /api/exercise` - Get all exercises
- `POST /api/exercise` - Create new exercise
- `PUT /api/exercise/{id}` - Update exercise
- `DELETE /api/exercise/{id}` - Delete exercise

### Health Check

- `GET /health` - API health status

## Technology Stack

### Backend

- **.NET 8**: Latest LTS version of .NET
- **Entity Framework Core**: ORM for database operations
- **SQL Server Express**: Database engine
- **ASP.NET Core**: Web API framework

### Frontend

- **Angular 18**: Latest Angular framework
- **Ionic 8**: Mobile-first UI framework
- **TypeScript**: Type-safe JavaScript
- **SCSS**: Enhanced CSS with variables and mixins

### DevOps & Development

- **Docker**: Containerization
- **Docker Compose**: Multi-container orchestration
- **Dev Containers**: Consistent development environment
- **VS Code**: Primary development IDE

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes in the dev container environment
4. Test your changes locally with Docker Compose
5. Submit a pull request

## Troubleshooting

### Common Issues

1. **Database Connection Issues**

   - Ensure the database container is healthy: `docker-compose logs database`
   - Check that the DB_PASSWORD environment variable is set correctly

2. **Port Conflicts**

   - Ensure ports 1433, 4200, 5000, 5001, 8080, 8090, and 8100 are not in use
   - Modify port mappings in docker-compose files if needed

3. **Frontend Build Issues**

   - Clear node_modules: `rm -rf IONIC/node_modules && npm install`
   - Check Node.js version compatibility

4. **Backend Build Issues**
   - Clean and restore: `dotnet clean && dotnet restore`
   - Check .NET SDK version

### Logs and Debugging

```bash
# View all service logs
docker-compose -f docker-compose-build.yml logs

# View specific service logs
docker-compose -f docker-compose-build.yml logs backend
docker-compose -f docker-compose-build.yml logs database
docker-compose -f docker-compose-build.yml logs frontend

# Follow logs in real-time
docker-compose -f docker-compose-build.yml logs -f
```

## License

This project is part of an academic project work and is intended for educational purposes.

## Environment Variables Configuration

All Docker Compose files support environment variables for flexible configuration. **Each user sets their own environment variables** - no sensitive data is included in the Docker images.

### **How It Works:**
1. Copy `.env.example` to `.env` 
2. Set your own secure `DB_PASSWORD` and other preferences
3. Docker Compose reads your `.env` file and applies the settings
4. The database container uses **your** password, not a hardcoded one

### **Required Variables:**

- `DB_PASSWORD` - Database password (required)
- `DOCKER_HUB_USER` - Your Docker Hub username (for hub deployment)

### **Optional Variables (with defaults):**

- `DB_NAME` - Database name (default: RisingSigma)
- `API_PORT` - Backend API port (default: 5000)
- `FRONTEND_PORT` - Frontend port (default: 8080)
- `ADMINER_PORT` - Database UI port (default: 8090)
- `DB_PORT` - Database port (default: 1433)
- `DEV_FRONTEND_PORT` - Development frontend port (default: 4200)
- `IONIC_PORT` - Ionic dev server port (default: 8100)

### **Container Names (optional):**

- `DB_CONTAINER_NAME` - Database container (default: rs-database)
- `BACKEND_CONTAINER_NAME` - Backend container (default: rs-backend)
- `FRONTEND_CONTAINER_NAME` - Frontend container (default: rs-frontend)
- `ADMINER_CONTAINER_NAME` - Adminer container (default: rs-adminer)

**Note:** Copy `.env.example` to `.env` and customize as needed.

## Frequently Asked Questions (FAQ)

### **Q: Do I need to know the original database password to use the Docker Hub images?**
**A:** No! Each user sets their own database password in their `.env` file. The password is not hardcoded in the Docker images.

### **Q: What happens if I don't set a DB_PASSWORD?**
**A:** Docker Compose will fail to start because the database container requires a password. You must create a `.env` file with `DB_PASSWORD=YourPassword`.

### **Q: Can I change the ports if they conflict with my system?**
**A:** Yes! Set different ports in your `.env` file:
```env
API_PORT=3000        # Instead of default 5000
FRONTEND_PORT=3001   # Instead of default 8080
```

### **Q: Are the container names configurable?**
**A:** Yes! You can change container names via environment variables to avoid conflicts:
```env
DB_CONTAINER_NAME=my-custom-db
BACKEND_CONTAINER_NAME=my-custom-api
```
