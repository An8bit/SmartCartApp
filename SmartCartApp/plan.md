# SmartCart API Documentation

## Overview
This document outlines all the API endpoints available in the SmartCart application, organized by controller.

## Authentication & User Management

### MembershipController
- `GET /api/Membership/current-state` - Get current membership tier state of the logged-in user
- `POST /api/Membership/update-tier` - Check and update user membership tier

### UserAdminController
- `GET /api/UserAdmin` - Get all users (Admin only)
- `GET /api/UserAdmin/{id}` - Get user by ID (Admin only)
- `POST /api/UserAdmin` - Create a new user (Admin only)
- `PUT /api/UserAdmin/{id}` - Update user information (Admin only)
- `DELETE /api/UserAdmin/{id}` - Delete a user (Admin only)
- `GET /api/UserAdmin/email/{email}` - Get user by email (Admin only)

### UserController
- `GET /api/User/Profile` - Get current user profile
- `GET /api/User/ProfileByEmail/{email}` - Get user profile by email
- `POST /api/User/Login` - User login
- `POST /api/User/Register` - User registration
- `POST /api/User/Logout` - User logout
- `PUT /api/User/UpdateAddress` - Update user address

## Products & Categories

### ProductsController
- `GET /api/Products/filter` - Get filtered products based on search criteria
- `GET /api/Products/{id}` - Get product by ID
- `GET /api/Products/category/{categoryId}` - Get products by category
- `POST /api/Products` - Create a new product
- `PUT /api/Products/{id}` - Update an existing product
- `DELETE /api/Products/{id}` - Delete a product
- `GET /api/Products/all` - Get all products

### CategoriesController
- `GET /api/Categories` - Get all categories
- `GET /api/Categories/{id}` - Get category by ID
- `POST /api/Categories` - Create a new category
- `PUT /api/Categories/{id}` - Update an existing category
- `DELETE /api/Categories/{id}` - Delete a category

## Shopping & Orders

### ShoppingCartController
- `POST /api/ShoppingCart` - Add item to cart
- `GET /api/ShoppingCart` - Get current cart
- `PUT /api/ShoppingCart/items/{cartItemId}` - Update cart item quantity
- `DELETE /api/ShoppingCart/items/{cartItemId}` - Remove item from cart
- `DELETE /api/ShoppingCart` - Clear cart

### OrderController
- `GET /api/Order` - Get current user's orders
- `GET /api/Order/{id}` - Get order by ID
- `POST /api/Order` - Create a new order from cart
- `PUT /api/Order/{id}/status` - Update order status
- `GET /api/Order/{id}/history` - Get order status history
- `GET /api/Order/admin` - Get all orders (Admin only)
- `GET /api/Order/statistics` - Get order statistics (Admin only)

## Payment

### PaymentController
- `POST /api/Payment/process` - Process payment
- `GET /api/Payment` - Get all payments
- `GET /api/Payment/{id}` - Get payment by ID
- `GET /api/Payment/order/{orderId}` - Get payments by order ID

## Authentication & Security

Many endpoints require authentication through cookies:
- Protected routes require a valid user authentication cookie
- Admin routes require admin role
- Shopping cart uses a unique session ID stored in cookies

## Response Formats

### Success Responses
- `200 OK` - Standard success response
- `201 Created` - Resource successfully created

### Error Responses
- `400 Bad Request` - Invalid input
- `401 Unauthorized` - Authentication required
- `403 Forbidden` - Insufficient permissions
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server-side error

## Next Steps
1. Review and document remaining controllers
2. Add request/response examples for each endpoint
3. Document authentication requirements for each endpoint
4. Add error response documentation 