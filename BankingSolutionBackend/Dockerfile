FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY BankingSolution.sln ./

COPY BankingSolution.Application/BankingSolution.Application.csproj BankingSolution.Application/
COPY BankingSolution.Domain/BankingSolution.Domain.csproj BankingSolution.Domain/
COPY BankingSolution.Infrastructure/BankingSolution.Infrastructure.csproj BankingSolution.Infrastructure/
COPY BankingSolution.WebApi/BankingSolution.WebApi.csproj BankingSolution.WebApi/
COPY BankingSolution.Tests.Unit/BankingSolution.Tests.Unit.csproj BankingSolution.Tests.Unit/
COPY BankingSolution.Tests.Integration/BankingSolution.Tests.Integration.csproj BankingSolution.Tests.Integration/
RUN dotnet restore

COPY . .
RUN dotnet publish BankingSolution.WebApi/BankingSolution.WebApi.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
ENV ASPNETCORE_HTTP_PORTS=80
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "BankingSolution.WebApi.dll"]