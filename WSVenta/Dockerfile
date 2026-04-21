# Etapa 1: Base de ejecución con parches de seguridad (por eso root)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
# Actualizamos paquetes para corregir los CVE que vimos antes
RUN apt-get update && apt-get upgrade -y && rm -rf /var/lib/apt/lists/*
USER app
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Etapa 2: Compilación (Build)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Copiamos y restauramos dependencias (se cachea si el csproj no cambia)
COPY ["WSVenta/WSVenta.csproj", "WSVenta/"]
RUN dotnet restore "WSVenta/WSVenta.csproj"
# Copiamos el resto del código
COPY . .
WORKDIR /src/WSVenta
# Publicamos la app
RUN dotnet build "WSVenta.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "WSVenta.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa 3: Imagen Final (Producción)
FROM base AS final
WORKDIR /app
# Copiamos solo los binarios compilados
COPY --from=build /app/publish .
# Mantenemos el punto de entrada
ENTRYPOINT ["dotnet", "WSVenta.dll"]
