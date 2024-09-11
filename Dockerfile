FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /Invoice.Service

COPY . .

RUN dotnet tool install --global dotnet-aspnet-codegenerator

EXPOSE 5162

CMD if [ -f *.csproj ]; then \
        dotnet clean Invoice.Service.csproj; \
        dotnet restore Invoice.Service.csproj && dotnet build Invoice.Service.csproj; \
        dotnet run --project Invoice.Service.csproj; \
    else \
        tail -f /dev/null; \
    fi