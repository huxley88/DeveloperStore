# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj", "src/Ambev.DeveloperEvaluation.WebApi/"]
RUN dotnet restore "src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj"

COPY . .

WORKDIR /src/src/Ambev.DeveloperEvaluation.WebApi
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

COPY ./aspnetapp.pfx /app/aspnetapp.pfx

ENV ASPNETCORE_URLS=https://+:5001;http://+:5000
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=YourPassword123

EXPOSE 5000
EXPOSE 5001

ENTRYPOINT ["dotnet", "Ambev.DeveloperEvaluation.WebApi.dll"]
