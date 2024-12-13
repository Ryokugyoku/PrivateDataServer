# ベースイメージとしてASP.NET Coreランタイムを使用
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# ビルドイメージとしてASP.NET Core SDKを使用
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["./PrivateDataServer.csproj", "APrivateDataServer/"]
RUN dotnet restore "PrivateDataServer/PrivateDataServer.csproj"
COPY . .
WORKDIR "/src/PrivateDataServer"
RUN dotnet build "PrivateDataServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PrivateDataServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PrivateData.dll"]