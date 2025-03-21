# Use the .NET 9.0 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy everything and restore
COPY . ./
RUN dotnet restore

# Build and publish the app to /app folder
RUN dotnet publish -c Release -o /app --no-restore

# Use the ASP.NET runtime image for final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copy the published output from the build container
COPY --from=build /app ./

# Expose port (Railway will override this if needed)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "portal-techgel-api.dll"]
