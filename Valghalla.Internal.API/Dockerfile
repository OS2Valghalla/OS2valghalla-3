#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["nuget.config", "."]
COPY ["Valghalla.Internal.API/Valghalla.Internal.API.csproj", "Valghalla.Internal.API/"]
COPY ["Valghalla.Integration/Valghalla.Integration.csproj", "Valghalla.Integration/"]
COPY ["Valghalla.Application/Valghalla.Application.csproj", "Valghalla.Application/"]
COPY ["Valghalla.Internal.Application/Valghalla.Internal.Application.csproj", "Valghalla.Internal.Application/"]
COPY ["Valghalla.Internal.Infrastructure/Valghalla.Internal.Infrastructure.csproj", "Valghalla.Internal.Infrastructure/"]
COPY ["Valghalla.Infrastructure/Valghalla.Infrastructure.csproj", "Valghalla.Infrastructure/"]
COPY ["Valghalla.Database/Valghalla.Database.csproj", "Valghalla.Database/"]
COPY ["Environment/", "Environment/"]
RUN dotnet restore "Valghalla.Internal.API/Valghalla.Internal.API.csproj"
COPY . .
WORKDIR "/src/Valghalla.Internal.API"
RUN dotnet build "Valghalla.Internal.API.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "Valghalla.Internal.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM node:20 as ng-build
WORKDIR /src
COPY ["Valghalla.Internal.Web/", "Valghalla.Internal.Web/"]
WORKDIR /src/Valghalla.Internal.Web
RUN npm install
COPY . .
RUN npm run build

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=ng-build /src/Valghalla.Internal.Web/dist/src /app/wwwroot
COPY --from=ng-build /src/Valghalla.Internal.Web/Environment /Environment
ENTRYPOINT ["dotnet", "Valghalla.Internal.API.dll"]