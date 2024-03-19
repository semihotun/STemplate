#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/OrderServices/OrderService/OrderService.Api/OrderService.Api.csproj", "src/Services/OrderServices/OrderService/OrderService.Api/"]
COPY ["src/Services/OrderServices/OrderService/OrderService.Application/OrderService.Application.csproj", "src/Services/OrderServices/OrderService/OrderService.Application/"]
COPY ["src/Services/OrderServices/OrderService/OrderService.Domain/OrderService.Domain.csproj", "src/Services/OrderServices/OrderService/OrderService.Domain/"]
COPY ["src/Services/OrderServices/OrderService/OrderService.Insfrastructure/OrderService.Insfrastructure.csproj", "src/Services/OrderServices/OrderService/OrderService.Insfrastructure/"]
COPY ["src/Services/OrderServices/OrderService/OrderService.Persistence/OrderService.Persistence.csproj", "src/Services/OrderServices/OrderService/OrderService.Persistence/"]
RUN dotnet restore "./src/Services/OrderServices/OrderService/OrderService.Api/./OrderService.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/OrderServices/OrderService/OrderService.Api"
RUN dotnet build "./OrderService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OrderService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.Api.dll"]