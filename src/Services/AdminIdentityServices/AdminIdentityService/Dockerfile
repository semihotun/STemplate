#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/AdminIdentityServices/AdminIdentityService/AdminIdentityService.csproj", "src/Services/AdminIdentityServices/AdminIdentityService/"]
COPY ["src/Services/AdminIdentityServices/AdminIdentityService.Application/AdminIdentityService.Application.csproj", "src/Services/AdminIdentityServices/AdminIdentityService.Application/"]
COPY ["src/Services/AdminIdentityServices/AdminIdentityService.Domain/AdminIdentityService.Domain.csproj", "src/Services/AdminIdentityServices/AdminIdentityService.Domain/"]
COPY ["src/Services/AdminIdentityServices/AdminIdentityService.Insfrastructure/AdminIdentityService.Insfrastructure.csproj", "src/Services/AdminIdentityServices/AdminIdentityService.Insfrastructure/"]
COPY ["src/Services/AdminIdentityServices/AdminIdentityService.Persistence/AdminIdentityService.Persistence.csproj", "src/Services/AdminIdentityServices/AdminIdentityService.Persistence/"]
RUN dotnet restore "./src/Services/AdminIdentityServices/AdminIdentityService/./AdminIdentityService.csproj"
COPY . .
WORKDIR "/src/src/Services/AdminIdentityServices/AdminIdentityService"
RUN dotnet build "./AdminIdentityService.csproj" -c $BUILD_CONFIGURATION -o /app/build
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AdminIdentityService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdminIdentityService.dll"]