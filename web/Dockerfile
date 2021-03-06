#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY docker-compose/wait-for-it.sh /wait-for-it.sh
RUN chmod +x /wait-for-it.sh


FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY web/web.csproj web/
RUN dotnet restore "web/web.csproj"
COPY . .
WORKDIR "/src/web"
RUN dotnet build "web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# override entrypoint in docker-compose.yml and docker-compose.vs.debug.yml
#ENTRYPOINT ["dotnet", "web.dll"]