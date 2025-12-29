# Base runtime image for Cloud Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj and restore (IMPORTANT PATH)
COPY ["Filetoolkits/Filetoolkits.csproj", "Filetoolkits/"]
RUN dotnet restore "Filetoolkits/Filetoolkits.csproj"

# Copy remaining files
COPY . .

# Build
WORKDIR "/src/Filetoolkits"
RUN dotnet build "Filetoolkits.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Filetoolkits.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Filetoolkits.dll"]
