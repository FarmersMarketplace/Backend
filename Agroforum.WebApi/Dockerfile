#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Agroforum.WebApi/Agroforum.WebApi.csproj", "Agroforum.WebApi/"]
COPY ["Agroforum.Application/Agroforum.Application.csproj", "Agroforum.Application/"]
COPY ["Agroforum.Domain/Agroforum.Domain.csproj", "Agroforum.Domain/"]
COPY ["Agroforum.Persistence/Agroforum.Persistence.csproj", "Agroforum.Persistence/"]
RUN dotnet restore "Agroforum.WebApi/Agroforum.WebApi.csproj"
COPY . .
WORKDIR "/src/Agroforum.WebApi"
RUN dotnet build "Agroforum.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Agroforum.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Agroforum.WebApi.dll"]