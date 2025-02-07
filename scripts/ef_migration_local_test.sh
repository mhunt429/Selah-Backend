#!/bin/bash

export DOTNET_ENVIRONMENT=Testing
export ConnectionStrings__DefaultConnection="Host=localhost;Port=65432;Database=postgres;User ID=postgres;Password=postgres"

# Add Migration
#dotnet ef migrations add "$MIGRATION_NAME" --project src/Selah.Infrastructure --startup-project src/Selah.WebAPI/

# Update Database
dotnet ef database update --project src/Selah.Infrastructure --startup-project src/Selah.WebAPI/ --connection "Host=localhost;Port=65432;Database=postgres;User ID=postgres;Password=postgres"