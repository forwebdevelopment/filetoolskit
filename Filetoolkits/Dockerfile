# See https://aka.ms/customizecontainer to learn how to customize your debug container 
# and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# Cloud Run requires the application to listen on port 8080.
# We set the ASPNETCORE_URLs environment variable to configure this in .NET applications.
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
# EXPOSE 8081 # Not typically needed for Cloud Run deployment

# This stage is used to build the service project
FROM mcr.microsoft.com AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["Filetoolkits/Filetoolkits.csproj", "Filetoolkits/"]
RUN dotnet restore "./Filetoolkits/Filetoolkits.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/Filetoolkits"
RUN dotnet build "./Filetoolkits.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
# Consider using /p:PublishReadyToRun=true for performance optimization
RUN dotnet publish "./Filetoolkits.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Filetoolkits.dll"]
