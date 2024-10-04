FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/SachkovTech.API/SachkovTech.API.csproj", "src/SachkovTech.API/"]
COPY ["src/SachkovTech.Application/SachkovTech.Application.csproj", "src/SachkovTech.Application/"]
COPY ["src/SachkovTech.Domain/SachkovTech.Domain.csproj", "src/SachkovTech.Domain/"]
COPY ["src/SachkovTech.Infrastructure/SachkovTech.Infrastructure.csproj", "src/SachkovTech.Infrastructure/"]
RUN dotnet restore "src/SachkovTech.API/SachkovTech.API.csproj"
COPY . .
WORKDIR "/src/src/SachkovTech.API"
RUN dotnet build "SachkovTech.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SachkovTech.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SachkovTech.API.dll"]
