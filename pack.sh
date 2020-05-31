#!/usr/bin/env bash

# Clean and build in release
dotnet restore
dotnet clean
dotnet build -c Release

# Create all NuGet packages
nuget pack nuspec/Piranha.Templates.nuspec -OutputDirectory ./artifacts
