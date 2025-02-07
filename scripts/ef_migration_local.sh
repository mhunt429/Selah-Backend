#!/bin/bash

# Set local environment variables
export DOTNET_ENVIRONMENT=Development
export ConnectionStrings__DefaultConnection="Host=localhost;Port=55432;Database=postgres;User ID=postgres;Password=postgres"

# Add Migration
#dotnet ef migrations add "$MIGRATION_NAME" --project src/Selah.Infrastructure --startup-project src/Selah.WebAPI/

# Update Database
dotnet ef database update --project src/Selah.Infrastructure --startup-project src/Selah.WebAPI/ --connection "Host=localhost;Port=55432;Database=postgres;User ID=postgres;Password=postgres"
