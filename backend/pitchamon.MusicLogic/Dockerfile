
FROM mcr.microsoft.com/dotnet/sdk:8.0

# Download python
RUN apt-get update && \
    apt-get install -y python3 python3-pip && \
    ln -s /usr/bin/python3 /usr/bin/python

WORKDIR /app

COPY . .

# python dependency
RUN pip install librosa numpy soundfile

# .NET restore + build
RUN dotnet restore
RUN dotnet build -c Release

# Command
CMD ["dotnet", "run"]