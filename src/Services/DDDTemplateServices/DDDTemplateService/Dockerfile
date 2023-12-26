#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/DDDTemplateServices/DDDTemplateService/DDDTemplateService.csproj", "src/Services/DDDTemplateServices/DDDTemplateService/"]
COPY ["src/Services/DDDTemplateServices/DDDTemplateService.Application/DDDTemplateService.Application.csproj", "src/Services/DDDTemplateServices/DDDTemplateService.Application/"]
COPY ["src/Services/DDDTemplateServices/DDDTemplateService.Domain/DDDTemplateService.Domain.csproj", "src/Services/DDDTemplateServices/DDDTemplateService.Domain/"]
COPY ["src/Services/DDDTemplateServices/DDDTemplateServices.Insfrastructure/DDDTemplateServices.Insfrastructure.csproj", "src/Services/DDDTemplateServices/DDDTemplateServices.Insfrastructure/"]
RUN dotnet restore "./src/Services/DDDTemplateServices/DDDTemplateService/./DDDTemplateService.csproj"
COPY . .
WORKDIR "/src/src/Services/DDDTemplateServices/DDDTemplateService"
RUN dotnet build "./DDDTemplateService.csproj" -c $BUILD_CONFIGURATION -o /app/build
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DDDTemplateService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DDDTemplateService.dll"]