#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Discount/Discount.API/Discount.API.csproj", "Discount/Discount.API/"]
COPY ["Discount/Discount.Application/Discount.Application.csproj", "Discount/Discount.Application/"]
COPY ["Discount/Discount.Core/Discount.Core.csproj", "Discount/Discount.Core/"]
COPY ["Discount/Discount.Infrastructure/Discount.Infrastructure.csproj", "Discount/Discount.Infrastructure/"]
RUN dotnet restore "Discount/Discount.API/Discount.API.csproj"
COPY . .
WORKDIR "/src/Discount/Discount.API"
RUN dotnet build "Discount.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Discount.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Discount.API.dll"]