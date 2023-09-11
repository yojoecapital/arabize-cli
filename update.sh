#!/bin/bash

# GitHub repository and release information
REPO_OWNER="yojoecapital"
REPO_NAME="Arabize"

# Download the latest release ZIP file
gh release download -R "$REPO_OWNER/$REPO_NAME" --pattern "*.zip"

# Rename the settings file
mv settings.json tmp.json

# Extract the ZIP file using 7z (assuming 7z is installed)
7z x "$REPO_NAME.zip"

# Restore the settings file
mv tmp.json settings.json

# Delete the downloaded ZIP file
rm "$REPO_NAME.zip"

echo "Download, extraction, and cleanup complete."