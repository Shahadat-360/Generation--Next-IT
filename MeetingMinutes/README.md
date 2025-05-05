# Meeting Minutes Application

A web application for recording and managing meeting minutes with customers, built using ASP.NET Core MVC and Entity Framework Core.

## Overview

This application allows users to create, edit, and view meeting minutes for meetings held with both corporate and individual customers. It supports recording meeting details such as date, time, location, attendees, agenda, discussions, decisions, and products/services discussed during the meeting.

## Features

- Create, edit, view, and delete meeting minutes
- Support for both corporate and individual customers
- Add multiple products/services to each meeting with quantities and units
- Record meeting attendees from both client and host sides
- Document meeting agenda, discussions, and decisions
- Responsive design using Bootstrap for optimal viewing on all devices

## Technology Stack

- **Framework**: ASP.NET Core MVC (.NET 9.0)
- **ORM**: Entity Framework Core 9.0.4
- **Database**: SQL Server
- **Frontend**: HTML, CSS, JavaScript, Bootstrap
- **Development Tools**: Visual Studio 2022 or Visual Studio Code

## Requirements

- .NET 9.0 SDK or later
- SQL Server (Express, LocalDB, or full SQL Server)
- Visual Studio 2022, Visual Studio Code, or other compatible IDE

## Setup Instructions

1. **Clone the repository**

2. **Set up the database**

   There are two options to set up the database:

   **Option 1: Using Entity Framework migrations**
   - Open a command prompt or terminal in the project directory
   - Run the following command to apply existing migrations:
     ```
     dotnet ef database update --project .\MeetingMinutes --context ApplicationDbContext
     ```

   **Option 2: Using SQL Server Management Studio (SSMS)**
   - Generate the SQL script using Entity Framework:
     ```
     dotnet ef migrations script --project .\MeetingMinutes --context ApplicationDbContext
     ```
   - Execute the generated script in SQL Server Management Studio

3. **Update connection string (if needed)**

   If you're not using SQL Server Express or have a custom SQL Server setup, update the connection string in `appsettings.json` to match your environment:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YourServerName;Database=Gen_Next;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
   }
   ```

4. **Run the application**

   - Using .NET CLI:
     ```
     dotnet run --project .\MeetingMinutes
     ```
   - Or using Visual Studio: Open the solution and press F5 or click the "Run" button

## Project Structure

- **Controllers/**
  - HomeController.cs - Handles the home page and basic navigation
  - MeetingMinutesController.cs - Handles all meeting minutes CRUD operations

- **Models/**
  - Customer.cs - Contains CorporateCustomer and IndividualCustomer classes (simplified with only Id and Name)
  - MeetingMinutes.cs - Contains MeetingMinutesMaster and MeetingMinutesDetail classes
  - ProductService.cs - Represents products and services
  - MeetingMinutesViewModel.cs - ViewModel for meeting minutes forms
  - ErrorViewModel.cs - Used for error handling

- **Views/**
  - Home/ - Views for the home page and general navigation
  - MeetingMinutes/ - Views for creating, editing, and viewing meeting minutes
  - Shared/ - Layout and shared components

- **Data/**
  - ApplicationDbContext.cs - Entity Framework database context

- **Migrations/**
  - Database migration files for Entity Framework Core

## Database Schema

The application uses the following tables:

1. **Corporate_Customer_Tbl**
   - Id (PK)
   - Name

2. **Individual_Customer_Tbl**
   - Id (PK)
   - Name

3. **Products_Service_Tbl**
   - Id (PK)
   - Name
   - Unit
   - Description

4. **Meeting_Minutes_Master_Tbl**
   - Id (PK)
   - CustomerType
   - CustomerId (FK)
   - CustomerName
   - Date
   - Time
   - MeetingPlace
   - AttendsFromClientSide
   - AttendsFromHostSide
   - MeetingAgenda
   - MeetingDiscussion
   - MeetingDecision

5. **Meeting_Minutes_Details_Tbl**
   - Id (PK)
   - MeetingMinutesMasterId (FK)
   - ProductServiceId (FK)
   - ProductServiceName
   - Quantity (decimal with precision 18,2)
   - Unit