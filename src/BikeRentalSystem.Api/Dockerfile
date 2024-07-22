FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/BikeRentalSystem.Api/BikeRentalSystem.Api.csproj", "src/BikeRentalSystem.Api/"]
COPY ["src/BikeRentalSystem.Core/BikeRentalSystem.Core.csproj", "src/BikeRentalSystem.Core/"]
COPY ["src/BikeRentalSystem.Identity/BikeRentalSystem.Identity.csproj", "src/BikeRentalSystem.Identity/"]
COPY ["src/BikeRentalSystem.Services/BikeRentalSystem.RentalServices.csproj", "src/BikeRentalSystem.Services/"]
COPY ["src/BikeRentalSystem.Infrastructure/BikeRentalSystem.Infrastructure.csproj", "src/BikeRentalSystem.Infrastructure/"]
COPY ["src/BikeRentalSystem.Shared/BikeRentalSystem.Shared.csproj", "src/BikeRentalSystem.Shared/"]
COPY ["src/BikeRentalSystem.Messaging/BikeRentalSystem.Messaging.csproj", "src/BikeRentalSystem.Messaging/"]
RUN dotnet restore "./BikeRentalSystem.Api/BikeRentalSystem.Api.csproj"
COPY . .
WORKDIR "/src/BikeRentalSystem.Api"
RUN dotnet build "./BikeRentalSystem.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "./BikeRentalSystem.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BikeRentalSystem.Api.dll"]
