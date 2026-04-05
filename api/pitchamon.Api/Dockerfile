FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["api/pitchamon.Api/pitchamon.Api.csproj", "api/pitchamon.Api/"]
RUN dotnet restore "api/pitchamon.Api/pitchamon.Api.csproj"
COPY . .
WORKDIR "/src/api/pitchamon.Api"
RUN dotnet build "./pitchamon.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./pitchamon.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY api/pitchamon.Api/Assets/Cries ./Assets/Cries
ENTRYPOINT ["dotnet", "pitchamon.Api.dll"]