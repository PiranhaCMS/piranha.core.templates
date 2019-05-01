#!/usr/bin/env bash

# Define directories
SCRIPT_DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )
OUTPUT_DIR=$SCRIPT_DIR/artifacts

# Clean and build in release
dotnet clean
dotnet build -c Release

# Make sure output folder exist.
if [ ! -d "$OUTPUT_DIR" ]; then
  mkdir "$OUTPUT_DIR"
fi

# Create all NuGet packages
nuget pack nuspec/Piranha.Templates.nuspec -OutputDirectory $OUTPUT_DIR
