# PowerShell launch script for ArxivExpress Flutter app
# This script launches the app on Windows desktop

Write-Host "Starting ArxivExpress Flutter app on Windows..." -ForegroundColor Green
flutter run -d windows

Write-Host "Press any key to continue..." -ForegroundColor Yellow
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")