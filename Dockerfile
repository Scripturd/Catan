FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish src/Catan.Server/Catan.Server.csproj -c Release -o /app \
 && mkdir -p /app/plugins \
 && cp src/Catan.Modes.Standard/bin/Release/net9.0/Catan.Modes.Standard.dll /app/plugins/ \
 && cp src/Catan.Modes.Seafarers/bin/Release/net9.0/Catan.Modes.Seafarers.dll /app/plugins/ \
 && cp src/Catan.Modes.Mini/bin/Release/net9.0/Catan.Modes.Mini.dll /app/plugins/

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Catan.Server.dll"]
