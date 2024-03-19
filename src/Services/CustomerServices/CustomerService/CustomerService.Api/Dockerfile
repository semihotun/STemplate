#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/CustomerServices/CustomerService/CustomerService.Api/CustomerService.Api.csproj", "src/Services/CustomerServices/CustomerService/CustomerService.Api/"]
COPY ["src/Services/CustomerServices/CustomerService/CustomerService.Application/CustomerService.Application.csproj", "src/Services/CustomerServices/CustomerService/CustomerService.Application/"]
COPY ["src/Services/CustomerServices/CustomerService/CustomerService.Domain/CustomerService.Domain.csproj", "src/Services/CustomerServices/CustomerService/CustomerService.Domain/"]
COPY ["src/Services/CustomerServices/CustomerService/CustomerService.Insfrastructure/CustomerService.Insfrastructure.csproj", "src/Services/CustomerServices/CustomerService/CustomerService.Insfrastructure/"]
COPY ["src/Services/CustomerServices/CustomerService/CustomerService.Persistence/CustomerService.Persistence.csproj", "src/Services/CustomerServices/CustomerService/CustomerService.Persistence/"]
RUN dotnet restore "./src/Services/CustomerServices/CustomerService/CustomerService.Api/./CustomerService.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/CustomerServices/CustomerService/CustomerService.Api"
RUN dotnet build "./CustomerService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CustomerService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerService.Api.dll"]