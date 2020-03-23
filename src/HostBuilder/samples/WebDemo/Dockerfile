#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS base
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-bionic AS build
WORKDIR /src
COPY ["samples/WebDemo/WebDemo.csproj", "samples/WebDemo/"]
COPY ["src/Extensions.HostBuilder/Extensions.HostBuilder.csproj", "src/Extensions.HostBuilder/"]
RUN dotnet restore "samples/WebDemo/WebDemo.csproj"
COPY . .
WORKDIR "/src/samples/WebDemo"
RUN dotnet build "WebDemo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebDemo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebDemo.dll"]
