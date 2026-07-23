# Etapa 1: Imagen base ligera para ejecutar la app
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

# Etapa 2: Imagen con el SDK completo para compilar el código
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["GimnasioFit.Api/GimnasioFit.Api.csproj", "GimnasioFit.Api/"]
COPY ["GimnasioFit.Core/GimnasioFit.Core.csproj", "GimnasioFit.Core/"]
COPY ["GimnasioFit.Infrastructure/GimnasioFit.Infrastructure.csproj", "GimnasioFit.Infrastructure/"]

RUN dotnet restore "GimnasioFit.Api/GimnasioFit.Api.csproj"
COPY . .
WORKDIR "/src/GimnasioFit.Api"
RUN dotnet build "GimnasioFit.Api.csproj" -c Release -o /app/build

# Etapa 3: Publicación del binario
FROM build AS publish
RUN dotnet publish "GimnasioFit.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 4: Copiar el binario a la imagen ligera final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GimnasioFit.Api.dll"]