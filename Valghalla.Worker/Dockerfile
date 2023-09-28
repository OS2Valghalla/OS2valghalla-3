#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["nuget.config", "."]
COPY ["Valghalla.Worker/Valghalla.Worker.csproj", "Valghalla.Worker/"]
COPY ["Valghalla.Application/Valghalla.Application.csproj", "Valghalla.Application/"]
COPY ["Valghalla.Integration/Valghalla.Integration.csproj", "Valghalla.Integration/"]
COPY ["Valghalla.Worker.Infrastructure/Valghalla.Worker.Infrastructure.csproj", "Valghalla.Worker.Infrastructure/"]
COPY ["Valghalla.Infrastructure/Valghalla.Infrastructure.csproj", "Valghalla.Infrastructure/"]
COPY ["Valghalla.Database/Valghalla.Database.csproj", "Valghalla.Database/"]
COPY ["Environment/", "Environment/"]
RUN dotnet restore "Valghalla.Worker/Valghalla.Worker.csproj"
COPY . .
WORKDIR "/src/Valghalla.Worker"
RUN dotnet build "Valghalla.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Valghalla.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /src/Environment /Environment
ENTRYPOINT ["dotnet", "Valghalla.Worker.dll"]