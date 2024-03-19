FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base
WORKDIR /app
EXPOSE 80 443

# ENV ASPNETCORE_URLS=http://+:80

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
# RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
# USER appuser

# FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS publish
WORKDIR /src
COPY src/ .
WORKDIR "/src/TeamsMaker.Api"
RUN dotnet publish "TeamsMaker.Api.csproj" -c Release -o /app/publish



FROM base AS final
WORKDIR /app 
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeamsMaker.Api.dll"]
 