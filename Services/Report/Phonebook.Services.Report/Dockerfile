FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/Directory.Core/*.csproj Shared/Directory.Core/
COPY Services/Report/Phonebook.Services.Report/*.csproj Services/Report/Phonebook.Services.Report/
RUN dotnet restore Services/Report/Phonebook.Services.Report/*.csproj
COPY . .
RUN dotnet publish Services/Report/Phonebook.Services.Report/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out/ .
ENTRYPOINT ["dotnet", "Phonebook.Services.Report.dll"]
