#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/backend2/presentation/DropMate2.WebAPI/DropMate2.WebAPI.csproj", "src/backend2/presentation/DropMate2.WebAPI/"]
COPY ["src/backend2/core/DropMate2.LoggerService/DropMate2.LoggerService.csproj", "src/backend2/core/DropMate2.LoggerService/"]
COPY ["src/backend2/core/DropMate2.Application/DropMate2.Application.csproj", "src/backend2/core/DropMate2.Application/"]
COPY ["src/backend2/core/DropMate2.Domain/DropMate2.Domain.csproj", "src/backend2/core/DropMate2.Domain/"]
COPY ["src/backend2/core/DropMate2.Shared/DropMate2.Shared.csproj", "src/backend2/core/DropMate2.Shared/"]
COPY ["src/backend2/infrastructure/DropMate2.Persistence/DropMate2.Persistence.csproj", "src/backend2/infrastructure/DropMate2.Persistence/"]
COPY ["src/backend2/infrastructure/DropMate2.Service/DropMate2.Service.csproj", "src/backend2/infrastructure/DropMate2.Service/"]
COPY ["src/backend2/presentation/DropMate2.ControllerEndPoints/DropMate2.ControllerEndPoints.csproj", "src/backend2/presentation/DropMate2.ControllerEndPoints/"]
RUN dotnet restore "src/backend2/presentation/DropMate2.WebAPI/DropMate2.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/backend2/presentation/DropMate2.WebAPI"
RUN dotnet build "DropMate2.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DropMate2.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DropMate2.WebAPI.dll"]