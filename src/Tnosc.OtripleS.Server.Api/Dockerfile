# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["src/Tnosc.OtripleS.Server.Api/Tnosc.OtripleS.Server.Api.csproj", "src/Tnosc.OtripleS.Server.Api/"]
COPY ["lib/Tnosc.Lib.Api/Tnosc.Lib.Api.csproj", "lib/Tnosc.Lib.Api/"]
COPY ["src/Tnosc.OtripleS.Server.Infrastructure/Tnosc.OtripleS.Server.Infrastructure.csproj", "src/Tnosc.OtripleS.Server.Infrastructure/"]
COPY ["src/Tnosc.OtripleS.Server.Application/Tnosc.OtripleS.Server.Application.csproj", "src/Tnosc.OtripleS.Server.Application/"]
COPY ["src/Tnosc.OtripleS.Server.Domain/Tnosc.OtripleS.Server.Domain.csproj", "src/Tnosc.OtripleS.Server.Domain/"]
COPY ["lib/Tnosc.Lib.Domain/Tnosc.Lib.Domain.csproj", "lib/Tnosc.Lib.Domain/"]
RUN dotnet restore "./src/Tnosc.OtripleS.Server.Api/Tnosc.OtripleS.Server.Api.csproj"
COPY . .
WORKDIR "/src/src/Tnosc.OtripleS.Server.Api"
RUN dotnet build "./Tnosc.OtripleS.Server.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tnosc.OtripleS.Server.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tnosc.OtripleS.Server.Api.dll"]