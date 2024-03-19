#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/SearchBarServices/SearchBarService/SearchBarService.Api/SearchBarService.Api.csproj", "src/Services/SearchBarServices/SearchBarService/SearchBarService.Api/"]
COPY ["src/Services/SearchBarServices/SearchBarService/SearchBarService.Application/SearchBarService.Application.csproj", "src/Services/SearchBarServices/SearchBarService/SearchBarService.Application/"]
COPY ["src/Services/SearchBarServices/SearchBarService/SearchBarService.Domain/SearchBarService.Domain.csproj", "src/Services/SearchBarServices/SearchBarService/SearchBarService.Domain/"]
COPY ["src/Services/SearchBarServices/SearchBarService/SearchBarService.Insfrastructure/SearchBarService.Insfrastructure.csproj", "src/Services/SearchBarServices/SearchBarService/SearchBarService.Insfrastructure/"]
COPY ["src/Services/SearchBarServices/SearchBarService/SearchBarService.Persistence/SearchBarService.Persistence.csproj", "src/Services/SearchBarServices/SearchBarService/SearchBarService.Persistence/"]
RUN dotnet restore "./src/Services/SearchBarServices/SearchBarService/SearchBarService.Api/./SearchBarService.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/SearchBarServices/SearchBarService/SearchBarService.Api"
RUN dotnet build "./SearchBarService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SearchBarService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SearchBarService.Api.dll"]