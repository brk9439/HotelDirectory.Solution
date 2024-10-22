#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HotelDirectory.Reporting.Service.Application/HotelDirectory.Reporting.Service.Application.csproj", "HotelDirectory.Reporting.Service.Application/"]
COPY ["HotelDirectory.Hotel.Service.Business/HotelDirectory.Hotel.Service.Business.csproj", "HotelDirectory.Hotel.Service.Business/"]
COPY ["HotelDirectory.Hotel.Service.Infrastruct/HotelDirectory.Hotel.Service.Infrastructure.csproj", "HotelDirectory.Hotel.Service.Infrastruct/"]
COPY ["HotelDirectory.Shared.ElasticSearch/HotelDirectory.Shared.ElasticSearch.csproj", "HotelDirectory.Shared.ElasticSearch/"]
COPY ["HotelDirectory.Reporting.Service.Business/HotelDirectory.Reporting.Service.Business.csproj", "HotelDirectory.Reporting.Service.Business/"]
COPY ["HotelDirectory.Reporting.Service.Infrastructure/HotelDirectory.Reporting.Service.Infrastructure.csproj", "HotelDirectory.Reporting.Service.Infrastructure/"]
COPY ["HotelDirectory.Reporting.Service.Consumer/HotelDirectory.Reporting.Service.Consumer.csproj", "HotelDirectory.Reporting.Service.Consumer/"]
RUN dotnet restore "./HotelDirectory.Reporting.Service.Application/HotelDirectory.Reporting.Service.Application.csproj"
COPY . .
WORKDIR "/src/HotelDirectory.Reporting.Service.Application"
RUN dotnet build "./HotelDirectory.Reporting.Service.Application.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HotelDirectory.Reporting.Service.Application.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HotelDirectory.Reporting.Service.Application.dll"]