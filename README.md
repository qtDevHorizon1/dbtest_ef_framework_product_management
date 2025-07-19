# Entity Framework Product Management Application

This is a .NET Framework 4.8 console application using Entity Framework 6.5.0 for data access, demonstrating integration with SQL Server 2019 and following best practices for application architecture.

## Technology Stack

- .NET Framework 4.8
- Entity Framework 6.5.0
- SQL Server 2019
- Visual Studio 2019 or later

## Prerequisites

- Visual Studio 2019 or later
- .NET Framework 4.8 SDK
- SQL Server 2019 (Developer Edition is free and recommended for development)
- SQL Server Management Studio (SSMS) or Azure Data Studio

## Project Structure

- `DataAccess/`: Contains Entity Framework DbContext and repository classes
- `Models/`: Contains entity classes with EF attributes
- `Business/`: Contains business logic
- `CLI/`: Contains command-line interface implementation
- `Database/Scripts/`: Contains database setup and stored procedure scripts

## Setup Instructions

1. **Install SQL Server 2019**:
   - Download and install SQL Server 2019 Developer Edition
   - During installation, note down your instance name (default is usually `.` or `localhost`)
   - Make sure SQL Server Browser service is running

2. **Set Up Database**:
   - Open SQL Server Management Studio (SSMS)
   - Connect to your SQL Server instance
   - Run the script `Database/Scripts/01_InitialSetup.sql`
   - This will create the database, tables, stored procedures, and sample data

3. **Configure Connection String**:
   - Open `App.config`
   - The connection string is already configured for SQL Server 2019:
   ```xml
   <add name="DevConnection" 
        connectionString="Data Source=.;Initial Catalog=ProductManagement;Integrated Security=True;MultipleActiveResultSets=True"
        providerName="System.Data.SqlClient" />
   ```
   - If your SQL Server instance has a different name, update the `Data Source` value

4. **Restore NuGet Packages**:
   - Run the PowerShell script `restore-ef.ps1` to restore Entity Framework packages:
   ```powershell
   powershell -ExecutionPolicy Bypass -File restore-ef.ps1
   ```
   - This will install Entity Framework 6.5.0 and its dependencies

## Running the Application

The application can be run in two modes: Interactive (menu-driven) and Command-Line Interface (CLI).

### Interactive Mode

1. Run the application without any arguments:
   ```
   dotnet run
   ```
2. You'll see the main menu with these options:
   ```
   Product Management System
   ------------------------
   1. List all products
   2. Get product by ID
   3. Create new product
   4. Update product
   5. Delete product
   6. Update product stock
   Q. Quit
   ```

### Command-Line Interface (CLI)

The application supports the following commands:

```bash
# Show help
dotnet run -- --help

# List all products
dotnet run -- list

# Get product by ID
dotnet run -- get 1

# Add new product
dotnet run -- add "Gaming Mouse" 49.99 10 "High-performance gaming mouse"

# Update product
dotnet run -- update 1 "Gaming Mouse Pro" 59.99 15 "Updated gaming mouse"

# Delete product
dotnet run -- delete 1

# Update stock quantity
dotnet run -- stock 1 20
```

## Database Structure

The application uses the following database objects:

1. **Products Table**:
   - ProductId (INT, Primary Key)
   - Name (NVARCHAR(100))
   - Description (NVARCHAR(500))
   - Price (DECIMAL(18,2))
   - StockQuantity (INT)
   - CreatedDate (DATETIME)
   - ModifiedDate (DATETIME)

2. **Stored Procedures**:
   - sp_GetAllProducts
   - sp_GetProductById
   - sp_InsertProduct
   - sp_UpdateProduct
   - sp_DeleteProduct

## Troubleshooting

If you encounter errors:
1. Verify SQL Server 2019 is running (check Services)
2. Confirm your connection string matches your SQL Server instance name
3. Ensure the database exists and is accessible
4. Check you have appropriate permissions to access the database
5. Make sure Entity Framework 6.5.0 NuGet package is installed correctly
6. Check the Output window in Visual Studio for detailed error messages

## Entity Framework Features Used

- Entity Framework 6.5.0 with .NET Framework 4.8
- Code-First approach with data annotations
- Automatic change tracking
- Built-in transaction management
- LINQ queries for data access
- Proper resource disposal with DbContext
- Integration with SQL Server stored procedures
- Fluent API for entity configuration

## Best Practices Implemented

- Separation of concerns (layered architecture)
- Repository pattern
- Proper resource disposal
- Transaction management
- Error handling and logging
- Configuration management
- Security best practices
- Stored procedure usage for data access 