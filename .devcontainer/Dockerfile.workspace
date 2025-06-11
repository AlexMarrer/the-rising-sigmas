FROM mcr.microsoft.com/dotnet/sdk:8.0
 
# Set environment variables

ENV DEBIAN_FRONTEND=noninteractive

ENV PATH="${PATH}:/root/.dotnet/tools"
 
# Install Node.js 20.x, PHP, and other dependencies in a single layer

RUN curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
&& apt-get update \
&& apt-get install -y --no-install-recommends \

        nodejs \

        git \

        curl \

        unzip \

        procps \

        wget \

        ca-certificates \

        vim \

        php \

        php-cli \

        php-pgsql \

        php-mysql \

        php-sqlite3 \
&& apt-get clean \
&& rm -rf /var/lib/apt/lists/*
 
# Install dotnet tools, npm packages, and generate dev certificate

RUN dotnet tool install -g dotnet-ef \
&& dotnet tool install -g dotnet-aspnet-codegenerator \
&& npm install -g @ionic/cli@7.2.0 \
&& npm install -g @angular/cli@18.0.7 \
&& npm install -g nodemon@3.1.7 \
&& npm install -g npm-run-all@4.1.5 \
&& (dotnet dev-certs https --trust || true)
 
# Set working directory

WORKDIR /workspace
 
# Add labels for better documentation

LABEL description="Development container for Ionic/Angular 18 + .NET 8 with Node.js 20, PHP and development tools"

LABEL maintainer="Sigmas"

LABEL version="1.0.0"
 