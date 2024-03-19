#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/ShippingServices/ShippingService/ShippingService.Api/ShippingService.Api.csproj", "src/Services/ShippingServices/ShippingService/ShippingService.Api/"]
COPY ["src/Services/ShippingServices/ShippingService/ShippingService.Application/ShippingService.Application.csproj", "src/Services/ShippingServices/ShippingService/ShippingService.Application/"]
COPY ["src/Services/ShippingServices/ShippingService/ShippingService.Domain/ShippingService.Domain.csproj", "src/Services/ShippingServices/ShippingService/ShippingService.Domain/"]
COPY ["src/Services/ShippingServices/ShippingService/ShippingService.Insfrastructure/ShippingService.Insfrastructure.csproj", "src/Services/ShippingServices/ShippingService/ShippingService.Insfrastructure/"]
COPY ["src/Services/ShippingServices/ShippingService/ShippingService.Persistence/ShippingService.Persistence.csproj", "src/Services/ShippingServices/ShippingService/ShippingService.Persistence/"]
RUN dotnet restore "./src/Services/ShippingServices/ShippingService/ShippingService.Api/./ShippingService.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/ShippingServices/ShippingService/ShippingService.Api"
RUN dotnet build "./ShippingService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ShippingService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShippingService.Api.dll"]