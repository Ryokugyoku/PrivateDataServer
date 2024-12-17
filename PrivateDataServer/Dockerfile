# ベースイメージとして ASP.NET Core ランタイムを使用
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# ビルドイメージとして ASP.NET Core SDK を使用
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["PrivateDataServer.csproj", "./"]
RUN dotnet restore "PrivateDataServer.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "PrivateDataServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PrivateDataServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PrivateDataServer.dll"]