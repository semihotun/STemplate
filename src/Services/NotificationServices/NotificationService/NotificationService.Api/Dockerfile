#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/NotificationServices/NotificationService/NotificationService.Api/NotificationService.Api.csproj", "src/Services/NotificationServices/NotificationService/NotificationService.Api/"]
COPY ["src/Services/NotificationServices/NotificationService/NotificationService.Application/NotificationService.Application.csproj", "src/Services/NotificationServices/NotificationService/NotificationService.Application/"]
COPY ["src/Services/NotificationServices/NotificationService/NotificationService.Domain/NotificationService.Domain.csproj", "src/Services/NotificationServices/NotificationService/NotificationService.Domain/"]
COPY ["src/Services/NotificationServices/NotificationService/NotificationService.Insfrastructure/NotificationService.Insfrastructure.csproj", "src/Services/NotificationServices/NotificationService/NotificationService.Insfrastructure/"]
COPY ["src/Services/NotificationServices/NotificationService/NotificationService.Persistence/NotificationService.Persistence.csproj", "src/Services/NotificationServices/NotificationService/NotificationService.Persistence/"]
RUN dotnet restore "./src/Services/NotificationServices/NotificationService/NotificationService.Api/./NotificationService.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/NotificationServices/NotificationService/NotificationService.Api"
RUN dotnet build "./NotificationService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NotificationService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.Api.dll"]