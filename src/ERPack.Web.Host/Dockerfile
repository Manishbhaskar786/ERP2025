FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["src/ERPack.Web.Host/ERPack.Web.Host.csproj", "src/ERPack.Web.Host/"]
COPY ["src/ERPack.Web.Core/ERPack.Web.Core.csproj", "src/ERPack.Web.Core/"]
COPY ["src/ERPack.Application/ERPack.Application.csproj", "src/ERPack.Application/"]
COPY ["src/ERPack.Core/ERPack.Core.csproj", "src/ERPack.Core/"]
COPY ["src/ERPack.EntityFrameworkCore/ERPack.EntityFrameworkCore.csproj", "src/ERPack.EntityFrameworkCore/"]
WORKDIR "/src/src/ERPack.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/ERPack.Web.Host", "src/ERPack.Web.Host"]
COPY ["src/ERPack.Web.Core", "src/ERPack.Web.Core"]
COPY ["src/ERPack.Application", "src/ERPack.Application"]
COPY ["src/ERPack.Core", "src/ERPack.Core"]
COPY ["src/ERPack.EntityFrameworkCore", "src/ERPack.EntityFrameworkCore"]
WORKDIR "/src/src/ERPack.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "ERPack.Web.Host.dll"]
