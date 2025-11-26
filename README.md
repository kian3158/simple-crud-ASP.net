# Clean Architecture .NET Project

This project is a refactored .NET application, organized into a Clean Architecture structure.

## Prerequisites

*   **.NET SDK:** Version 8.0 or higher.
*   **Entity Framework Core:** The following packages are required:
    *   `Microsoft.EntityFrameworkCore` (version 8.0.0)
    *   `Microsoft.EntityFrameworkCore.SqlServer` (version 8.0.0)
    *   `Microsoft.EntityFrameworkCore.Tools` (version 8.0.0)
    *   `Microsoft.EntityFrameworkCore.Design` (version 8.0.0)

## Local Setup

1.  **Connection String:**
    Update the `ConnectionString` in `src/WebAPI/appsettings.json`. For a local SQL Server database, you can use the following example:

    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SchoolDb;Trusted_Connection=True;"
    }
    ```

2.  **Verification Script:**
    Run the `verify-locally.ps1` script from the root of the repository to restore packages, build the solution, and apply database migrations.

    ```powershell
    ./verify-locally.ps1
    ```

## Manual Steps (if the script fails)

If the verification script fails, you can perform the following steps manually:

1.  **Restore Packages:**
    ```bash
    dotnet restore src/CleanCrud.sln
    ```

2.  **Build Solution:**
    ```bash
    dotnet build src/CleanCrud.sln --no-restore
    ```

3.  **Install `dotnet-ef` Tool:**
    ```bash
    dotnet tool install --global dotnet-ef --version 8.0.0
    ```

4.  **Apply Migrations:**
    ```bash
    dotnet ef database update --project src/Infrastructure/Infrastructure.csproj --startup-project src/WebAPI/WebAPI.csproj
    ```

5.  **Run the Application:**
    ```bash
    dotnet run --project src/WebAPI/WebAPI.csproj
    ```
