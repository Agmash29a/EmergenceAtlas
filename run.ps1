# Run from the repository folder in Windows PowerShell 5.1 or PowerShell 7.
$ErrorActionPreference = 'Stop'
Push-Location $PSScriptRoot
try {
    Write-Host 'Running Emergence Atlas tests...' -ForegroundColor Cyan
    dotnet run --project tests/EmergenceAtlas.Tests
    if ($LASTEXITCODE -ne 0) { throw 'Automated tests failed.' }
    Write-Host 'Running all Emergence Atlas experiments...' -ForegroundColor Cyan
    dotnet run --project src/EmergenceAtlas.Cli -- all --out output
    if ($LASTEXITCODE -ne 0) { throw 'Experiments failed.' }
    $report = Join-Path $PSScriptRoot 'output\report.html'
    Write-Host "Opening $report" -ForegroundColor Green
    Start-Process $report
}
finally { Pop-Location }
