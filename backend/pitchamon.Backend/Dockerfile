FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8081

RUN apt-get update && apt-get install -y \
    python3 \
    python3-pip \
    python3-venv \
    libsndfile1 \
    ffmpeg \
    && rm -rf /var/lib/apt/lists/* 

ENV VIRTUAL_ENV=/opt/venv
RUN python3 -m venv $VIRTUAL_ENV
ENV PATH="$VIRTUAL_ENV/bin:$PATH"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["backend/pitchamon.Backend/pitchamon.Backend.csproj", "backend/pitchamon.Backend/"]
RUN dotnet restore "backend/pitchamon.Backend/pitchamon.Backend.csproj"

COPY . .
WORKDIR "/src/backend/pitchamon.Backend"
RUN dotnet build "./pitchamon.Backend.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./pitchamon.Backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish ./
COPY backend/pitchamon.Backend/audio_processor.py ./audio_processor.py
COPY backend/pitchamon.Backend/requirements.txt ./requirements.txt

RUN pip install --no-cache-dir -r requirements.txt

ENTRYPOINT ["dotnet", "pitchamon.Backend.dll"]