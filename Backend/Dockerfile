FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files first for better layer caching
COPY ["RisingSigma/RisingSigma.API/RisingSigma.Api.csproj", "RisingSigma.API/"]
COPY ["RisingSigma/RisingSigma.Database/RisingSigma.Database.csproj", "RisingSigma.Database/"]
COPY ["RisingSigma/RisingSigma.sln", "./"]

# Restore dependencies
RUN dotnet restore "RisingSigma.API/RisingSigma.Api.csproj"

# Copy source code (dockerignore excludes bin/obj)
COPY RisingSigma/ .

WORKDIR "/src/RisingSigma.API"
RUN dotnet build "RisingSigma.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RisingSigma.Api.csproj" -c Release -o /app/publish

RUN dotnet tool install --global dotnet-ef --version 8.0.*

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Non-root User
RUN adduser --disabled-password --gecos '' appuser \
    && chown -R appuser:appuser /app

ENV PATH="${PATH}:/home/appuser/.dotnet/tools" \
    DOTNET_ENVIRONMENT=Production \
    ASPNETCORE_URLS=http://+:80

USER appuser

# Only copy the necessary files from the publish stage
COPY --chown=appuser:appuser --from=publish /app/publish .
COPY --chown=appuser:appuser --from=publish /root/.dotnet/tools /home/appuser/.dotnet/tools

EXPOSE 80

ENTRYPOINT ["dotnet", "RisingSigma.Api.dll"]