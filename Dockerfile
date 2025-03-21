# Use official .NET 9.0 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

# Copy built app
COPY ./bin/Release/net9.0/publish/ .

# Set environment port for Railway
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Run the app
ENTRYPOINT ["dotnet", "portal-techgel-api.dll"]
