# Base runtime image (Cloud Run)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj and restore
COPY ["Filetoolkits/Filetoolkits.csproj", "Filetoolkits/"]
RUN dotnet restore "Filetoolkits/Filetoolkits.csproj"

# Copy remaining files and build
COPY . .
WORKDIR "/src/Filetoolkits"
RUN dotnet build "Filetoolkits.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Filetoolkits.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Filetoolkits.dll"]
