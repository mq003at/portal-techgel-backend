# ----------- Build stage -------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy everything and restore
COPY . .
RUN dotnet restore

# Build and publish to /app/out
RUN dotnet publish -c Release -o /app/out

# ----------- Runtime stage -------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy built output from previous stage
COPY --from=build /app/out .

# Set environment port for Railway
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
RUN echo "Contents of /app:" && ls -R /app

# Run the app
ENTRYPOINT ["dotnet", "out/portal-techgel-backend.dll"]
