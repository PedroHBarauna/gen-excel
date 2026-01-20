FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY GenExcel.slnx ./
COPY NuGet.Config ./

COPY src/GenExcel.API/GenExcel.API.csproj src/GenExcel.API/
COPY src/GenExcel.Application/GenExcel.Application.csproj src/GenExcel.Application/
COPY src/GenExcel.Domain/GenExcel.Domain.csproj src/GenExcel.Domain/
COPY src/GenExcel.Infrastructure/GenExcel.Infrastructure.csproj src/GenExcel.Infrastructure/

RUN dotnet restore src/GenExcel.API/GenExcel.API.csproj --configfile ./NuGet.Config

COPY src ./src

RUN dotnet publish src/GenExcel.API/GenExcel.API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "GenExcel.API.dll"]
