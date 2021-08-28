FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /src

COPY ["Searchify.csproj", "./"]

RUN dotnet restore --disable-parallel "./Searchify.csproj"

COPY . .

WORKDIR /src/.

RUN dotnet build "Searchify.csproj" -c Release -o /app/build

RUN dotnet publish "Searchify.csproj" -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS final

ENV ASPNETCORE_URLS=http://0.0.0.0:5000

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 5000

ENTRYPOINT [ "dotnet", "Searchify.dll"]




