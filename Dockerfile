# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything
COPY . .

# Move into project folder
WORKDIR /src/AuthCrudApp

# Restore & Publish
RUN dotnet restore
RUN dotnet publish -c Release -o /app/out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "AuthCrudApp.dll"]
