@echo off
REM This batch file was created for development/testing purposes to handle
REM zombie process cleanup during development. Not intended for normal usage.
REM For normal operation, use: dotnet run "path\to\image.jpg" or ImageComments.exe "path\to\image.jpg"

echo ImageComments - Image Metadata Viewer (Development/Testing)
echo --------------------------------------------------------
echo.

if "%~1"=="" (
    echo Usage: RunWithImage.bat [path_to_image_file]
    echo Please provide a path to a JPG or PNG image file.
    exit /b 1
)

rem Close any running instances
taskkill /f /im ImageComments.exe >nul 2>&1

echo Loading image: %~1
echo.

dotnet run "%~1"

pause 