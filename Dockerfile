# 👷 Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0.201 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# Copy only the project file and restore dependencies
COPY ["portal-techgel-api.csproj", "./"]
RUN dotnet restore "portal-techgel-api.csproj"

# Copy everything else from the root (including DTO/, config/, etc.)
COPY . .

# Build the application
RUN dotnet build "portal-techgel-api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 🚀 Publish stage
FROM build AS publish
RUN dotnet publish "portal-techgel-api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 🏃 Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0.201 AS final
WORKDIR /app

# Copy published app from previous stage
COPY --from=publish /app/publish .

# Set environment for Railway (listens on port 8080)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Run the application
ENTRYPOINT ["dotnet", "portal-techgel-api.dll"]
