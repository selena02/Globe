FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["API/API.csproj", "API/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "API/API.csproj"

COPY ["API/.", "API/"]
COPY ["Application/.", "Application/"]
COPY ["Infrastructure/.", "Infrastructure/"]
COPY ["Domain/.", "Domain/"]

WORKDIR "/src/API"
RUN dotnet build "API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]