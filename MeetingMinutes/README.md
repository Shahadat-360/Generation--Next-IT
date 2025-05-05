# Meeting Minutes Application

A web application for recording and managing meeting minutes with customers, built using ASP.NET Core MVC.

## Features

- Create, edit, and view meeting minutes
- Switch between Corporate and Individual customers
- Add multiple products/services to each meeting
- Responsive design using Bootstrap 5

## Requirements

- .NET 8.0 SDK or later
- SQL Server (LocalDB or full SQL Server)
- Visual Studio 2022 or other compatible IDE

## Setup Instructions

1. **Clone the repository**

2. **Set up the database**

   There are two options to set up the database:

   **Option 1: Using SQL Server Management Studio (SSMS)**
   - Open the `CreateDatabase.sql` script in SSMS
   - Execute the script to create the database, tables, stored procedures, and sample data

   **Option 2: Using Entity Framework migrations**
   - Open a command prompt or terminal in the project directory
   - Run the following commands:
     ```
     dotnet ef migrations add InitialCreate
     dotnet ef database update
     ```

3. **Update connection string (if needed)**

   If you're not using LocalDB or have a custom SQL Server setup, update the connection string in `appsettings.json` to match your environment.

4. **Run the application**

   - Open a command prompt or terminal in the project directory
   - Run the following command:
     ```
     dotnet run
     ```
   - Navigate to https://localhost:5001 (or the URL shown in the console)

## Usage

1. **Create a new meeting minute**
   - Click on "Meeting Minutes" in the navigation menu
   - Click on "Create New" button
   - Fill out the form with meeting details
   - Add products/services using the form at the bottom
   - Click "Save" to save the meeting minute

2. **Edit an existing meeting minute**
   - Click on "Meeting Minutes" in the navigation menu
   - Find the meeting minute you want to edit
   - Click on the "Edit" button
   - Make your changes
   - Click "Save" to update the meeting minute

## Project Structure

- **Models**: Contains the data models for the application
- **Views**: Contains the Razor views for rendering the UI
- **Controllers**: Contains the controllers for handling HTTP requests
- **Data**: Contains the database context and configuration

## Database Schema

The application uses the following tables:
- Corporate_Customer_Tbl
- Individual_Customer_Tbl
- Products_Service_Tbl
- Meeting_Minutes_Master_Tbl
- Meeting_Minutes_Details_Tbl

And the following stored procedures:
- Meeting_Minutes_Master_Save_SP
- Meeting_Minutes_Details_Save_SP 