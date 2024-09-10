FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /Infraction.Backend.Image.Service

COPY . .

RUN dotnet tool install --global dotnet-aspnet-codegenerator

EXPOSE 5162

CMD if [ -f *.csproj ]; then \
        dotnet clean Infraction.Backend.Image.Service.csproj; \
        dotnet restore Infraction.Backend.Image.Service.csproj && dotnet build Infraction.Backend.Image.Service.csproj; \
        dotnet run --project Infraction.Backend.Image.Service.csproj; \
    else \
        tail -f /dev/null; \
    fi