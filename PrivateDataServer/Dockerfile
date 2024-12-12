# ベースイメージとしてASP.NET Coreランタイムを使用
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# ビルドイメージとしてASP.NET Core SDKを使用
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["./AiWebServer.csproj", "AiWebServer/"]
RUN dotnet restore "AiWebServer/AiWebServer.csproj"
COPY . .
WORKDIR "/src/AiWebServer"
RUN dotnet build "AiWebServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AiWebServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AiWebServer.dll"]