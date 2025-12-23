FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY NoteApplication/NoteApplication.csproj NoteApplication/
RUN dotnet restore NoteApplication/NoteApplication.csproj

COPY NoteApplication/ NoteApplication/
RUN dotnet publish NoteApplication/NoteApplication.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NoteApplication.dll"]