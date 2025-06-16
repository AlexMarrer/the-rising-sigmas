FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["RisingSigma/RisingSigma.API/RisingSigma.Api.csproj", "RisingSigma.API/"]
COPY ["RisingSigma/RisingSigma.Database/RisingSigma.Database.csproj", "RisingSigma.Database/"]

RUN dotnet restore "RisingSigma.API/RisingSigma.Api.csproj"

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

# Only copy the necessary files from the publish stage
COPY --from=publish --chown=appuser:appuser /app/publish .
COPY --from=publish --chown=appuser:appuser /root/.dotnet/tools /home/appuser/.dotnet/tools

USER appuser
EXPOSE 80

ENTRYPOINT ["dotnet", "RisingSigma.Api.dll"]