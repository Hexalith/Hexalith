#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["src/Programs/Server/Hexalith.Server.csproj", "src/Programs/Server/"]
COPY ["src/Modules/Server/Hexalith.QuartzScheduler.Server/Hexalith.QuartzScheduler.Server.csproj", "src/Modules/Server/Hexalith.QuartzScheduler.Server/"]
COPY ["src/Modules/Shared/Hexalith.QuartzScheduler.Shared/Hexalith.QuartzScheduler.Shared.csproj", "src/Modules/Shared/Hexalith.QuartzScheduler.Shared/"]
COPY ["src/Core/Domain/Hexalith.Domain.Abstractions/Hexalith.Domain.Abstractions.csproj", "src/Core/Domain/Hexalith.Domain.Abstractions/"]
COPY ["src/Core/Application/Hexalith.Application.Abstractions/Hexalith.Application.Abstractions.csproj", "src/Core/Application/Hexalith.Application.Abstractions/"]
COPY ["src/Core/Infrastructure/Hexalith.Infrastructure.WebServer/Hexalith.Infrastructure.WebServer.csproj", "src/Core/Infrastructure/Hexalith.Infrastructure.WebServer/"]
COPY ["src/Core/Infrastructure/Hexalith.Infrastructure.Abstractions/Hexalith.Infrastructure.Abstractions.csproj", "src/Core/Infrastructure/Hexalith.Infrastructure.Abstractions/"]
COPY ["src/Core/Infrastructure/Hexalith.Infrastructure.Host/Hexalith.Infrastructure.Host.csproj", "src/Core/Infrastructure/Hexalith.Infrastructure.Host/"]
COPY ["src/Core/Application/Hexalith.Application/Hexalith.Application.csproj", "src/Core/Application/Hexalith.Application/"]
COPY ["src/Core/Infrastructure/Hexalith.Infrastructure.Modules/Hexalith.Infrastructure.Modules.csproj", "src/Core/Infrastructure/Hexalith.Infrastructure.Modules/"]
COPY ["src/Modules/Server/Hexalith.WorkItems.Server/Hexalith.WorkItems.Server.csproj", "src/Modules/Server/Hexalith.WorkItems.Server/"]
COPY ["src/Modules/Shared/Hexalith.WorkItems.Shared/Hexalith.WorkItems.Shared.csproj", "src/Modules/Shared/Hexalith.WorkItems.Shared/"]
COPY ["src/Programs/Client/Hexalith.Client.csproj", "src/Programs/Client/"]
COPY ["src/Modules/Client/Hexalith.Units.Client/Hexalith.Units.Client.csproj", "src/Modules/Client/Hexalith.Units.Client/"]
COPY ["src/Modules/Client/Hexalith.WorkItems.Client/Hexalith.WorkItems.Client.csproj", "src/Modules/Client/Hexalith.WorkItems.Client/"]
COPY ["src/Core/Infrastructure/Hexalith.Infrastructure.BlazorClient/Hexalith.Infrastructure.BlazorClient.csproj", "src/Core/Infrastructure/Hexalith.Infrastructure.BlazorClient/"]
COPY ["src/Modules/Client/Hexalith.OpenIdDict.Client/Hexalith.OpenIdDict.Client.csproj", "src/Modules/Client/Hexalith.OpenIdDict.Client/"]
COPY ["src/Modules/Shared/Hexalith.OpenIdDict.Shared/Hexalith.OpenIdDict.Shared.csproj", "src/Modules/Shared/Hexalith.OpenIdDict.Shared/"]
COPY ["src/Modules/Server/Hexalith.OpenIdDict.Server/Hexalith.OpenIdDict.Server.csproj", "src/Modules/Server/Hexalith.OpenIdDict.Server/"]
RUN dotnet restore "src/Programs/Server/Hexalith.Server.csproj"
COPY . .
WORKDIR "/src/src/Programs/Server"
RUN dotnet build "Hexalith.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hexalith.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["/Certificates/HexalithEncryptionCertificate.pfx", "app/publish/"]
COPY ["/Certificates/HexalithSigningCertificate.pfx", "app/publish/"]
ENTRYPOINT ["dotnet", "Hexalith.Server.dll"]