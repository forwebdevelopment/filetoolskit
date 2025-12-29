# ================================
# Runtime image (Cloud Run)
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Cloud Run listens on port 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080


# ================================
# Build image
# ================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY ["Filetoolkits.csproj", "./"]
RUN dotnet restore

# Copy all source code
COPY . .

# Build & publish
RUN dotnet publish \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false


# ================================
# Final image
# ================================
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Filetoolkits.dll"]
