#!/bin/sh
echo "Aplicando migrations..."

dotnet ef database update \
  --project ../Venice.Orders.Infrastructure/Venice.Orders.Infrastructure.csproj \
  --startup-project Venice.Orders.Api.csproj \
  --context Venice.Orders.Infrastructure.DataProviders.Contexts.ApplicationDbContext \
  --verbose

echo "Migrations aplicadas, iniciando API..."
dotnet Venice.Orders.Api.dll
