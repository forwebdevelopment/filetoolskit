
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# 1. Copy the solution file first to allow Docker to cache layers efficiently.
# Assuming the solution file is in the repository root directory (the build context)
COPY ["*.sln", "./"]

# 2. Copy all project files (*.csproj) while maintaining the original directory structure
# This allows 'dotnet restore' to resolve project-to-project references correctly.
# The paths here are relative to the build context (root of repo)
COPY ["Filetoolkits/Filetoolkits.csproj", "Filetoolkits/"]
COPY ["Filetoolkits.application/Filetoolkits.application.csproj", "Filetoolkits.application/"]
COPY ["Filetoolkits.domain/Filetoolkits.domain.csproj", "Filetoolkits.domain/"]
COPY ["Filetoolkits.infrastructure/Filetoolkits.infrastructure.csproj", "Filetoolkits.infrastructure/"]

# 3. Restore dependencies for the main project.
RUN dotnet restore "Filetoolkits/Filetoolkits.csproj"

# 4. Copy the remaining source code (source files change more frequently than project files/dependencies)
COPY . .

# 5. Build the main web project (adjust 'Filetoolkits' if your primary startup project is different)
WORKDIR /src/Filetoolkits
RUN dotnet build "Filetoolkits.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
# Use AppHost=false is often useful for container compatibility
RUN dotnet publish "Filetoolkits.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# The application is assumed to be an ASP.NET Core app listening on 8080/8081 (as per the base image)
ENTRYPOINT ["dotnet", "Filetoolkits.dll"]
