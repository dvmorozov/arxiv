#!/bin/bash
# Universal launcher script for ArxivExpress Flutter app
# Automatically detects platform and launches the appropriate version

echo "ArxivExpress Flutter Universal Launcher"
echo "======================================"

# Detect platform
if [[ "$OSTYPE" == "msys" || "$OSTYPE" == "cygwin" || "$OSTYPE" == "win32" ]]; then
    echo "Detected Windows platform"
    echo "Launching Windows desktop app..."
    ./scripts/run_windows.bat
elif [[ "$OSTYPE" == "darwin"* ]]; then
    echo "Detected macOS platform"
    echo "Launching macOS desktop app..."
    chmod +x ./scripts/run_macos.sh
    ./scripts/run_macos.sh
elif [[ "$OSTYPE" == "linux-gnu"* ]]; then
    echo "Detected Linux platform" 
    echo "Launching Linux desktop app..."
    chmod +x ./scripts/run_linux.sh
    ./scripts/run_linux.sh
else
    echo "Unknown platform: $OSTYPE"
    echo "Please run the appropriate script manually from the scripts/ directory"
    exit 1
fi