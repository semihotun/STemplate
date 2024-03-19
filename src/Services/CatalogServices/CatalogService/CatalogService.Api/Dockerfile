#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/CatalogServices/CatalogService/CatalogService.Api/CatalogService.Api.csproj", "src/Services/CatalogServices/CatalogService/CatalogService.Api/"]
COPY ["src/Services/CatalogServices/CatalogService/CatalogService.Application/CatalogService.Application.csproj", "src/Services/CatalogServices/CatalogService/CatalogService.Application/"]
COPY ["src/Services/CatalogServices/CatalogService/CatalogService.Domain/CatalogService.Domain.csproj", "src/Services/CatalogServices/CatalogService/CatalogService.Domain/"]
COPY ["src/Services/CatalogServices/CatalogService/CatalogService.Insfrastructure/CatalogService.Insfrastructure.csproj", "src/Services/CatalogServices/CatalogService/CatalogService.Insfrastructure/"]
COPY ["src/Services/CatalogServices/CatalogService/CatalogService.Persistence/CatalogService.Persistence.csproj", "src/Services/CatalogServices/CatalogService/CatalogService.Persistence/"]
RUN dotnet restore "./src/Services/CatalogServices/CatalogService/CatalogService.Api/./CatalogService.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/CatalogServices/CatalogService/CatalogService.Api"
RUN dotnet build "./CatalogService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CatalogService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CatalogService.Api.dll"]