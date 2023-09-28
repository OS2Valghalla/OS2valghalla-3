#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["nuget.config", "."]
COPY ["Valghalla.External.API/Valghalla.External.API.csproj", "Valghalla.External.API/"]
COPY ["Valghalla.Integration/Valghalla.Integration.csproj", "Valghalla.Integration/"]
COPY ["Valghalla.Application/Valghalla.Application.csproj", "Valghalla.Application/"]
COPY ["Valghalla.External.Application/Valghalla.External.Application.csproj", "Valghalla.External.Application/"]
COPY ["Valghalla.External.Infrastructure/Valghalla.External.Infrastructure.csproj", "Valghalla.External.Infrastructure/"]
COPY ["Valghalla.Infrastructure/Valghalla.Infrastructure.csproj", "Valghalla.Infrastructure/"]
COPY ["Valghalla.Database/Valghalla.Database.csproj", "Valghalla.Database/"]
COPY ["Environment/", "Environment/"]
RUN dotnet restore "Valghalla.External.API/Valghalla.External.API.csproj"
COPY . .
WORKDIR "/src/Valghalla.External.API"
RUN dotnet build "Valghalla.External.API.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "Valghalla.External.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM node as ng-build
WORKDIR /src
COPY ["Valghalla.External.Web/", "Valghalla.External.Web/"]
WORKDIR /src/Valghalla.External.Web
RUN npm install
COPY . .
RUN npm run build

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=ng-build /src/Valghalla.External.Web/dist/src /app/wwwroot
COPY --from=ng-build /src/Valghalla.External.Web/Environment /Environment
ENTRYPOINT ["dotnet", "Valghalla.External.API.dll"]