FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli
RUN ln -s ~/.dotnet/tools/libman /usr/local/bin/libman

WORKDIR /src
COPY ["src/ERPack.Web.Mvc/ERPack.Web.Mvc.csproj", "src/ERPack.Web.Mvc/"]
COPY ["src/ERPack.Web.Core/ERPack.Web.Core.csproj", "src/ERPack.Web.Core/"]
COPY ["src/ERPack.Application/ERPack.Application.csproj", "src/ERPack.Application/"]
COPY ["src/ERPack.Core/ERPack.Core.csproj", "src/ERPack.Core/"]
COPY ["src/ERPack.EntityFrameworkCore/ERPack.EntityFrameworkCore.csproj", "src/ERPack.EntityFrameworkCore/"]
WORKDIR "/src/src/ERPack.Web.Mvc"
RUN dotnet restore 

WORKDIR /src
COPY ["src/ERPack.Web.Mvc", "src/ERPack.Web.Mvc"]
COPY ["src/ERPack.Web.Core", "src/ERPack.Web.Core"]
COPY ["src/ERPack.Application", "src/ERPack.Application"]
COPY ["src/ERPack.Core", "src/ERPack.Core"]
COPY ["src/ERPack.EntityFrameworkCore", "src/ERPack.EntityFrameworkCore"]
WORKDIR "/src/src/ERPack.Web.Mvc"
RUN libman restore
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "ERPack.Web.Mvc.dll"]
