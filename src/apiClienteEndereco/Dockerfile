#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/apiClienteEndereco/apiClienteEndereco.csproj", "src/apiClienteEndereco/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Data/Data.csproj", "src/Data/"]
COPY ["src/dominio/dominio.csproj", "src/dominio/"]
COPY ["src/Infraestructure/Infraestructure.csproj", "src/Infraestructure/"]
RUN dotnet restore "src/apiClienteEndereco/apiClienteEndereco.csproj"
COPY . .
WORKDIR "/src/src/apiClienteEndereco"
RUN dotnet build "apiClienteEndereco.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "apiClienteEndereco.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "apiClienteEndereco.dll"]