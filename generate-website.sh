#!/bin/bash
# Simple script to generate and open the website

# Activate virtual environment if available
if [ -f ".venv/bin/activate" ]; then
    source .venv/bin/activate
fi

# Generate website
echo "Generating website..."
python3 scripts/generate_website.py

# Open in browser
if [ -f "output/website/index.html" ]; then
    echo ""
    echo "Website generated successfully!"
    echo "Opening in browser..."
    
    # Try to open with xdg-open (Linux), open (macOS), or start (Windows/WSL)
    if command -v xdg-open &> /dev/null; then
        xdg-open output/website/index.html
    elif command -v open &> /dev/null; then
        open output/website/index.html
    elif command -v start &> /dev/null; then
        start output/website/index.html
    else
        echo "Could not automatically open browser."
        echo "Please open: $(pwd)/output/website/index.html"
    fi
else
    echo "Error: Website generation failed!"
    exit 1
fi
