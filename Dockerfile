FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
#EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS with-node
RUN apt-get update
RUN apt-get install curl
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash
RUN apt-get -y install nodejs


FROM with-node AS files-required
WORKDIR /src
COPY ["web-api/", "web-api/"]
COPY ["app-react/public/", "app-react/public/"]
COPY ["app-react/src/", "app-react/src/"]
COPY ["app-react/package*.json", "app-react/"]
COPY ["app-react/tsconfig.json", "app-react/"]

FROM files-required AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/web-api"

RUN dotnet restore "./Cliente.API/Cliente.API.csproj"
RUN dotnet build "./Cliente.API/Cliente.API.csproj" -c $BUILD_CONFIGURATION -o /app/web-api/build
RUN dotnet publish "./Cliente.API/Cliente.API.csproj" -c $BUILD_CONFIGURATION -o /app/web-api/publish /p:UseAppHost=false

WORKDIR "/src/app-react"
#RUN rm -rf node_modules
#RUN rm -f package-lock.json
RUN npm install
RUN npm run build

FROM base AS final
WORKDIR /app
COPY --from=publish /app/web-api/publish .
COPY --from=publish /src/app-react/build /app/wwwroot
ENTRYPOINT ["dotnet", "Cliente.API.dll"]