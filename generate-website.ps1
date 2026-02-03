#!/usr/bin/env pwsh
# Simple script to generate and open the website

# Activate virtual environment if available
if (Test-Path ".venv\Scripts\Activate.ps1") {
    & ".venv\Scripts\Activate.ps1"
}

# Generate website
Write-Host "Generating website..." -ForegroundColor Cyan
python scripts\generate_website.py

# Check if generation was successful
if (Test-Path "output\website\index.html") {
    Write-Host ""
    Write-Host "Website generated successfully!" -ForegroundColor Green
    Write-Host "Opening in browser..." -ForegroundColor Cyan
    
    # Open in default browser (Windows)
    Start-Process "output\website\index.html"
}
else {
    Write-Host "Error: Website generation failed!" -ForegroundColor Red
    exit 1
}
