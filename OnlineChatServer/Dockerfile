FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OnlineChat/OnlineChat.csproj", "OnlineChat/"]
COPY ["OnlineChat.Application/OnlineChat.Application.csproj", "OnlineChat.Application/"]
COPY ["OnlineChat.DatabaseAccess/OnlineChat.DatabaseAccess.csproj", "OnlineChat.DatabaseAccess/"]
RUN dotnet restore "OnlineChat/OnlineChat.csproj"
COPY . .
WORKDIR "/src/OnlineChat"
RUN dotnet build "./OnlineChat.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OnlineChat.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnlineChat.dll"]
