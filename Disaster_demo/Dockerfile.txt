# --- Build stage ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY Disaster_demo/Disaster_demo.csproj Disaster_demo/
RUN dotnet restore Disaster_demo/Disaster_demo.csproj

# Copy the full source and publish
COPY . . 
RUN dotnet publish Disaster_demo/Disaster_demo.csproj -c Release -o /app/publish

# --- Runtime stage ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

# Expose port 8080 for Railway
EXPOSE 8080

ENTRYPOINT ["dotnet", "Disaster_demo.dll"]
