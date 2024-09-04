#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["API.Gateways/Ocelot.ApiGateway/Ocelot.ApiGateway.csproj", "API.Gateways/Ocelot.ApiGateway/"]
RUN dotnet restore "API.Gateways/Ocelot.ApiGateway/Ocelot.ApiGateway.csproj"
COPY . .
WORKDIR "/src/API.Gateways/Ocelot.ApiGateway"
RUN dotnet build "Ocelot.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ocelot.ApiGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ocelot.ApiGateway.dll"]