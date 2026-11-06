FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Dale.Api/Dale.Api.csproj", "Dale.Api/"]
RUN dotnet restore "Dale.Api/Dale.Api.csproj"

COPY . .
WORKDIR "/src/Dale.Api"
RUN dotnet publish "Dale.Api.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

USER $APP_UID
ENTRYPOINT ["dotnet", "Dale.Api.dll"]