#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["Developer.API/Developer.API.csproj", "Developer.API/"]
COPY ["Developer.Service/Developer.Service.csproj", "Developer.Service/"]

RUN dotnet restore "Developer.API/Developer.API.csproj"
RUN dotnet restore "Developer.Service/Developer.Service.csproj"

COPY . .

WORKDIR "/src/Developer.API"
RUN dotnet build "./Developer.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "./Developer.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Developer.API.dll"]