FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/user-details-service/user-details-service.csproj", "src/user-details-service/"]
RUN dotnet restore "src/user-details-service/user-details-service.csproj"
COPY ["src/application-infrastructure/application-infrastructure.csproj", "src/application-infrastructure/"]
RUN dotnet restore "src/application-infrastructure/application-infrastructure.csproj"
COPY . .
WORKDIR "/src/src/user-details-service"
RUN dotnet build "user-details-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "user-details-service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "user-details-service.dll"]
