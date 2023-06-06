FROM mcr.microsoft.com/dotnet/sdk:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN mkdir /app/Images

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["LibraryApi/LibraryApi.csproj", "LibraryApi/"]
RUN dotnet restore "LibraryApi/LibraryApi.csproj"
COPY . .
WORKDIR "/src/LibraryApi"
RUN dotnet build "LibraryApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LibraryApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
VOLUME ["/app/Images"]
ENTRYPOINT ["dotnet", "LibraryApi.dll"]
