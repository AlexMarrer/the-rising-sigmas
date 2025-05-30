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

RUN dotnet tool install --global dotnet-ef

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80

# Set the PATH to include the copied tools
ENV PATH="${PATH}:/root/.dotnet/tools"

# Copy the published application from the 'publish' stage
COPY --from=publish /app/publish .
# Copy the installed dotnet-ef tool from the 'publish' stage
COPY --from=publish /root/.dotnet/tools /root/.dotnet/tools

ENTRYPOINT ["dotnet", "RisingSigma.Api.dll"]