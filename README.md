# WSVenta API (.NET 8)

Backend para sistema de ventas utilizando Entity Framework Core y SQL Server en Docker.

## 🚀 Requisitos previos
* Docker 
* .NET 8 SDK

## 🛠️ Configuración del entorno

1. **Levantar la base de datos:**
   ```bash
   docker compose up -d
   ```

2. **Actualizar la base de datos:**
   ```bash
   dotnet ef database update
   ```

3. **Ejecutar la API:**
   ```bash
   dotnet run
   ```