#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/PaymentServices/PaymentService/PaymentService.Api/PaymentService.Api.csproj", "src/Services/PaymentServices/PaymentService/PaymentService.Api/"]
COPY ["src/Services/PaymentServices/PaymentService/PaymentService.Application/PaymentService.Application.csproj", "src/Services/PaymentServices/PaymentService/PaymentService.Application/"]
COPY ["src/Services/PaymentServices/PaymentService/PaymentService.Domain/PaymentService.Domain.csproj", "src/Services/PaymentServices/PaymentService/PaymentService.Domain/"]
COPY ["src/Services/PaymentServices/PaymentService/PaymentService.Insfrastructure/PaymentService.Insfrastructure.csproj", "src/Services/PaymentServices/PaymentService/PaymentService.Insfrastructure/"]
COPY ["src/Services/PaymentServices/PaymentService/PaymentService.Persistence/PaymentService.Persistence.csproj", "src/Services/PaymentServices/PaymentService/PaymentService.Persistence/"]
RUN dotnet restore "./src/Services/PaymentServices/PaymentService/PaymentService.Api/./PaymentService.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/PaymentServices/PaymentService/PaymentService.Api"
RUN dotnet build "./PaymentService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PaymentService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaymentService.Api.dll"]