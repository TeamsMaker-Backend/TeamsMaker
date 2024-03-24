# Use the SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS base
WORKDIR /src

# Copy the entire content of the "src" directory
COPY . .

# Restore dependencies
RUN dotnet restore

# Build the application
RUN dotnet publish "TeamsMaker.Api.csproj" -c Release -o /app/publish/

# Use the runtime image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS publish
WORKDIR /app
EXPOSE 8080 443

# Copy the published output from the build stage
COPY --from=base /app/publish .

# Start the application
ENTRYPOINT ["dotnet", "TeamsMaker.Api.dll"]
