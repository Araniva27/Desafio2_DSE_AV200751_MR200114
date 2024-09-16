## Etapa de construcción
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#WORKDIR /app
#
## Copiar y restaurar dependencias solo para APIGateway
#COPY ["APIGateway/APIGateway.csproj", "APIGateway/"]
#RUN dotnet restore "APIGateway/APIGateway.csproj"
#
## Copiar el resto del código de APIGateway
#COPY . .
#
## Publicar el proyecto APIGateway
#WORKDIR /app/APIGateway
#RUN dotnet publish -c Release -o /out
#
## Etapa de runtime
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
#WORKDIR /app
#COPY --from=build /out .
#
#EXPOSE 7115
#ENTRYPOINT ["dotnet", "APIGateway.dll"]
#
# Dockerfile en APIGateway/
# Dockerfile en APIGateway/
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar el archivo .csproj y restaurar las dependencias
COPY *.csproj .
RUN dotnet restore 

# Copiar el resto del código fuente
COPY . .

# Publicar el proyecto
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out .
EXPOSE 7115
ENTRYPOINT ["dotnet", "APIGateway.dll"]

