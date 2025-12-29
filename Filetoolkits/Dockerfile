# ===============================
# Runtime
# ===============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# ===============================
# Build
# ===============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY Filetoolkits/Filetoolkits.csproj Filetoolkits/
COPY Filetoolkits.application/Filetoolkits.application.csproj Filetoolkits.application/
COPY Filetoolkits.domain/Filetoolkits.domain.csproj Filetoolkits.domain/
COPY Filetoolkits.infrastructure/Filetoolkits.infrastructure.csproj Filetoolkits.infrastructure/

# Restore
RUN dotnet restore Filetoolkits/Filetoolkits.csproj

# Copy everything
COPY . .

# Publish
WORKDIR /src/Filetoolkits
RUN dotnet publish Filetoolkits.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# ===============================
# Final
# ===============================
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Filetoolkits.dll"]
