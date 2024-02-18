# Stage 1: Build the React application
FROM node:latest as build

WORKDIR /app

COPY package.json package-lock.json ./

RUN npm install

COPY .env.production .env
COPY . ./

RUN npm run build

# Stage 2: Serve the app using 'http-server'
FROM node:latest

WORKDIR /app

# Install 'http-server' globally
RUN npm install -g http-server

# Copy the built React app from the build stage
COPY --from=build /app/dist ./dist

# Expose port 8173
EXPOSE 80

# Start the web server to serve the React app on port 80
CMD ["http-server", "./dist", "-p", "80", "-d", "false"]
