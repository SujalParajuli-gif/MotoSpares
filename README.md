# MotoSpares

MotoSpares is a Vehicle Parts Selling and Inventory Management System developed as part of the CS6004NI Application Development coursework. The system is designed for vehicle service centers and vehicle parts retail businesses to manage daily operations more efficiently through a role-based digital platform.

The application focuses on inventory handling, customer service, sales processing, vendor management, reporting, and service-related activities in a single system. It supports three main user roles: **Admin**, **Staff**, and **Customer**.

## Project Overview

The purpose of this project is to build a practical management system that helps streamline the operation of a vehicle parts and service business. Traditional manual processes can lead to stock issues, difficulty in tracking purchases, delayed reporting, and weak customer follow-up. MotoSpares addresses these problems by providing a structured web-based solution for handling business activities more accurately and efficiently.

The project is developed using **C#.NET** for the backend and is structured to support clear separation of concerns, maintainability, and future scalability.

## Core Functionalities

### Admin
- Generate and view financial reports on a daily, monthly, and yearly basis
- Manage staff registration and role assignments
- Manage vehicle parts, including adding, updating, and removing records
- Create purchase invoices for stock updates
- Manage vendor details
- Receive low-stock alerts when product quantity falls below the defined threshold

### Staff
- Register new customers along with their vehicle details
- Sell vehicle parts and create sales invoices
- View customer details, purchase history, and vehicle information
- Search customers by vehicle number, phone number, customer ID, or name
- Generate customer-related reports
- Send invoices to customers by email

### Customer
- Register and manage profile details
- Manage vehicle details
- Book service appointments
- Request unavailable parts
- Submit service reviews
- View purchase and service history

### System Features
- Automatic low-stock notification support
- Credit reminder support for overdue payments
- Loyalty discount logic for eligible purchases

## Project Structure

This project is managed in a single repository with separate folders for backend, frontend, and supporting documentation.

```bash
MotoSpares/
├── backend/
├── frontend/
├── docs/
├── .gitignore
└── MotoSpares.slnx
```

## Technology Stack

**Backend**
- ASP.NET Core Web API
- C#
- Entity Framework Core

**Database**
- PostgreSQL

**Development Tools**
- Visual Studio
- Git and GitHub
- Postman

## Design Approach

The project is being developed with a structured and maintainable approach. The backend follows a clean folder organization to separate controllers, models, data access, services, DTOs, and supporting components. This helps keep the codebase readable and makes collaboration easier within the team.

Version control is managed through GitHub using branches for feature-based development. This supports safer collaboration, clearer commit history, and better project tracking throughout the coursework.

## Documentation and Evidence

The project is supported by documentation and development evidence, including:
- project planning and task division
- API development records
- screenshots
- ER diagrams
- milestone progress
- GitHub commit history

## Team Members

- **Sujal Parajuli**
- **Md**
- **Swodhin**
- **Nischal**
- **Niran**

## Academic Context

This project was developed for the **CS6004NI Application Development** module as a group coursework submission. The system is based on the provided vehicle parts selling and inventory management scenario and is built to reflect both functional and non-functional requirements expected in the module.

## Repository Notes

- `backend/` contains the ASP.NET Core Web API project
- `frontend/` is reserved for the client-side application
- `docs/` stores project-related supporting materials

## License

This repository is submitted for academic purposes.
