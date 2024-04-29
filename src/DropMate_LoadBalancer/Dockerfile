#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/DropMate_LoadBalancer/DropMate_LoadBalancer.csproj", "src/DropMate_LoadBalancer/"]
RUN dotnet restore "./src/DropMate_LoadBalancer/DropMate_LoadBalancer.csproj"
COPY . .
WORKDIR "/src/src/DropMate_LoadBalancer"
RUN dotnet build "./DropMate_LoadBalancer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DropMate_LoadBalancer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DropMate_LoadBalancer.dll"]