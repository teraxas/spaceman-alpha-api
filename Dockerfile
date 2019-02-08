FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["SpacemanAPI/Spaceman.Api.csproj", "SpacemanAPI/"]
COPY ["Spaceman.Service/Spaceman.Service.csproj", "Spaceman.Service/"]
RUN dotnet restore "SpacemanAPI/Spaceman.Api.csproj"
COPY . .
WORKDIR "/src/SpacemanAPI"
RUN dotnet build "Spaceman.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Spaceman.Api.csproj" -c Release -o /app

FROM base AS final
ENV ASPNETCORE_URLS http://*:5000
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Spaceman.Api.dll"]