# verify-locally.ps1

# Exit script on first error
$ErrorActionPreference = "Stop"

# Restore NuGet packages
Write-Host "Restoring NuGet packages..."
dotnet restore src/CleanCrud.sln
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to restore packages."
    exit 1
}
Write-Host "Package restoration complete."

# Build the solution
Write-Host "Building the solution..."
dotnet build src/CleanCrud.sln --no-restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build the solution."
    exit 1
}
Write-Host "Solution built successfully."

# Install or update dotnet-ef tool
Write-Host "Installing/updating dotnet-ef tool..."
dotnet tool install --global dotnet-ef --version 8.0.0
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to install dotnet-ef tool."
    exit 1
}
Write-Host "dotnet-ef tool is ready."

# Apply EF Core migrations
Write-Host "Applying database migrations..."
dotnet ef database update --project src/Infrastructure/Infrastructure.csproj --startup-project src/WebAPI/WebAPI.csproj
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to apply migrations."
    exit 1
}
Write-Host "Migrations applied successfully."

Write-Host "Verification script completed successfully!"
exit 0
