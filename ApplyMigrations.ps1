# ApplyMigrations.ps1
param (
    [string]$MigrationName = ""
)

# Use timestamp if no migration name provided
if ([string]::IsNullOrEmpty($MigrationName)) {
    $MigrationName = "AutoMigration_" + (Get-Date -Format "yyyyMMdd_HHmmss")
}

# Paths
$solutionDir = Split-Path -Parent $MyInvocation.MyCommand.Definition
$infrastructureProj = Join-Path $solutionDir "..\Infrastructure\Infrastructure.csproj"
$startupProj = Join-Path $solutionDir "WebAPI.csproj"

Write-Host "Building solution..."
dotnet build $solutionDir
if ($LASTEXITCODE -ne 0) {
    Write-Error "Build failed. Aborting."
    exit $LASTEXITCODE
}

Write-Host "Creating migration '$MigrationName'..."
dotnet ef migrations add $MigrationName --project $infrastructureProj --startup-project $startupProj --output-dir Migrations
if ($LASTEXITCODE -ne 0) {
    Write-Error "Migration creation failed. Aborting."
    exit $LASTEXITCODE
}

Write-Host "Applying migration to database..."
dotnet ef database update --project $infrastructureProj --startup-project $startupProj
if ($LASTEXITCODE -ne 0) {
    Write-Error "Database update failed."
    exit $LASTEXITCODE
}

Write-Host "Migration applied successfully!"
