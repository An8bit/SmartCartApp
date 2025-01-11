# SmartCartApp

SmartCartApp is a modern e-commerce platform built using **ASP.NET Core** for the backend and **Angular** for the frontend. This application aims to provide a robust, scalable, and user-friendly shopping experience.

## Features

- **User Authentication:**
  - Secure login and registration.
  - Role-based access control (admin and user).

- **Product Management:**
  - CRUD operations for products.
  - Support for categories and subcategories.

- **Shopping Cart:**
  - Add, update, and remove products.
  - Dynamic cart updates and total calculation.

- **Order Management:**
  - Place orders and track their statuses.
  - Admin dashboard for order management.

- **Payment Integration:**
  - Integration with third-party payment gateways.
  - Support for multiple payment methods.

- **Responsive Design:**
  - Fully functional across devices (mobile, tablet, desktop).

## Technologies Used

### Backend:
- ASP.NET Core
- Entity Framework Core (EF Core) for database management
- SQL Server for data storage

### Frontend:
- Angular (TypeScript, RxJS)
- Angular Material for UI components
- SCSS for custom styling

### Other Tools:
- JWT (JSON Web Token) for authentication
- Swagger for API documentation
- Docker for containerization

## Getting Started

### Prerequisites

1. Install **.NET SDK** (version 7.0 or later).
2. Install **Node.js** (version 16.x or later).
3. Install **Angular CLI** globally:
   ```bash
   npm install -g @angular/cli
   ```
4. Set up **SQL Server** for database.
5. Optional: Install **Docker** for containerized deployment.

### Setup Instructions

#### Backend (ASP.NET Core):

1. Navigate to the backend directory:
   ```bash
   cd backend
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
   The API will be available at `https://localhost:5001`.

#### Frontend (Angular):

1. Navigate to the frontend directory:
   ```bash
   cd frontend
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Start the development server:
   ```bash
   ng serve
   ```
   The application will be available at `http://localhost:4200`.

## API Documentation

API documentation is available via Swagger:
- URL: `https://localhost:5001/swagger`

## Deployment

### Docker (Optional):

1. Build and run the Docker containers:
   ```bash
   docker-compose up --build
   ```
2. Access the application:
   - Backend: `https://localhost:5001`
   - Frontend: `http://localhost:4200`

## Contributing

1. Fork the repository.
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
5. Create a pull request.

## License

This project is licensed under the [MIT License](LICENSE).
