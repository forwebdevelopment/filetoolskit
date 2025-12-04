# Base image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy all project files
COPY ["Filetoolkits/Filetoolkits.csproj", "Filetoolkits/"]
COPY ["Filetoolkits.application/Filetoolkits.application.csproj", "Filetoolkits.application/"]
COPY ["Filetoolkits.domain/Filetoolkits.domain.csproj", "Filetoolkits.domain/"]
COPY ["Filetoolkits.infrastructure/Filetoolkits.infrastructure.csproj", "Filetoolkits.infrastructure/"]

# Restore dependencies
RUN dotnet restore "Filetoolkits/Filetoolkits.csproj"

# Copy everything else
COPY . .

# Build the Web project
WORKDIR "/src/Filetoolkits"
RUN dotnet build "Filetoolkits.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Filetoolkits.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Filetoolkits.dll"]
