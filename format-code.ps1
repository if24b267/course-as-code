#!/usr/bin/env pwsh

# Script to format C# code using dotnet-format
# This script can be run on Windows, macOS, or Linux with PowerShell Core

# Check if dotnet-format is installed
$formatTool = dotnet tool list --global | Select-String "dotnet-format"
if (-not $formatTool) {
    Write-Host "Installing dotnet-format globally..."
    dotnet tool install --global dotnet-format
}

# Format the code
Write-Host "Formatting C# code..."
dotnet format style --severity info

# Run code analyzers
Write-Host "Running code analyzers..."
dotnet build /p:TreatWarningsAsErrors=false /p:EnforceCodeStyleInBuild=true

Write-Host "Done!"