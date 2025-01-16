# Project Documentation: Training Plan App

## Overview
The Training Plan App aims to simplify creating and managing workout plans, providing a user-friendly alternative to Excel. The project combines a C# backend with an Ionic frontend using Angular.

---

## Table of Contents
- [Features](#features)
- [Project Structure](#project-structure)
  - [Backend (C#)](#backend-c)
  - [Frontend (Ionic + Angular)](#frontend-ionic-angular)
- [Dependencies](#dependencies)
- [Setup Instructions](#setup-instructions)

---

## Features
1. **Customizable Workout Plans**: Create, edit, and manage workout plans efficiently.
2. **Responsive Design**: Works seamlessly across devices.
3. **User Authentication**: Secure login and registration system.
4. **Data Persistence**: Stores user plans in a structured and accessible manner.

---

## Project Structure

### Backend (C#)
The backend provides APIs for managing user authentication, workout plans, and other core functionalities.

#### Key Directories
- **Controllers**: Handles API endpoints.
- **Models**: Defines the data structures.
- **Services**: Contains business logic.
- **Database**: Manages data storage and retrieval.

#### Example Structure
```
Backend/
├── Controllers/
│   ├── AuthenticationController.cs
│   └── WorkoutPlanController.cs
├── Models/
│   ├── User.cs
│   └── WorkoutPlan.cs
├── Services/
│   ├── AuthenticationService.cs
│   └── WorkoutPlanService.cs
└── Database/
       └── ApplicationDbContext.cs
```

### Frontend (Ionic + Angular)
The frontend is responsible for delivering an interactive user experience. It leverages Angular for state management and Ionic for UI components.

#### Key Directories
- **Pages**: Defines the app views.
- **Components**: Reusable UI elements.
- **Services**: Handles communication with the backend.
- **Assets**: Stores static files like images and styles.

#### Example Structure
```
Frontend/
├── src/
    ├── app/
    │   ├── pages/
    │   │   ├── login/
    │   │   └── dashboard/
    │   ├── components/
    │   │   └── navbar/
    │   ├── services/
    │   │   └── api.service.ts
    │   ├── assets/
    │   └── environments/
    └── index.html
```

---

## Dependencies

### Backend
Key dependencies listed in the C# project file:
- **Microsoft.EntityFrameworkCore**: Database management.
- **Microsoft.AspNetCore.Identity**: Authentication and authorization.

### Frontend
Dependencies as specified in `package.json`:
- **@angular/core**: Angular framework.
- **@ionic/angular**: Ionic components.
- **rxjs**: Reactive programming library.
- **angularfire**: Firebase integration (optional).

---

## Setup Instructions

### Backend
```bash
# Install .NET SDK
# Clone the repository
# Navigate to the Backend directory
cd Backend

# Restore dependencies
dotnet restore

# Start the backend server
dotnet run
```

### Frontend
```bash
# Install Node.js and npm
# Navigate to the Frontend directory
cd Frontend

# Install dependencies
npm install

# Start the development server with SCSS validation
npm start
```

### Installing Ionic (if needed)
```bash
# Install Ionic CLI globally
npm install -g @ionic/cli

# Verify the installation
ionic --version
```

The project is already set up, so there's no need to create a new Ionic project. Use `npm start` to launch the existing app and run SCSS validation as per the project’s guidelines.

---

## Future Improvements

### Planned Enhancements
1. **Third-Party API Integration**: Connect with external fitness APIs to provide users with a broader range of data and insights.
2. **Advanced Analytics**: Introduce tools to help users track and visualize their progress effectively.
3. **Multi-Language Support**: Expand language options, including English and German, to make the app accessible to a wider audience.
4. **Offline Functionality**: Enable access to workout plans even without an internet connection.

---

This documentation serves as a foundational guide for developers to understand and contribute to the project. For detailed implementation notes, refer to the respective `README` files in the Backend and Frontend directories.

