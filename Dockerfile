# Multi-stage build for LinhaExpressa.API
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files first for better layer caching
COPY LinhaExpressa.sln ./
COPY src/LinhaExpressa.API/LinhaExpressa.API.csproj      src/LinhaExpressa.API/
COPY src/LinhaExpressa.Application/LinhaExpressa.Application.csproj src/LinhaExpressa.Application/
COPY src/LinhaExpressa.Domain/LinhaExpressa.Domain.csproj src/LinhaExpressa.Domain/
COPY src/LinhaExpressa.Infrastructure/LinhaExpressa.Infrastructure.csproj src/LinhaExpressa.Infrastructure/
COPY tests/LinhaExpressa.Tests/LinhaExpressa.Tests.csproj tests/LinhaExpressa.Tests/

RUN dotnet restore LinhaExpressa.sln

# Copy everything else and publish
COPY . .
RUN dotnet publish src/LinhaExpressa.API/LinhaExpressa.API.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "LinhaExpressa.API.dll"]
