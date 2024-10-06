FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8082

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/SachkovTech.Web/SachkovTech.Web.csproj", "src/SachkovTech.Web/"]
COPY ["src/Accounts/SachkovTech.Accounts.Infrastructure/SachkovTech.Accounts.Infrastructure.csproj", "src/Accounts/SachkovTech.Accounts.Infrastructure/"]
COPY ["src/Accounts/SachkovTech.Accounts.Application/SachkovTech.Accounts.Application.csproj", "src/Accounts/SachkovTech.Accounts.Application/"]
COPY ["src/Shared/SachkovTech.Core/SachkovTech.Core.csproj", "src/Shared/SachkovTech.Core/"]
COPY ["src/Shared/SachkovTech.SharedKernel/SachkovTech.SharedKernel.csproj", "src/Shared/SachkovTech.SharedKernel/"]
COPY ["src/Accounts/SachkovTech.Accounts.Domain/SachkovTech.Accounts.Domain.csproj", "src/Accounts/SachkovTech.Accounts.Domain/"]
COPY ["src/Accounts/SachkovTech.Accounts.Presentation/SachkovTech.Accounts.Presentation.csproj", "src/Accounts/SachkovTech.Accounts.Presentation/"]
COPY ["src/Shared/SachkovTech.Framework/SachkovTech.Framework.csproj", "src/Shared/SachkovTech.Framework/"]
COPY ["src/Accounts/SachkovTech.Accounts.Contracts/SachkovTech.Accounts.Contracts.csproj", "src/Accounts/SachkovTech.Accounts.Contracts/"]
COPY ["src/Issues/SachkovTech.Issues.Infrastructure/SachkovTech.Issues.Infrastructure.csproj", "src/Issues/SachkovTech.Issues.Infrastructure/"]
COPY ["src/Issues/SachkovTech.Issues.Application/SachkovTech.Issues.Application.csproj", "src/Issues/SachkovTech.Issues.Application/"]
COPY ["src/Issues/SachkovTech.Issues.Domain/SachkovTech.Issues.Domain.csproj", "src/Issues/SachkovTech.Issues.Domain/"]
COPY ["src/Issues/SachkovTech.Issues.Presentation/SachkovTech.Issues.Presentation.csproj", "src/Issues/SachkovTech.Issues.Presentation/"]
RUN dotnet restore "src/SachkovTech.Web/SachkovTech.Web.csproj"
COPY . .
WORKDIR "/src/src/SachkovTech.Web"
RUN dotnet build "SachkovTech.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SachkovTech.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SachkovTech.Web.dll"]