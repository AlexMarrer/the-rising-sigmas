FROM mcr.microsoft.com/mssql/server:2022-latest

ENV ACCEPT_EULA=Y \
    DEBIAN_FRONTEND=noninteractive \
    PATH="$PATH:/opt/mssql-tools/bin"

USER root

RUN apt-get update \
    && apt-get install -y --no-install-recommends\
    curl \
    apt-transport-https \
    gnupg2 \
    unixodbc-dev \
    ca-certificates\
    && curl -fsSL https://packages.microsoft.com/keys/microsoft.asc \
    | gpg --dearmor -o /usr/share/keyrings/microsoft-prod.gpg \
    && echo "deb [arch=amd64,arm64,armhf signed-by=/usr/share/keyrings/microsoft-prod.gpg] https://packages.microsoft.com/ubuntu/22.04/prod jammy main" \
    > /etc/apt/sources.list.d/mssql-tools.list \
    && apt-get update \
    && apt-get install -y --no-install-recommends \
    mssql-tools18 \
    mssql-tools \
    msodbcsql18 \
    msodbcsql17 \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/* \
    && rm -rf /tmp/* \
    && rm -rf /var/tmp/*


USER mssql