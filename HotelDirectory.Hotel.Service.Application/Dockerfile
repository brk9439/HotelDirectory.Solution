#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HotelDirectory.Hotel.Service.Application/HotelDirectory.Hotel.Service.Application.csproj", "HotelDirectory.Hotel.Service.Application/"]
COPY ["HotelDirectory.Hotel.Service.Business/HotelDirectory.Hotel.Service.Business.csproj", "HotelDirectory.Hotel.Service.Business/"]
COPY ["HotelDirectory.Hotel.Service.Infrastruct/HotelDirectory.Hotel.Service.Infrastructure.csproj", "HotelDirectory.Hotel.Service.Infrastruct/"]
COPY ["HotelDirectory.Shared.Common/HotelDirectory.Shared.Common.csproj", "HotelDirectory.Shared.Common/"]
COPY ["HotelDirectory.Shared.ElasticSearch/HotelDirectory.Shared.ElasticSearch.csproj", "HotelDirectory.Shared.ElasticSearch/"]
RUN dotnet restore "./HotelDirectory.Hotel.Service.Application/HotelDirectory.Hotel.Service.Application.csproj"
COPY . .
WORKDIR "/src/HotelDirectory.Hotel.Service.Application"
RUN dotnet build "./HotelDirectory.Hotel.Service.Application.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HotelDirectory.Hotel.Service.Application.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HotelDirectory.Hotel.Service.Application.dll"]