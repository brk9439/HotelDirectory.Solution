#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HotelDirectory.Reporting.Service.Consumer/HotelDirectory.Reporting.Service.Consumer.csproj", "HotelDirectory.Reporting.Service.Consumer/"]
COPY ["HotelDirectory.Reporting.Service.Infrastructure/HotelDirectory.Reporting.Service.Infrastructure.csproj", "HotelDirectory.Reporting.Service.Infrastructure/"]
COPY ["HotelDirectory.Shared.Common/HotelDirectory.Shared.Common.csproj", "HotelDirectory.Shared.Common/"]
COPY ["HotelDirectory.Shared.ElasticSearch/HotelDirectory.Shared.ElasticSearch.csproj", "HotelDirectory.Shared.ElasticSearch/"]
RUN dotnet restore "./HotelDirectory.Reporting.Service.Consumer/HotelDirectory.Reporting.Service.Consumer.csproj"
COPY . .
WORKDIR "/src/HotelDirectory.Reporting.Service.Consumer"
RUN dotnet build "./HotelDirectory.Reporting.Service.Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HotelDirectory.Reporting.Service.Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HotelDirectory.Reporting.Service.Consumer.dll"]