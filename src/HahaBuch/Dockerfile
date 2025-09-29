# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0-noble AS base
#FROM mcr.microsoft.com/dotnet/aspnet:9.0-noble-chiseled-extra AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/HahaBuch/HahaBuch.csproj", "HahaBuch/"]
COPY ["src/HahaBuch.Client/HahaBuch.Client.csproj", "HahaBuch.Client/"]
COPY ["src/HahaBuch.Shared/HahaBuch.Shared.csproj", "HahaBuch.Shared/"]
RUN dotnet restore "./HahaBuch/HahaBuch.csproj"
COPY ["src", "."]
WORKDIR "/src/HahaBuch"
RUN dotnet build "./HahaBuch.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HahaBuch.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
RUN mkdir -p /data && chown $APP_UID /data
USER $APP_UID
COPY --from=publish /app/publish .
VOLUME /data
ENTRYPOINT ["dotnet", "HahaBuch.dll"]