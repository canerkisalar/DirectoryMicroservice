FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /app
EXPOSE 80
COPY Shared/Phonebook.Core/*.csproj Shared/Phonebook.Core/
COPY Services/Phonebook/Directory.Services.Phonebook/*.csproj Services/Phonebook/Directory.Services.Phonebook/
RUN dotnet restore Services/Phonebook/Directory.Services.Phonebook/*.csproj
COPY . .
RUN dotnet publish Services/Phonebook/Directory.Services.Phonebook/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim as runtime
WORKDIR /app
COPY --from=build /app/out/ .
ENTRYPOINT ["dotnet", "Phonebook.Services.Phonebook.dll"]