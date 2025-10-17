


FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Fundo.Applications.WebApi/Fundo.Applications.WebApi.csproj", "Fundo.Applications.WebApi/"]
COPY ["Fundo.Infrastructure/Fundo.Infrastructure.csproj", "Fundo.Infrastructure/"]
COPY ["Fundo.Domain/Fundo.Domain.csproj", "Fundo.Domain/"]
COPY ["Fundo.Services/Fundo.Services.csproj", "Fundo.Services/"]
COPY ["Fundo.Shared/Fundo.Shared.csproj", "Fundo.Shared/"]
RUN dotnet restore "./Fundo.Applications.WebApi/Fundo.Applications.WebApi.csproj"
COPY . .
WORKDIR "/src/Fundo.Applications.WebApi"
RUN dotnet build "./Fundo.Applications.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Fundo.Applications.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fundo.Applications.WebApi.dll"]