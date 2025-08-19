ARG DOTNET_RUNTIME=mcr.microsoft.com/dotnet/aspnet:9.0
ARG DOTNET_SDK=mcr.microsoft.com/dotnet/sdk:9.0

FROM ${DOTNET_RUNTIME} AS base
ENV ASPNETCORE_URLS=http://+:8080
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM ${DOTNET_SDK} AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY . .

RUN dotnet restore Venice.Orders.sln

WORKDIR /src/Venice.Orders.Api
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish Venice.Orders.Api.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM ${DOTNET_SDK} AS final
WORKDIR /app

COPY --from=build /src/ .

WORKDIR /app/Venice.Orders.Api
COPY --from=publish /app/publish/ .

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
COPY run-migrations.sh .
RUN chmod +x ./run-migrations.sh

ENTRYPOINT ["./run-migrations.sh"]
