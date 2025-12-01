<#
  Seeds database by running WebAPI in "seed-only" mode (it runs initializer then exits).
  Usage:
    .\scripts\seed-database.ps1
#>

Write-Host "Building WebAPI..."
dotnet build src/WebAPI

Write-Host "Running seed-only..."
# Set env var so Program knows to run initializer and exit
$env:SEED_ONLY = "1"

# Run dotnet; passing --no-build because we just built
dotnet run --project src/WebAPI --no-build

# Clean up env var
Remove-Item Env:\SEED_ONLY -ErrorAction SilentlyContinue

Write-Host "Seeding complete (or logged relevant errors)."
