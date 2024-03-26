FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebAPI_SensoresESP32/WebAPI_SensoresESP32.csproj", "WebAPI_SensoresESP32/"]
RUN dotnet restore "WebAPI_SensoresESP32/WebAPI_SensoresESP32.csproj"
COPY . .
WORKDIR "/src/WebAPI_SensoresESP32"
RUN dotnet build "WebAPI_SensoresESP32.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WebAPI_SensoresESP32.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPI_SensoresESP32.dll"]
