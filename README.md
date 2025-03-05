# SmartCartApp

![C#](https://img.shields.io/badge/Language-C%23-brightgreen)
![.NET](https://img.shields.io/badge/Framework-.NET-blueviolet)
![Status](https://img.shields.io/badge/Status-In%20Development-yellow)
![License](https://img.shields.io/badge/License-MIT-blue)

SmartCartApp is a modern e-commerce platform built using **ASP.NET Core** for the backend and **Angular** for the frontend. This application aims to provide a robust, scalable, and user-friendly shopping experience for customers while offering powerful management tools for administrators.

## 🌟 Features

- **User Authentication:**
  - Secure login and registration
  - Role-based access control (admin and user)
  - JWT token-based authentication

- **Product Management:**
  - CRUD operations for products
  - Support for categories and subcategories
  - Image upload and management
  - Product search and filtering

- **Shopping Cart:**
  - Add, update, and remove products
  - Dynamic cart updates and total calculation
  - Save cart for later

- **Order Management:**
  - Place orders and track their statuses
  - Admin dashboard for order management
  - Order history for users

- **Payment Integration:**
  - Integration with third-party payment gateways
  - Support for multiple payment methods
  - Secure checkout process

- **Responsive Design:**
  - Fully functional across devices (mobile, tablet, desktop)
  - Modern UI/UX principles

## 🔧 Technologies Used

### Backend:
- ASP.NET Core 7.0+
- Entity Framework Core (EF Core) for database management
- SQL Server for data storage
- RESTful API architecture

### Frontend:
- Angular 16+ (TypeScript, RxJS)
- Angular Material for UI components
- SCSS for custom styling
- Responsive design principles

### Other Tools:
- JWT (JSON Web Token) for authentication
- Swagger for API documentation
- Docker for containerization
- CI/CD pipeline with GitHub Actions

## 🚀 Getting Started

### Prerequisites

1. Install **.NET SDK** (version 7.0 or later)
2. Install **Node.js** (version 16.x or later)
3. Install **Angular CLI** globally:
   ```bash
   npm install -g @angular/cli
   ```
4. Set up **SQL Server** for database
5. Optional: Install **Docker** for containerized deployment

### Setup Instructions

#### Backend (ASP.NET Core):

1. Navigate to the backend directory:
   ```bash
   cd SmartCartApp/Backend
   ```
2. Restore dependencies:
   ```bash
   dotnet restore
   ```
3. Update database (EF Core migrations):
   ```bash
   dotnet ef database update
   ```
4. Run the application:
   ```bash
   dotnet run
   ```
   The API will be available at `https://localhost:5001`

#### Frontend (Angular):

1. Navigate to the frontend directory:
   ```bash
   cd SmartCartApp/Frontend
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Start the development server:
   ```bash
   ng serve
   ```
   The application will be available at `http://localhost:4200`

## 📚 API Documentation

API documentation is available via Swagger:
- URL: `https://localhost:5001/swagger`

## 🔄 Development Workflow

1. Create feature branches from `develop`
2. Submit pull requests to `develop`
3. Releases are managed through `main` branch
4. Use semantic versioning for releases

## 🐳 Deployment

### Docker (Optional):

1. Build and run the Docker containers:
   ```bash
   docker-compose up --build
   ```
2. Access the application:
   - Backend: `https://localhost:5001`
   - Frontend: `http://localhost:4200`

## 🤝 Contributing

1. Fork the repository
2. Create a new feature branch:
   ```bash
   git checkout -b feature/your-feature-name
   ```
3. Commit your changes:
   ```bash
   git commit -m "Add your message here"
   ```
4. Push to the branch:
   ```bash
   git push origin feature/your-feature-name
   ```
5. Create a pull request

## 📋 Project Structure

```
SmartCartApp/
├── Backend/                # ASP.NET Core API
│   ├── Controllers/        # API controllers
│   ├── Models/             # Domain models
│   ├── Services/           # Business logic
│   ├── Data/               # EF Core context and migrations
│   └── Program.cs          # Application entry point
│
├── Frontend/               # Angular application
│   ├── src/
│   │   ├── app/           # Core application code
│   │   ├── assets/        # Static assets
│   │   └── environments/  # Environment configurations
│   └── package.json       # Dependencies
│
├── docker-compose.yml      # Docker configuration
└── README.md               # This file
```

## 📝 License

This project is licensed under the [MIT License](LICENSE).

## 📞 Contact

- Developer: [An8bit](https://github.com/An8bit)
- Project Repository: [https://github.com/An8bit/SmartCartApp](https://github.com/An8bit/SmartCartApp)

---

© 2025 SmartCartApp. All Rights Reserved.
