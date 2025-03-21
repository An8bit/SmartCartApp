#!/bin/bash
echo "Starting SmartCartApp in English..."

# Set culture to English
export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
export ASPNETCORE_CULTURE=en-US
export ASPNETCORE_UI_CULTURE=en-US

# Navigate to the web project directory
cd "$(dirname "$0")/SmartCartApp/Web"

# Build the project in Release mode
dotnet build -c Release

# Run the application
echo "Application starting at http://localhost:5000"
dotnet run --urls=http://localhost:5000

echo "Application stopped."
