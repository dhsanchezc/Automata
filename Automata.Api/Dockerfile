# Runtime base: lightweight ASP.NET Core runtime image from Microsoft's MCR.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
# Bind Kestrel to port 8080 inside the container.
ENV ASPNETCORE_URLS=http://+:8080

# Build stage: uses the full SDK to restore, build, and publish.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# Copy solution and project files first so package restore can be cached independently of code changes.
COPY Automata.sln ./
COPY Automata.Api/Automata.Api.csproj Automata.Api/
COPY Automata.Application/Automata.Application.csproj Automata.Application/
COPY Automata.Domain/Automata.Domain.csproj Automata.Domain/
COPY Automata.Infrastructure/Automata.Infrastructure.csproj Automata.Infrastructure/
RUN dotnet restore Automata.Api/Automata.Api.csproj

# Copy remaining source and publish a Release build to /app/publish.
COPY . .
RUN dotnet publish Automata.Api/Automata.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Final stage: runtime-only image with published output copied in.
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Automata.Api.dll"]
