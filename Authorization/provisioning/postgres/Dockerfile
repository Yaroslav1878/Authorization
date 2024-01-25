FROM mcr.microsoft.com/dotnet/sdk:6.0.15
WORKDIR /app

COPY . .
RUN dotnet publish -c Release -o out Authorization.Migrations

ENTRYPOINT [ "dotnet", "/app/out/Authorization.Migrations.dll" ]
