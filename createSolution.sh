#!/bin/bash

echo "Creating .NET userPermission API"
dotnet new web -o userPermissionApi

echo "Creating .NET XUnit Test Project"
dotnet new xunit -o userPermissionApi.Test

echo "Adding Minimal API to Test Project"
dotnet add userPermissionApi.Tests reference userPermissionApi

echo "Creating solution file"
dotnet new sln -n userPermissionApi

echo "Adding projects to solution"
dotnet sln userPermissionApi.sln add userPermissionApi userPermissionApi.Tests

echo "Adding NuGet Packages"
dotnet add userPermissionApi.Tests package Microsoft.AspNetCore.Mvc.Testing
dotnet add userPermissionApi package MiniValidation