# Copy every then delete everything except project files used for restore 
FROM mcr.microsoft.com/dotnet/sdk:6.0-jammy AS projects-only
WORKDIR /src
COPY . .
RUN find . -name NuGet.Config -o -name "*.sln" -prune -o \! -type d \! -name \*.csproj -exec rm -f '{}' + \
     && find . -depth -type d -empty -exec rmdir '{}' \;

# copy project files and restore first to take advantage of cache
FROM mcr.microsoft.com/dotnet/sdk:6.0-jammy AS build
WORKDIR /src
COPY --from=projects-only /src .
RUN dotnet restore;
COPY . . 
RUN dotnet build --no-restore

FROM build AS publish
WORKDIR /src
RUN dotnet publish ./MLDB.Api --no-restore -c Release -o /out /p:UseAppHost=false && \
    rm /out/*.pdb

FROM mcr.microsoft.com/dotnet/aspnet:6.0-jammy AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

COPY --from=publish /out .
ENTRYPOINT ["dotnet", "MLDB.Api.dll"]
