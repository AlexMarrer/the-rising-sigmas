# Build stage
FROM node:24-slim AS build

WORKDIR /app

COPY package*.json ./
RUN npm ci --silent \
    && npm cache clean --force

# Copy all source files (dockerignore excludes compiled files)
COPY . .

RUN npm run build --prod --silent

# Production stage
FROM nginx:alpine AS production

COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/www /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]