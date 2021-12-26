FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/Directory.Core/*.csproj Shared/Directory.Core/
COPY Services/DataCapture/Directory.Services.DataCaptureService/*.csproj Services/DataCapture/Directory.Services.DataCaptureService/
RUN dotnet restore Services/DataCapture/Directory.Services.DataCaptureService/*.csproj
COPY . .
RUN dotnet publish Services/DataCapture/Directory.Services.DataCaptureService/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out/ .
ENTRYPOINT ["dotnet", "Phonebook.Services.DataCapture.dll"]
