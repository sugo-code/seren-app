#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SerenApp.Web/SerenApp.Web.csproj", "SerenApp.Web/"]
RUN dotnet restore "SerenApp.Web/SerenApp.Web.csproj"
COPY . .
WORKDIR "/src/SerenApp.Web"
RUN dotnet build "SerenApp.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SerenApp.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SerenApp.Web.dll"]
