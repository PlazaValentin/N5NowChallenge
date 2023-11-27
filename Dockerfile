FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /src
EXPOSE 80
EXPOSE 443

COPY . ./

RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS publish
WORKDIR /app

COPY --from=build-env /src/out .

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final-env
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "N5NowChallenge.API.dll"]