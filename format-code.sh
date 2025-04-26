#!/bin/bash

# Script to format C# code using dotnet-format
# This script can be run on macOS or Linux

# Check if dotnet-format is installed
if ! dotnet tool list --global | grep -q "dotnet-format"; then
    echo "Installing dotnet-format globally..."
    dotnet tool install --global dotnet-format
fi

# Format the code
echo "Formatting C# code..."
dotnet format style --severity info

# Run code analyzers
echo "Running code analyzers..."
dotnet build /p:TreatWarningsAsErrors=false /p:EnforceCodeStyleInBuild=true

echo "Done!"