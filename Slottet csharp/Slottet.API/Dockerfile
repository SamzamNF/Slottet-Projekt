# Creates a Docker image for the Slottet.API project.

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy the solution and project files to the container
COPY Slottet.slnx ./
COPY Slottet.API/Slottet.API.csproj Slottet.API/
COPY Slottet.Application/Slottet.Application.csproj Slottet.Application/
COPY Slottet.Infrastructure/Slottet.Infrastructure.csproj Slottet.Infrastructure/
COPY Slottet.Domain/Slottet.Domain.csproj Slottet.Domain/
COPY Slottet.Shared/Slottet.Shared.csproj Slottet.Shared/

# Restore the project dependencies
RUN dotnet restore Slottet.API/Slottet.API.csproj

# Copy the remaining source code to the container
COPY . .
# 

RUN dotnet publish Slottet.API/Slottet.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Slottet.API.dll"]