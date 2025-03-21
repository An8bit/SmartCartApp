@echo off
echo Starting SmartCartApp in English...

:: Set culture to English
set DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
set ASPNETCORE_CULTURE=en-US
set ASPNETCORE_UI_CULTURE=en-US

:: Navigate to the web project directory
cd %~dp0\SmartCartApp\Web

:: Build the project in Release mode
dotnet build -c Release

:: Run the application
echo Application starting at http://localhost:5000
dotnet run --urls=http://localhost:5000

echo Application stopped.
pause