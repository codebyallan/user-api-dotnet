FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY User.sln ./
COPY User.Api/User.Api.csproj User.Api/
RUN dotnet restore User.Api/User.Api.csproj
COPY . .
RUN dotnet publish User.Api/User.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet","User.Api.dll"]
