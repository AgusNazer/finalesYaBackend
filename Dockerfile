# Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["finalesYaBackend.csproj", "./"]
RUN dotnet restore "finalesYaBackend.csproj"
COPY . .
RUN dotnet build "finalesYaBackend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "finalesYaBackend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "finalesYaBackend.dll"]