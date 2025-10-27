@echo off
REM Universal launcher script for ArxivExpress Flutter app (Windows)
REM Provides menu to choose between desktop and web versions

echo ArxivExpress Flutter Universal Launcher
echo ======================================
echo.
echo Choose launch option:
echo 1. Windows Desktop App
echo 2. Web App (Chrome)
echo 3. Exit
echo.

set /p choice="Enter your choice (1-3): "

if "%choice%"=="1" (
    echo.
    echo Launching Windows desktop app...
    call scripts\run_windows.bat
) else if "%choice%"=="2" (
    echo.
    echo Launching web app...
    call scripts\run_web.bat
) else if "%choice%"=="3" (
    echo Goodbye!
    exit /b 0
) else (
    echo Invalid choice. Please run the script again.
    pause
    exit /b 1
)