<#
  Run from repo root:
  .\scripts\apply-migrations.ps1
#>

Write-Host "Applying EF Core migrations (Infrastructure project, WebAPI startup project)..."

dotnet build

dotnet ef database update --project src/Infrastructure --startup-project src/WebAPI

if ($LASTEXITCODE -ne 0) {
    Write-Error "Migrations failed (exit code $LASTEXITCODE)"
    exit $LASTEXITCODE
}

Write-Host "Migrations applied successfully."
