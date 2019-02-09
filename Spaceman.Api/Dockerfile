FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Spaceman.Api/Spaceman.Api.csproj", "Spaceman.Api/"]
COPY ["Spaceman.Service/Spaceman.Service.csproj", "Spaceman.Service/"]
RUN dotnet restore "Spaceman.Api/Spaceman.Api.csproj"
COPY . .
WORKDIR "/src/Spaceman.Api"
RUN dotnet build "Spaceman.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Spaceman.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Spaceman.Api.dll"]