# Creates a Docker image for the Slottet.Frontend project.

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy the solution and project files to the container
COPY Slottet.slnx ./
COPY Slottet.Frontend/Slottet.Frontend.csproj Slottet.Frontend/
COPY Slottet.Shared/Slottet.Shared.csproj Slottet.Shared/

# Restore the project dependencies
RUN dotnet restore Slottet.Frontend/Slottet.Frontend.csproj

# Copy the remaining source code to the container
COPY . .
RUN dotnet publish Slottet.Frontend/Slottet.Frontend.csproj -c Release -o /app/publish /p:UseAppHost=false

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Slottet.Frontend.dll"]