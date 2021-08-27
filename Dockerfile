FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://*:5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Searchify.csproj", "./"]
RUN dotnet restore "./Searchify.csproj"
COPY . .
WORKDIR /src/.
RUN dotnet build "Searchify.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Searchify.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "Searchify.dll"]




