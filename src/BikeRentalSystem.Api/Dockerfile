#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/BikeRentalSystem.Api/BikeRentalSystem.Api.csproj", "src/BikeRentalSystem.Api/"]
COPY ["src/BikeRentalSystem.Core/BikeRentalSystem.Core.csproj", "src/BikeRentalSystem.Core/"]
COPY ["src/BikeRentalSystem.Messaging/BikeRentalSystem.Messaging.csproj", "src/BikeRentalSystem.Messaging/"]
COPY ["src/BikeRentalSystem.Services/BikeRentalSystem.Services.csproj", "src/BikeRentalSystem.Services/"]
COPY ["src/BikeRentalSystem.Infrastructure/BikeRentalSystem.Infrastructure.csproj", "src/BikeRentalSystem.Infrastructure/"]
RUN dotnet restore "./src/BikeRentalSystem.Api/./BikeRentalSystem.Api.csproj"
COPY . .
WORKDIR "/src/src/BikeRentalSystem.Api"
RUN dotnet build "./BikeRentalSystem.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BikeRentalSystem.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BikeRentalSystem.Api.dll"]