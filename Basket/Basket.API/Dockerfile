#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Basket/Basket.API/Basket.API.csproj", "Basket/Basket.API/"]
COPY ["Infrastructure/Common.Logging/Common.Logging.csproj", "Infrastructure/Common.Logging/"]
COPY ["Basket/Basket.Application/Basket.Application.csproj", "Basket/Basket.Application/"]
COPY ["Infrastructure/EventBuss.Messages/EventBuss.Messages.csproj", "Infrastructure/EventBuss.Messages/"]
COPY ["Basket/Basket.Core/Basket.Core.csproj", "Basket/Basket.Core/"]
COPY ["Basket/Basket.Infrastructure/Basket.Infrastructure.csproj", "Basket/Basket.Infrastructure/"]
RUN dotnet restore "./Basket/Basket.API/Basket.API.csproj"
COPY . .
WORKDIR "/src/Basket/Basket.API"
RUN dotnet build "./Basket.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Basket.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basket.API.dll"]