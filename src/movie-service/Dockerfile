FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
ENV ASPNETCORE_ENVIRONMENT = Development
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/movie-service/movie-service.csproj", "src/movie-service/"]
RUN dotnet restore "src/movie-service/movie-service.csproj"
COPY ["src/application-infrastructure/application-infrastructure.csproj", "src/application-infrastructure/"]
RUN dotnet restore "src/application-infrastructure/application-infrastructure.csproj"
COPY . .
WORKDIR "/src/src/movie-service"
RUN dotnet build "movie-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "movie-service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "movie-service.dll" , "--environment=Development"]
