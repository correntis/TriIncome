# Используем SDK образ для сборки
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["LoanApi.csproj", "./"]
RUN dotnet restore "LoanApi.csproj"

COPY . .
RUN dotnet build "LoanApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LoanApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

ENTRYPOINT ["dotnet", "LoanApi.dll"]

