#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/backend/presentation/DropMate.WebAPI/DropMate.WebAPI.csproj", "src/backend/presentation/DropMate.WebAPI/"]
COPY ["src/backend/core/LoggerService/LoggerService.csproj", "src/backend/core/LoggerService/"]
COPY ["src/backend/core/DropMate.Application/DropMate.Application.csproj", "src/backend/core/DropMate.Application/"]
COPY ["src/backend/core/DropMate.Domain/DropMate.Domain.csproj", "src/backend/core/DropMate.Domain/"]
COPY ["src/backend/core/DropMate.Shared/DropMate.Shared.csproj", "src/backend/core/DropMate.Shared/"]
COPY ["src/backend/infrastructure/DropMate.Persistence/DropMate.Persistence.csproj", "src/backend/infrastructure/DropMate.Persistence/"]
COPY ["src/backend/infrastructure/DropMate.Service/DropMate.Service.csproj", "src/backend/infrastructure/DropMate.Service/"]
COPY ["src/backend/presentation/DropMate.ControllerEndPoints/DropMate.ControllerEndPoints.csproj", "src/backend/presentation/DropMate.ControllerEndPoints/"]
RUN dotnet restore "src/backend/presentation/DropMate.WebAPI/DropMate.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/backend/presentation/DropMate.WebAPI"
RUN dotnet build "DropMate.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DropMate.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DropMate.WebAPI.dll"]