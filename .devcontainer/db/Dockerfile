FROM mcr.microsoft.com/mssql/server:2022-latest

ENV ACCEPT_EULA=Y \
    DEBIAN_FRONTEND=noninteractive \
    PATH="$PATH:/opt/mssql-tools/bin"

USER root

RUN apt-get update \
 && apt-get install -y curl apt-transport-https gnupg2 unixodbc-dev \
 && curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - \
 && curl https://packages.microsoft.com/config/ubuntu/22.04/prod.list \
      > /etc/apt/sources.list.d/mssql-tools.list \
 && apt-get update \
 && apt-get install -y mssql-tools msodbcsql17 \
 && rm -rf /var/lib/apt/lists/*

USER mssql