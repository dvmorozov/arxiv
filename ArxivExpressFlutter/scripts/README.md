# ArxivExpress Flutter Launch Scripts

This directory contains platform-specific launch scripts for the ArxivExpress Flutter application.

## Prerequisites

- Flutter SDK installed and in PATH
- Platform-specific requirements met (see Flutter documentation)

## Scripts Available

### Universal Launchers
- `launch.bat` - Windows universal launcher with menu
- `launch.sh` - Cross-platform universal launcher (auto-detects OS)

### Windows
- `run_windows.bat` - Windows batch script to launch desktop app
- `run_windows.ps1` - PowerShell script to launch desktop app  
- `run_web.bat` - Windows batch script to launch web app

### macOS
- `run_macos.sh` - Bash script to launch macOS desktop app

### Linux
- `run_linux.sh` - Bash script to launch Linux desktop app

### Web (Cross-platform)
- `run_web.sh` - Bash script to launch web app in browser

## Usage

### Quick Start (Recommended)
```cmd
# Windows - Universal launcher with menu
launch.bat

# macOS/Linux - Auto-detects platform
chmod +x launch.sh
./launch.sh
```

### Platform-Specific Launch

#### Windows
```cmd
# Desktop app
run_windows.bat
# or
powershell .\run_windows.ps1

# Web app
run_web.bat
```

#### macOS/Linux
```bash
# Make script executable (first time only)
chmod +x run_macos.sh
chmod +x run_linux.sh
chmod +x run_web.sh

# Desktop app
./run_macos.sh    # on macOS
./run_linux.sh    # on Linux

# Web app
./run_web.sh
```

## Notes

- Scripts must be run from the ArxivExpressFlutter project root directory
- Make sure Flutter dependencies are installed (`flutter pub get`)
- For first-time setup, run `flutter doctor` to verify platform requirements
- Web scripts will open the app in Chrome by default

## Development

For development with hot reload, you can also run directly:
```bash
flutter run -d windows   # Windows
flutter run -d macos     # macOS  
flutter run -d linux     # Linux
flutter run -d chrome    # Web
```