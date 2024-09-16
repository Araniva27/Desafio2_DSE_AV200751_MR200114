## Etapa de construcción
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#WORKDIR /app
#
## Copiar el archivo .csproj de APIUsuarios al contenedor
#COPY APIUsuarios.csproj ./APIUsuarios/
#
## Restaurar dependencias
#RUN dotnet restore ./APIUsuarios/APIUsuarios.csproj
#
## Copiar el resto del código fuente de APIUsuarios
#COPY . .
#
## Publicar el proyecto APIUsuarios
#WORKDIR /app/APIUsuarios
#RUN dotnet publish -c Release -o /out
#
## Etapa de runtime
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
#WORKDIR /app
#COPY --from=build /out .
#
#EXPOSE 7267
#ENTRYPOINT ["dotnet", "APIUsuarios.dll"]
#
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#WORKDIR /app
#
#COPY *.csproj .
#RUN dotnet restore
#
#COPY . .
#RUN dotnet publish -c Release -o /out
#
##RUN apt-get update && apt-get install -y libgssapi-krb5-2
#
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
#WORKDIR /app
#COPY --from=build /out .
#
#EXPOSE 7267
#
#ENTRYPOINT ["dotnet", "APIUsuarios.dll"]
# Etapa de construcción
# Etapa de construcción
# Etapa de construcción
# Etapa de construcción
# Dockerfile en APIUsuarios/
# Dockerfile en APIUsuarios/
# Dockerfile en APIUsuarios/
# Dockerfile en APIUsuarios/
# Etapa 1: Compilación
# Etapa 1: Construcción
# Etapa 1: Construcción
# Etapa 1: Compilación
# Etapa 1: Construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia el archivo de proyecto de APIUsuarios y los otros proyectos relacionados
COPY *.csproj .
#COPY ../APIUsuarios.Test2/APIUsuarios.Test2.csproj .
#COPY ../APIGateway/APIGateway.csproj .

# Restaurar dependencias para todos los proyectos
RUN dotnet restore ./APIUsuarios/APIUsuarios.csproj

# Copia el resto del código fuente de APIUsuarios y compila la aplicación
COPY . .
WORKDIR /app/APIUsuarios
RUN dotnet publish -c Release -o /out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out .

# Exponer el puerto de APIUsuarios
EXPOSE 7267

# Comando de inicio
ENTRYPOINT ["dotnet", "APIUsuarios.dll"]












